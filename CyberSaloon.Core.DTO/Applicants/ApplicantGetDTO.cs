using System;
using System.Collections.Generic;

namespace CyberSaloon.Core.DTO.Applicants
{
    public class ApplicantGetDTO
    {
        public Guid Id { get; set; }
        public string Alias { get; set; } = string.Empty;

        public ICollection<Guid> Supported { get; set; } = new List<Guid>();
        public ICollection<Guid> Applications { get; set; } = new List<Guid>();
    }
}