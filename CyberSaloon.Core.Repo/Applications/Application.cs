using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.Repo.Arts;

namespace CyberSaloon.Core.Repo.Applications
{
    public class Application
    {
        [Key]
        public Guid Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public bool Fullfilled => ArtId != default;
        
        public virtual Art Art { get; set; }
        public Guid? ArtId { get; set; }

        public virtual Artist Artist { get; set; }
        public Guid ArtistId { get; set; }

        public virtual Applicant Applicant { get; set; }
        public Guid ApplicantId { get; set; }
        
        public virtual ICollection<Applicant> Supporters { get; set; }
    }
}