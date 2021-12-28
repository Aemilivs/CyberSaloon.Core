using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.DTO.Applicants;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;

namespace CyberSaloon.Core.API.Applicants
{
    public class ApplicantsProfile : Profile
    {
        public ApplicantsProfile()
        {
            CreateMap<Applicant, ApplicantGetDTO>()
                .ForMember(
                    it => it.Applications,
                    it => it.MapFrom(that => that.Applications.Select(application => application.Id))
                )
                .ForMember(
                    it => it.Supported,
                    it => it.MapFrom(that => that.Supported.Select(application => application.Id))
                );

            CreateMap<ApplicantPostDTO, Applicant>()
                .ForMember(
                    it => it.Supported,
                    it => it.MapFrom(that => new List<Application>())
                );
            CreateMap<ApplicantPatchDTO, Applicant>();
        }
    }
}