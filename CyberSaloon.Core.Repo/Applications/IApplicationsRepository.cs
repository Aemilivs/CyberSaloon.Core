using System;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.Repo.Applications
{
    public interface IApplicationsRepository : IRepository<Application>
    {
        void Apply(Application application, Applicant existingApplicant);
        void Defy(Application application, Applicant existingApplicant);
    }
}