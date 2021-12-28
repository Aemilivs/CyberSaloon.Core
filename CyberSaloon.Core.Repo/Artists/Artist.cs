using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Arts;

namespace CyberSaloon.Core.Repo.Artists
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set;}
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public virtual ICollection<Application> Applications { get; set; }
    }
}