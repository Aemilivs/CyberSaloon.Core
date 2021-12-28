using System;

namespace CyberSaloon.Core.DTO.Applications
{
    public class ApplicationPatchDTO
    {
        public Guid Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}