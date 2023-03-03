using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models.ViewModels
{
    public class UpdateHouse
    {
        public HouseDto House { get; set; }


        public IEnumerable<OwnerDto> Owner { get; set; }
    }
}