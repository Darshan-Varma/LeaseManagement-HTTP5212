using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models.ViewModels
{
    public class HouseDetails
    {
        public HouseDto House { get; set; }
        public IEnumerable<TenantDto> Tenant { get; set; }
        public IEnumerable <TenantDto> OtherTenants { get; set; }
    }
}