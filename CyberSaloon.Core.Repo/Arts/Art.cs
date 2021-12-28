using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;

namespace CyberSaloon.Core.Repo.Arts
{
    public class Art
    {
        [Key]
        public Guid Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        public virtual Application Application { get; set; }
    }
}
