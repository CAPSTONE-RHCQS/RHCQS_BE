using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Utility
{
    public class UtilityResponse
    {
        public UtilityResponse(Guid id, string? name, string? type, bool? deflag,
            DateTime? insDate, DateTime? upsDate, List<UtilitiesSectionResponse> sections)
        {
            Id = id;
            Name = name;
            Type = type;
            Deflag = deflag;
            InsDate = insDate;
            UpsDate = upsDate;
            Sections = sections;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public bool? Deflag { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public List<UtilitiesSectionResponse> Sections { get; set; }

    }

    public class UtilitiesSectionResponse
    {
        public UtilitiesSectionResponse(Guid id, string? name, bool? deflag, DateTime? insDate,
            DateTime? upsDate, string? description, double? unitPrice, string? unit)
        {
            Id = id;
            Name = name;
            Deflag = deflag;
            InsDate = insDate;
            UpsDate = upsDate;
            Description = description;
            UnitPrice = unitPrice;
            Unit = unit;
            Items = new List<UtilityItemResponse>();
        }

        public UtilitiesSectionResponse(Guid id, string? name, bool? deflag, DateTime? insDate,
            DateTime? upsDate, string? description, double? unitPrice, string? unit, List<UtilityItemResponse> items)
        {
            Id = id;
            Name = name;
            Deflag = deflag;
            InsDate = insDate;
            UpsDate = upsDate;
            Description = description;
            UnitPrice = unitPrice;
            Unit = unit;
            Items = items;
        }
        public Guid Id { get; set; }


        public string? Name { get; set; }

        public bool? Deflag { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Description { get; set; }
        public double? UnitPrice { get; set; }

        public string? Unit { get; set; }

        public List<UtilityItemResponse> Items { get; set; }

    }

    public class UtilityItemResponse
    {
        public UtilityItemResponse() { }
        public UtilityItemResponse(Guid id, string? name, double? coefficient, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            Name = name;
            Coefficient = coefficient;
            InsDate = insDate;
            UpsDate = upsDate;
        }
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public string? Name { get; set; }

        public double? Coefficient { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
    }
}
