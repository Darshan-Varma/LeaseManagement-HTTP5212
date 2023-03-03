using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string HouseDescription { get; set; }

        //A house belongs to one owner
        //An owner can have many houses
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        //A house can be leased by many tenants
        public ICollection<Tenant> Tenants { get; set; }
    }

    public class HouseDto
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string HouseDescription { get; set; }

        public string OwnerName { get; set; }


    }
}