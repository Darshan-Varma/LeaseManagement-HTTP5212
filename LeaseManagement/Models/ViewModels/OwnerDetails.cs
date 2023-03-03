using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaseManagement.Models.ViewModels
{
    public class OwnerDetails
    {
        public OwnerDto Owner { get; set; }
        public IEnumerable<HouseDto> RelatedHouses { get; set; }
    }
}