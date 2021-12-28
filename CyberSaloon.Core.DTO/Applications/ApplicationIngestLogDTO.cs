using System;

namespace CyberSaloon.Core.DTO.Applications
{
    public class ApplicationIngestLogDTO
    {
        public Guid Id { get; set; }
        public Guid Application { get; set; }
        public Guid Applicant { get; set; }
        public int WeightDelta { get; set; }
    }
}