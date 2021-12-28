using System;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.API.Common;
using CyberSaloon.Core.BLL.Artists;
using CyberSaloon.Core.DTO.Artists;
using CyberSaloon.Core.DTO.Artists.Validation;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.Repo.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CyberSaloon.Core.API.Artists
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsController : CyberSaloonController
    {
        private readonly IArtistsService _artistsService;
        private readonly IArtistsDTOValidator _validator;
        private readonly IMapper _mapper;

        public ArtistsController(
                IArtistsService service, 
                IArtistsDTOValidator validator,
                IMapper mapper
            )
        {
            _artistsService = 
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

            var existingApplicant = _artistsService.ReadByUserId(userId);
                
            if(existingApplicant == null)
                return NotFound();

            return Ok(existingApplicant);
        }

        [HttpGet("{artistId}")]
        public IActionResult GetArtist(Guid artistId)
        {
            var artist = _artistsService.Read(artistId);

            if(artist == null)
                return NotFound();

            var result = _mapper.Map<ArtistGetDTO>(artist);

            return Ok(result);
        }

        [HttpGet("profile/{artistAlias}")]
        public IActionResult GetArtistByAlias(string artistAlias)
        {
            var artist = _artistsService.ReadByAlias(artistAlias);

            if(artist == null)
                return NotFound();

            var result = _mapper.Map<ArtistGetDTO>(artist);

            return Ok(result);
        }

        [HttpGet(Name = nameof(GetArtists))]
        public IActionResult GetArtists([FromQuery] ResourceParameters parameters)
        {
            var validationResults = _validator.Validate(parameters);

            if (!validationResults.IsValid)
                return BadRequest(validationResults);

            var artists = _artistsService.ReadAll();

            var pagination =
                new Pagination<ArtistGetDTO>(
                    artists.Select(_mapper.Map<ArtistGetDTO>),
                    parameters.PageSize,
                    parameters.PageNumber
                );

            if (!pagination.Entities.Any())
                return NotFound();
            
            IntroduceMetadata<ArtistGetDTO>(
                    nameof(GetArtists), 
                    parameters, 
                    pagination
                );

            return Ok(pagination);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostArtist(ArtistPostDTO payload)
        {
            var validationResults = _validator.Validate(payload);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            var userId = User.GetSub();

            var existingArtist = _artistsService.ReadByUserId(userId);

            if(existingArtist != null)
            {
                var existingartistDTO = _mapper.Map<ArtistGetDTO>(existingArtist);
                return CreatedAtAction(
                    nameof(GetArtist),
                    new { artistId = existingartistDTO.Id },
                    existingartistDTO
                );
            }

            var artist = _mapper.Map<Artist>(payload);
            artist.Id = userId;
            artist.UserId = userId;
            
            _artistsService.Create(artist);

            var serviceResult = _mapper.Map<ArtistGetDTO>(artist);

            return CreatedAtAction(
                nameof(GetArtist),
                new { artistId = serviceResult.Id },
                serviceResult
            );
        }

        [HttpPatch]
        [Authorize]
        public ActionResult PatchArtist(JsonPatchDocument<ArtistPatchDTO> payload) 
        {
            var userId = User.GetSub();

            var artist = _artistsService.ReadByUserId(userId);

            if(artist == null)
                return NotFound();

            var dto = _mapper.Map<ArtistPatchDTO>(artist);

            payload.ApplyTo(dto);

            var validationResults = _validator.Validate(dto);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            artist = _mapper.Map<Artist>(dto);

            _artistsService.Update(artist);
            
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteArtist()
        {
            var userId = User.GetSub();

            var artist = _artistsService.ReadByUserId(userId);

            if(artist == null)
                return NotFound();

            _artistsService.Delete(artist.Id);

            return NoContent();
        }

        [HttpOptions]
        public ActionResult ArtistOptions() 
        {
            Response.Headers.Add("Allow", "GET, POST, PATCH, DELETE, OPTIONS");
            return Ok();
        }
    }
}