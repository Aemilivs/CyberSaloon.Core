using System;
using System.Collections.Generic;

namespace CyberSaloon.Core.DTO.Applications
{
    public class ApplicationGetDTO
    {
        public Guid Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Fullfilled { get; set; } = false;
        public string Artist { get; set; }
        public string Author { get; set; }
        public Guid Art { get; set; }
        public IList<Guid> Applicants { get; set; } = new List<Guid>();
    }
}