using System;

namespace CyberSaloon.Core.DTO.Applications
{
    public class ApplicationPostDTO
    {
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}