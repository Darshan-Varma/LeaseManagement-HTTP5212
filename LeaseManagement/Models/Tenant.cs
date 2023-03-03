using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantDescription { get; set; }
        //A tenant/broker can lease multiple house at a time
        public ICollection<House> House { get; set; }
    }

    public class TenantDto
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantDescription { get; set; }

    }
}