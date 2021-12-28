using System;

namespace CyberSaloon.Core.DTO.Artists
{
    public class ArtistPatchDTO
    {
        public Guid Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}