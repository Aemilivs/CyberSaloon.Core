using System;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.API.Common;
using CyberSaloon.Core.BLL.Artists;
using CyberSaloon.Core.BLL.Arts;
using CyberSaloon.Core.DTO.Arts;
using CyberSaloon.Core.DTO.Arts.Validation;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Arts;
using CyberSaloon.Core.Repo.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CyberSaloon.Core.API.Arts
{
    [ApiController]
    [Route("[controller]")]
    public class ArtsController : CyberSaloonController
    {
        private readonly IArtsService _artsService;
        private readonly IArtsDTOValidator _artsValidator;
        private readonly IArtistsService _artistsService;
        private readonly IMapper _mapper;

        public ArtsController(
                IArtsService artService, 
                IArtsDTOValidator artsValidator,
                IArtistsService artistsService, 
                IMapper mapper
            )
        {
            _artsService = 
                artService ?? 
                throw new ArgumentNullException(nameof(artService));

            _artsValidator = 
                artsValidator ?? 
                throw new ArgumentNullException(nameof(artsValidator));
            
            _artistsService =
                artistsService ??
                throw new ArgumentNullException(nameof(artistsService));

            _mapper = 
                mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        // TODO: Finish other domains
        // TODO: Introduce authorization
        // TODO: Introduce unit tests

        [HttpGet("{artId}")]
        public IActionResult GetArt(Guid artId)
        {
            var art = _artsService.Read(artId);

            if(art == null)
                return NotFound();

            var result = _mapper.Map<ArtGetDTO>(art);

            return Ok(result);
        }

        [HttpGet(Name = nameof(GetArts))]
        public IActionResult GetArts([FromQuery] ResourceParameters parameters)
        {
            var validationResults = _artsValidator.Validate(parameters);

            if (!validationResults.IsValid)
                return BadRequest(validationResults);

            var arts = _artsService.ReadAll();

            var pagination = 
                new Pagination<ArtGetDTO>(
                    arts.Select(_mapper.Map<ArtGetDTO>),
                    parameters.PageSize,
                    parameters.PageNumber
                );

            if (!pagination.Entities.Any())
                return NotFound();
            
            IntroduceMetadata<ArtGetDTO>(
                    nameof(GetArts), 
                    parameters, 
                    pagination
                );

            return Ok(pagination);
        }

        [HttpPost]
        public IActionResult PostArt(ArtPostDTO payload)
        {
            var validationResults = _artsValidator.Validate(payload);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            var art = _mapper.Map<Art>(payload);
            art.Application.Art = art;
            _artsService.Create(art);

            var serviceResult = _mapper.Map<ArtGetDTO>(art);

            return CreatedAtAction(
                nameof(GetArt),
                new { artId = serviceResult.Id },
                serviceResult
            );
        }

        [HttpPatch]
        [Route("{artId}")]
        public ActionResult PatchArt(Guid artId, JsonPatchDocument<Art> payload) 
        {
            var art = _artsService.Read(artId);

            if(art == null)
                return NotFound();

            var userId = User.GetSub();

            var artist = _artistsService.ReadByUserId(userId);

            if(artist == null)
                return Unauthorized();

            if(artist.Id != art.Application.ArtistId)
                return Unauthorized();

            var validationResults = _artsValidator.Validate(payload);

            if(!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            payload.ApplyTo(art);

            _artsService.Update(art);
            
            return NoContent();
        }

        [HttpDelete]
        [Route("{artId}")]
        public IActionResult DeleteArt(Guid artId)
        {
            var art = _artsService.Read(artId);

            if(art == null)
                return NotFound();

            _artsService.Delete(artId);

            return NoContent();
        }

        [HttpOptions]
        public ActionResult ArtOptions() 
        {
            Response.Headers.Add("Allow", "GET, POST, PATCH, DELETE, OPTIONS");
            return Ok();
        }
    }
}