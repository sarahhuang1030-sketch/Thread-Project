using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop04.Data.Data.DTO
{
    public class AgencyDTO
    {
        public int AgencyId { get; set; }
        public string AgncyAddress { get; set; }
        public string AgncyCity { get; set; }
        public string AgncyProv { get; set; }
        public string AgncyPostal { get; set; }
        public string AgncyCountry { get; set; }
        public string AgncyPhone { get; set; }
        public string AgncyFax { get; set; }
    }
}
