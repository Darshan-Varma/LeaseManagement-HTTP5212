using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerDescription { get; set; }

    }

    public class OwnerDto
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerDescription { get; set; }

    }
}