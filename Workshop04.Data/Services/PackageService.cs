using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop04.Data.Data;
using Workshop04.Data.Data.DTO;

namespace Workshop04.Data.Services
{
    public class PackageService
    {
        //get all packageDTOs
        public static List<PackageDTO> GetAllPackageDTOs(TravelExpertsContext context)
        {
            List<PackageDTO> packages = context.Packages.
                Select(p => new PackageDTO
                {
                    PackageId = p.PackageId,
                    PkgName = p.PkgName,
                    PkgStartDate = p.PkgStartDate,
                    PkgEndDate = p.PkgEndDate,
                    PkgDesc = p.PkgDesc,
                    PkgBasePrice = p.PkgBasePrice,
                    PkgAgencyCommission = p.PkgAgencyCommission
                }).
                ToList();
            return packages;
        }

        //get the first 3 package
        public static List<PackageDTO> GetTop3PackageDTOs(TravelExpertsContext context)
        {
            List<PackageDTO> packages = context.Packages.
                Take(3).
                Select (p => new PackageDTO
                {
                    PackageId = p.PackageId,
                    PkgName = p.PkgName,
                    PkgStartDate = p.PkgStartDate,
                    PkgEndDate = p.PkgEndDate,
                    PkgDesc = p.PkgDesc,
                    PkgBasePrice = p.PkgBasePrice,
                    PkgAgencyCommission = p.PkgAgencyCommission
                }).
                ToList();
            return packages;
        }

        //get the first 3 package
        public static List<PackageDTO> GetTop3ValidPackageDTOs(TravelExpertsContext context)
        {
            List<PackageDTO> packages = context.Packages
                .Where(p => p.PkgEndDate == null || p.PkgEndDate > DateTime.Now)
                .Take(3)
                .Select(p => new PackageDTO
                {
                    PackageId = p.PackageId,
                    PkgName = p.PkgName,
                    PkgStartDate = p.PkgStartDate,
                    PkgEndDate = p.PkgEndDate,
                    PkgDesc = p.PkgDesc,
                    PkgBasePrice = p.PkgBasePrice,
                    PkgAgencyCommission = p.PkgAgencyCommission
                })
                .ToList();
            return packages;
        }
    }
}
