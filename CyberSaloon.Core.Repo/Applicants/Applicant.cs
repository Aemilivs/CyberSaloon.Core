using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CyberSaloon.Core.Repo.Applications;

namespace CyberSaloon.Core.Repo.Applicants
{
    public class Applicant
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Alias { get; set; } = string.Empty;

        public virtual ICollection<Application> Supported { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}