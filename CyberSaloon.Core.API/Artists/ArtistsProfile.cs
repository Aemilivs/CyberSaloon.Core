using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.DTO.Artists;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;

namespace CyberSaloon.Core.API.Artists
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile()
        {
            CreateMap<Artist, ArtistGetDTO>()
                .ForMember(
                    it => it.Applications,
                    it => it.MapFrom(that => that.Applications.Select(application => application.Id))
                );
            CreateMap<ArtistPostDTO, Artist>()
                .ForMember(
                    it => it.Applications,
                    it => it.MapFrom(that => new List<Application>())
                );
            CreateMap<ArtistPatchDTO, Artist>();
        }
    }
}