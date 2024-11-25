using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Utility
{
    public class AutoUtilityResponse
    {
        public AutoUtilityResponse(
            Guid utilitySectionId,
            Guid? utilityItemId,
            string name,
            double? coefficient,
            double? unitPrice)
        {
            UtilitySectionId = utilitySectionId;
            UtilityItemId = utilityItemId; 
            Name = name;
            Coefficient = coefficient;
            UnitPrice = unitPrice;
        }
        public Guid UtilitySectionId { get; set; }
        public Guid? UtilityItemId { get; set; }
        public string Name { get; set; }
        public double? Coefficient { get; set; }
        public double? UnitPrice { get; set; }
    }
}
