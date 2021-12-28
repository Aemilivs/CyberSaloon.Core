using System;

namespace CyberSaloon.Core.DTO.Applicants
{
    public class ApplicantPatchDTO
    {
        public Guid Id { get; set; }
        public string Alias { get; set; } = string.Empty;
    }
}