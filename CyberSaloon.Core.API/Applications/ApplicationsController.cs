using System;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.API.Common;
using CyberSaloon.Core.BLL.Applicants;
using CyberSaloon.Core.BLL.Applications;
using CyberSaloon.Core.BLL.Artists;
using CyberSaloon.Core.DTO.Applications;
using CyberSaloon.Core.DTO.Applications.Validation;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CyberSaloon.Core.API.Controllers
{
    /// <summary>
    /// Controller for user applications.
    /// Applications represent requests towards artists for creating specific pieces of art.
    /// Applications can receive tokens of popularity from other applicants also known as likes, 
    /// so that they could be prioritized in the queue of applications for artists, 
    /// so that artists have understanding of what requests are more demanded.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController : CyberSaloonController
    {
        private readonly IApplicationsService _applicationsService;
        private readonly IApplicantsService _applicantsService;
        private readonly IArtistsService _artistsService;
        private readonly IApplicationsDTOValidator _validator;

        private readonly IMapper _mapper;

        public ApplicationsController(
                IApplicationsService applicationsService, 
                IApplicantsService applicantsService,
                IArtistsService artistsService,
                IApplicationsDTOValidator validator,
                IMapper mapper
            )
        {
            _applicationsService = 
                applicationsService ?? 
                throw new ArgumentNullException(nameof(applicationsService));

            _applicantsService = 
                applicantsService ?? 
                throw new ArgumentNullException(nameof(applicantsService));

            _artistsService =
                artistsService ??
                throw new ArgumentNullException(nameof(artistsService));

            _validator = 
                validator ?? 
                throw new ArgumentNullException(nameof(validator));

            _mapper = 
                mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{applicationId}")]
        public IActionResult GetApplication(Guid applicationId)
        {
            var application = _applicationsService.Read(applicationId);

            if(application == null)
                return NotFound();

            var result = _mapper.Map<ApplicationGetDTO>(application);

            return Ok(result);
        }

        /// <summary>
        /// Fetch all of applications without any specific filter.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetApplications([FromQuery] ResourceParameters parameters)
        {
            var validationResults = _validator.Validate(parameters);

            if (!validationResults.IsValid)
                return BadRequest(validationResults);

            var applications = _applicationsService.ReadAll();

            var pagination =
                new Pagination<ApplicationGetDTO>(
                        applications.Select(_mapper.Map<ApplicationGetDTO>),
                        parameters.PageSize,
                        parameters.PageNumber
                    );

            if (!pagination.Entities.Any())
                return NotFound();
            
            IntroduceMetadata<ApplicationGetDTO>(
                    nameof(GetApplications), 
                    parameters, 
                    pagination
                );

            return Ok(pagination);
        }

        
        [HttpGet("requested")]
        public IActionResult GetRequestedApplications(
                [FromQuery] ResourceParameters parameters
            ) => 
            GetApplicationsWithPredicate(
                parameters,
                it => !it.Fullfilled,
                // TODO: Change to sort by date of posting.
                it => it.Supporters.Count
            );

        [HttpGet("by-author/{applicantId}")]
        public IActionResult GetApplicationsByAuthor(
                [FromQuery] ResourceParameters parameters, 
                Guid applicantId
            ) => 
            GetApplicationsWithPredicate(
                parameters,
                it => it.Applicant.Id == applicantId,
                it => it.Supporters.Count
            );

        [HttpGet("by-artist/{artistId}")]
        public IActionResult GetApplicationsByArtist(
                [FromQuery] ResourceParameters parameters, 
                Guid artistId
            ) => 
            GetApplicationsWithPredicate(
                parameters,
                it => it.Artist.Id == artistId,
                it => it.Supporters.Count
            );

        [HttpGet("best")]
        public IActionResult GetBestApplications(
                [FromQuery] ResourceParameters parameters
            ) => 
            GetApplicationsWithPredicate(
                parameters,
                it => true,
                it => it.Supporters.Count
            );

        [HttpGet("all-fullfilled")]
        public IActionResult GetFullfilledApplications(
                [FromQuery] ResourceParameters parameters
            ) => 
            GetApplicationsWithPredicate(
                parameters,
                it => it.Fullfilled,
                it => it.Supporters.Count
            );

        [HttpGet("submitted/{applicantId}")]
        public IActionResult GetSubmittedApplications(
                [FromQuery] ResourceParameters parameters,
                Guid applicantId
            )
        {
            var applicant = _applicantsService.Read(applicantId);
                
            if(applicant == null)
                return Unauthorized();

            if (!applicant.Supported.Any())
                return NotFound();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ApplicantId == applicant.Id,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("submitted")]
        [Authorize]
        public IActionResult GetSubmittedApplications(
                [FromQuery] ResourceParameters parameters
            )
        {
            var userId = User.GetSub();

            var applicant = _applicantsService.ReadByUserId(userId);
                
            if(applicant == null)
                return Unauthorized();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ApplicantId == applicant.Id,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("fulfilled/{artistId}")]
        public IActionResult GetFulfilledApplications(
                [FromQuery] ResourceParameters parameters,
                Guid artistId
            )
        {
            var artist = _artistsService.Read(artistId);
                
            if(artist == null)
                return Unauthorized();

            if(!artist.Applications.Any())
                return NotFound();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ArtistId == artist.Id && it.Art != default,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("fulfilled")]
        [Authorize]
        public IActionResult GetFulfilledApplications(
                [FromQuery] ResourceParameters parameters
            )
        {
            var userId = User.GetSub();

            var artist = _artistsService.ReadByUserId(userId);
                
            if(artist == null)
                return Unauthorized();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ArtistId == artist.Id && it.Art != default,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("pending/{artistId}")]
        public IActionResult GetPendingApplications(
                [FromQuery] ResourceParameters parameters,
                Guid artistId
            )
        {
            var artist = _artistsService.Read(artistId);
                
            if(artist == null)
                return Unauthorized();

            if(!artist.Applications.Any())
                return NotFound();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ArtistId == artist.Id && it.Art == default,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("pending")]
        [Authorize]
        public IActionResult GetPendingApplications(
                [FromQuery] ResourceParameters parameters
            )
        {
            var userId = User.GetSub();

            var artist = _artistsService.ReadByUserId(userId);
                
            if(artist == null)
                return Unauthorized();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.ArtistId == artist.Id && it.Art == default,
                    it => it.Supporters.Count
                );
        }

        [HttpGet("liked/{applicantId}")]
        public IActionResult GetLikedApplications(
                [FromQuery] ResourceParameters parameters,
                Guid applicantId
            )
        {
            var applicant = _applicantsService.Read(applicantId);
                
            if(applicant == null)
                return Unauthorized();

            if (!applicant.Supported.Any())
                return NotFound();
            
            var pagination = 
                new Pagination<ApplicationGetDTO>(
                        applicant.Supported.Select(_mapper.Map<ApplicationGetDTO>), 
                        parameters.PageSize,
                        parameters.PageNumber
                    );

            IntroduceMetadata<ApplicationGetDTO>(
                    nameof(GetApplications), 
                    parameters, 
                    pagination
                );

            return Ok(pagination);
        }   

        [HttpGet("liked")]
        [Authorize]
        public IActionResult GetLikedApplications(
                [FromQuery] ResourceParameters parameters
            )
        {
            var userId = User.GetSub();

            var applicant = _applicantsService.ReadByUserId(userId);
                
            if(applicant == null)
                return Unauthorized();

            return
                GetApplicationsWithPredicate(
                    parameters,
                    it => it.Supporters.Contains(applicant),
                    it => it.Supporters.Count
                );
        }           

        /// <summary>
        /// Fetch the applications, filter and sort them by the given parameters 
        /// and return an action result.
        /// </summary>
        /// <param name="parameters">Page number and page size of a current query.</param>
        /// <param name="filter">Function to filter applications.</param>
        /// <param name="sort">Function to sort applications.</param>
        /// <typeparam name="T">Type of the key used for sorting.</typeparam>
        /// <returns>IActionResult containing searched applications.</returns>
        private IActionResult GetApplicationsWithPredicate<T>(
                ResourceParameters parameters,
                Func<Application, bool> filter,
                Func<Application, T> sort
            )
        {
            var validationResults = _validator.Validate(parameters);

            if (!validationResults.IsValid)
                return BadRequest(validationResults);

            var applications = 
                _applicationsService
                    .ReadAll()
                    .Where(filter)
                    .OrderByDescending(sort);

            if (!applications.Any())
                return NotFound();
            
            var pagination = 
                new Pagination<ApplicationGetDTO>(
                        applications.Select(_mapper.Map<ApplicationGetDTO>), 
                        parameters.PageSize,
                        parameters.PageNumber
                    );

            IntroduceMetadata<ApplicationGetDTO>(
                    nameof(GetApplications), 
                    parameters, 
                    pagination
                );

            return Ok(pagination);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostApplication(ApplicationPostDTO payload)
        {
            var userId = User.GetSub();

            var applicant = _applicantsService.ReadByUserId(userId);

            if(applicant == null)
                return Unauthorized();
            
            payload.Author = applicant.Alias;
            
            var validationResults = _validator.Validate(payload);
            
            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);
            
            var application = _mapper.Map<Application>(payload);
            
            _applicationsService.Create(application);

            var serviceResult = _mapper.Map<ApplicationGetDTO>(application);

            return CreatedAtAction(
                nameof(GetApplication),
                new { applicationId = serviceResult.Id },
                serviceResult
            );
        }

        [HttpPatch]
        [Route("{applicationId}")]
        [Authorize]
        public ActionResult PatchApplication(Guid applicationId, JsonPatchDocument<Application> payload) 
        {
            var application = _applicationsService.Read(applicationId);

            if(application == null)
                return NotFound();

            var userId = User.GetSub();

            var applicant = _applicantsService.ReadByUserId(userId);

            if(applicant == null)
                return Unauthorized();

            if(applicant.Id != application.ApplicantId)
                return Unauthorized();

            var validationResults = _validator.Validate(payload);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            payload.ApplyTo(application);

            _applicationsService.Update(application);
            
            return NoContent();
        }

        [HttpDelete]
        [Route("{applicationId}")]
        [Authorize]
        public IActionResult DeleteApplication(Guid applicationId)
        {
            var application = _applicationsService.Read(applicationId);

            if(application == null)
                return NotFound();
            
            var userId = User.GetSub();

            var applicant = _applicantsService.ReadByUserId(userId);

            if(applicant == null)
                return Unauthorized();

            if(application.Applicant != applicant)
                return Unauthorized();

            if(application.Fullfilled)
                return Conflict("Application is fullfilled and can't be deleted as it is shared with an author of an art now.");

            _applicationsService.Delete(applicationId);

            return NoContent();
        }

        [HttpGet("apply/{applicationId}")]
        [Authorize]
        public ActionResult Apply(Guid applicationId) 
        {
            var application = _applicationsService.Read(applicationId);

            if(application == null)
                return NotFound("Application with such id does not exist.");

            var userId = User.GetSub();
            var applicant = _applicantsService.ReadByUserId(userId);
                
            if(applicant == null)
                return Unauthorized("User with such id does not exist.");

            if(application.Supporters?.Any(it => it.Id == applicant.Id) ?? false)
                return BadRequest("User is already applied to this application.");

            _applicationsService.Apply(application, applicant);

            return Ok();
        }

        [HttpGet("defy/{applicationId}")]
        [Authorize]
        public ActionResult Defy(Guid applicationId) 
        {
            var application = _applicationsService.Read(applicationId);

            if(application == null)
                return NotFound("Application with such id does not exist.");

            var userId = User.GetSub();
            var applicant = _applicantsService.ReadByUserId(userId);
                
            if(applicant == null)
                return Unauthorized("User with such id does not exist.");

            if(application.Supporters?.All(it => it.Id != applicant.Id) ?? false)
                return BadRequest("User is not applied to this application.");

            _applicationsService.Defy(application, applicant);

            return Ok();
        }

        [HttpOptions]
        public ActionResult ApplicationOptions() 
        {
            Response.Headers.Add("Allow", "GET, POST, PATCH, DELETE, OPTIONS");
            return Ok();
        }
    }
}