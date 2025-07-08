using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _2iDashApp.Model
{
    public class Organisation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<SystemModel> Systems { get; set; } = new List<SystemModel>();
    }
}
