using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models.ViewModels
{
    public class TenantDetails
    {
        public TenantDto Tenant { get; set; }
        public IEnumerable<HouseDto> LeasedHouses { get; set; }
    }
}