using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class ConstructionItemResponse
    {
        public ConstructionItemResponse() { }
        public ConstructionItemResponse(Guid id, string? name, double? coefficient,
            string? unit, DateTime? insDate, DateTime? upsDate, List<SubConstructionItemResponse> subConstructionItems)
        {
            Id = id;
            Name = name;
            Coefficient = coefficient;
            Unit = unit;
            InsDate = insDate;
            UpsDate = upsDate;
            SubConstructionItems = subConstructionItems;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Coefficient { get; set; }

        public string? Unit { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        public List<SubConstructionItemResponse> SubConstructionItems { get; set; }
    }

    public class SubConstructionItemResponse
    {
        public SubConstructionItemResponse(Guid id, string? name, double? coefficient, string? unit, DateTime? insDate)
        {
            Id = id;
            Name = name;
            Coefficient = coefficient;
            Unit = unit;
            InsDate = insDate;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Coefficient { get; set; }

        public string? Unit { get; set; }

        public DateTime? InsDate { get; set; }
    }
}
