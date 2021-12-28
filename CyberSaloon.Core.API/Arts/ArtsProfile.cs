using System;
using AutoMapper;
using CyberSaloon.Core.DTO.Arts;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.Repo.Arts;

namespace CyberSaloon.Core.API.Arts
{
    public class ArtsProfile : Profile
    {
        private readonly IArtistsRepository _artistsRepository;
        private readonly IApplicationsRepository _applicationsRepository;
        public ArtsProfile(
                IArtistsRepository artistsRepository,
                IApplicationsRepository applicationsRepository
            )
        {
            _artistsRepository =
                artistsRepository ??
                throw new System.ArgumentNullException(nameof(artistsRepository));

            _applicationsRepository =
                applicationsRepository ??
                throw new System.ArgumentNullException(nameof(applicationsRepository));

            CreateMap<Art, ArtGetDTO>()
                .ForMember(
                        it => it.Application,
                        it => it.MapFrom(that => that.Application.Id)
                    )
                .ForMember(
                        it => it.Author,
                        it => it.MapFrom(that => that.Application.Artist.Alias)
                    );

            CreateMap<ArtPostDTO, Art>()
                .ForMember(
                        it => it.Id,
                        it => it.MapFrom(that => Guid.NewGuid())
                    )
                .ForMember(
                        it => it.Application,
                        it => it.MapFrom(that => _applicationsRepository.Read(that.Application))
                );

            CreateMap<ArtPatchDTO, Art>()
                .ForMember(
                        it => it.Application,
                        it => it.MapFrom(that => _applicationsRepository.Read(that.Application))
                );
        }
    }
}