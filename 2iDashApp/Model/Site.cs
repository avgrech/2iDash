using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2iDashApp.Model
{
    public enum SiteEnvironment
    {
        Development,
        Staging,
        Production
    }

    public class Site
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;

        [Required]
        [Url]
        public string HealthUrl { get; set; } = string.Empty;

        [Required]
        public SiteEnvironment Environment { get; set; }

        public int SystemModelId { get; set; }
        public SystemModel? SystemModel { get; set; }
    }
}
