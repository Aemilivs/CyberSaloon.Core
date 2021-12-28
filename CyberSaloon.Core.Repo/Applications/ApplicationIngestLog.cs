using System;
using CyberSaloon.Core.Repo.Applicants;

namespace CyberSaloon.Core.Repo.Applications
{
    public class ApplicationIngestLog
    {
        public Guid Id { get; set; }
        public virtual Application Application { get; set; } = new Application();
        public virtual Applicant Applicant { get; set; } = new Applicant();
        public int WeightDelta { get; set; }
    }
}