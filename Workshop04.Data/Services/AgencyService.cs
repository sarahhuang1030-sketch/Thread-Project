using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop04.Data.Data;
using Workshop04.Data.Data.DTO;

namespace Workshop04.Data.Services
{
    public class AgencyService
    {
        //Get all AgencyDTOs from the database
        public static List<AgencyDTO> GetAllAgencyDTOs(TravelExpertsContext context)
        {
            List<AgencyDTO> agencies = context.Agencies.
                Select(a => new AgencyDTO
                {
                    AgencyId = a.AgencyId,
                    AgncyAddress = a.AgncyAddress,
                    AgncyCity = a.AgncyCity,
                    AgncyProv = a.AgncyProv,
                    AgncyPostal = a.AgncyPostal,
                    AgncyCountry = a.AgncyCountry,
                    AgncyPhone = a.AgncyPhone,
                    AgncyFax = a.AgncyFax
                }).
                ToList();
            return agencies;
        }
    }
}
