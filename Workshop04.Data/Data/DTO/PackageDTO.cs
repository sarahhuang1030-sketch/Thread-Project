using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop04.Data.Data.DTO
{
    public class PackageDTO
    {
        public int PackageId { get; set; }
        public string PkgName { get; set; } = null!;
        public DateTime? PkgStartDate { get; set; }
        public DateTime? PkgEndDate { get; set; }
        public string? PkgDesc { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PkgBasePrice { get; set; }
        public decimal? PkgAgencyCommission { get; set; }
    }
}
