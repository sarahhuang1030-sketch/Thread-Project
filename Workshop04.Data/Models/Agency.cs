using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop04.Data.Models
{
    public class Agency
    {
        [Key]
        public int AgencyId { get; set; }

        [StringLength(20)]
        public string AgncyAddress { get; set; }

        [StringLength(20)]
        public string AgncyCity { get; set; }

        [StringLength(20)]
        public string AgncyProv { get; set; }

        [StringLength(20)]
        public string AgncyPostal { get; set; }

        [StringLength(20)]
        public string AgncyCountry { get; set; }

        [StringLength(20)]
        public string AgncyPhone { get; set; }

        [StringLength(20)]
        public string AgncyFax { get; set; }
    }
}
