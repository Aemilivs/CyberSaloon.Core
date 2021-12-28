using System;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.API.Common;
using CyberSaloon.Core.BLL.Applicants;
using CyberSaloon.Core.DTO.Applicants;
using CyberSaloon.Core.DTO.Applicants.Validation;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CyberSaloon.Core.API.Applicants
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantsController : CyberSaloonController
    {
        private readonly IApplicantsService _service;
        private readonly IApplicantsDTOValidator _validator;
        private readonly IMapper _mapper;

        public ApplicantsController(
                IApplicantsService service, 
                IApplicantsDTOValidator validator,
                IMapper mapper
            )
        {
            _service = 
                service ?? 
                throw new ArgumentNullException(nameof(service));

            _validator = 
                validator ?? 
                throw new ArgumentNullException(nameof(validator));

            _mapper = 
                mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("self")]
        [Authorize]
        public IActionResult GetApplicant()
        {
            var userId = User.GetSub();

            var existingApplicant = _service.ReadByUserId(userId);
                
            if(existingApplicant == null)
                return NotFound();

            return Ok(existingApplicant);
        }

        [HttpGet("{applicantId}")]
        public IActionResult GetApplicant(Guid applicantId)
        {
            var applicant = _service.Read(applicantId);

            if(applicant == null)
                return NotFound();

            var result = _mapper.Map<ApplicantGetDTO>(applicant);

            return Ok(result);
        }

        [HttpGet("profile/{applicantAlias}")]
        public IActionResult GetApplicantByAlias(string applicantAlias)
        {
            var applicant = _service.ReadByAlias(applicantAlias);

            if(applicant == null)
                return NotFound();

            var result = _mapper.Map<ApplicantGetDTO>(applicant);

            return Ok(result);
        }

        [HttpGet(Name = nameof(GetApplicant))]
        public IActionResult GetApplicants([FromQuery] ResourceParameters parameters)
        {
            var validationResults = _validator.Validate(parameters);

            if (!validationResults.IsValid)
                return BadRequest(validationResults);

            var applicants = _service.ReadAll();

            var pagination =
                new Pagination<ApplicantGetDTO>(
                        applicants.Select(_mapper.Map<ApplicantGetDTO>), 
                        parameters.PageSize, 
                        parameters.PageNumber
                    );

            if (!pagination.Entities.Any())
                return NotFound();
            
            IntroduceMetadata<ApplicantGetDTO>(
                    nameof(GetApplicant), 
                    parameters, 
                    pagination
                );
                    
            return Ok(pagination);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostApplicant(ApplicantPostDTO payload)
        {
            var validationResults = _validator.Validate(payload);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            var userId = User.GetSub();

            var existingApplicant = _service.ReadByUserId(userId);

            if(existingApplicant != null)
            {
                var existingApplicantDTO = _mapper.Map<ApplicantGetDTO>(existingApplicant);
                return CreatedAtAction(
                    nameof(GetApplicant),
                    new { applicantId = existingApplicantDTO.Id },
                    existingApplicantDTO
                );
            }

            var applicant = _mapper.Map<Applicant>(payload);
            applicant.Id = userId;
            applicant.UserId = userId;

            _service.Create(applicant);

            var createdApplicantDTO = _mapper.Map<ApplicantGetDTO>(applicant);

            return CreatedAtAction(
                nameof(GetApplicant),
                new { applicantId = createdApplicantDTO.Id },
                createdApplicantDTO
            );
        }

        [HttpPatch]
        [Authorize]
        public ActionResult PatchApplicant(JsonPatchDocument<ApplicantPatchDTO> payload) 
        {
            var userId = User.GetSub();

            var applicant = _service.ReadByUserId(userId);

            if(applicant == null)
                return NotFound();
        
            var dto = _mapper.Map<ApplicantPatchDTO>(applicant);

            payload.ApplyTo(dto);

            var validationResults = _validator.Validate(dto);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            applicant = _mapper.Map<Applicant>(dto);

            _service.Update(applicant);
            
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteApplicant()
        {
            var userId = User.GetSub();

            var applicant = _service.ReadByUserId(userId);

            if(applicant == null)
                return NotFound();

            _service.Delete(applicant.Id);

            return NoContent();
        }

        [HttpOptions]
        public ActionResult ApplicantOptions() 
        {
            Response.Headers.Add("Allow", "GET, POST, PATCH, DELETE, OPTIONS");
            return Ok();
        }
    }
}