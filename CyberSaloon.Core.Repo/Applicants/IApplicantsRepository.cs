using System;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.Repo.Applicants
{
    public interface IApplicantsRepository : IRepository<Applicant>
    {
        Applicant? ReadByUserId(Guid userId);
        Applicant? ReadByAlias(string alias);
        bool Exists(string alias);
    }
}