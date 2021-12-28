using System;
using CyberSaloon.Core.BLL.Common;
using CyberSaloon.Core.Repo.Applicants;

namespace CyberSaloon.Core.BLL.Applicants
{
    public interface IApplicantsService : IService<Applicant>
    {
        Applicant? ReadByUserId(Guid userId);
        Applicant? ReadByAlias(string alias);
    }
}