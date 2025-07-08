using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2iDashApp.Model
{
    [Table("Systems")]
    public class SystemModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }

        public ICollection<Site> Sites { get; set; } = new List<Site>();
    }
}
