using CyberSaloon.Core.BLL.Common;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;

namespace CyberSaloon.Core.BLL.Applications
{
    public interface IApplicationsService : IService<Application>
    {
        void Apply(Application application, Applicant existingApplicant);
        void Defy(Application application, Applicant existingApplicant);
    }
}