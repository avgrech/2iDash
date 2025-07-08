using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2iDashApp.Model
{
    public class Site
    {
        public int Id { get; set; }

        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;

        [Required]
        [Url]
        public string HealthUrl { get; set; } = string.Empty;

        [Required]
        public string Environment { get; set; } = string.Empty; // e.g., Staging, Production

        public int SystemModelId { get; set; }
        public SystemModel? SystemModel { get; set; }
    }
}
