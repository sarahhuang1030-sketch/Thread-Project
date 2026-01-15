using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop04.Data.Models
{
    public class CustomerPreferences
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferenceKey { get; set; } // "PreferredClimate"

        [Required]
        [StringLength(500)]
        public string PreferenceValue { get; set; } // "Tropical"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
