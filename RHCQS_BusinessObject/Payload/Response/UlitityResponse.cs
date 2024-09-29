using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class UtilityResponse
    {
        public UtilityResponse() { }
        public UtilityResponse(Guid id, string? name, string? type, string? status, 
            DateTime? insDate, DateTime? upsDate, List<UtilitiesSectionResponse> sections)
        { 
            Id = id;
            Name = name;
            Type = type;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            Sections = sections;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public List<UtilitiesSectionResponse> Sections { get; set; }

    }

    public class UtilitiesSectionResponse
    {
        public UtilitiesSectionResponse() { }
        public UtilitiesSectionResponse(Guid id, string? name, string? status, DateTime? insDate,
            DateTime? upsDate, string? description)
        {
            Id = id;
            Name = name;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            Description = description;
        }

        public UtilitiesSectionResponse(Guid id, string? name, string? status, DateTime? insDate,
            DateTime? upsDate, string? description, List<UtilityItemResponse> items)
        {
            Id = id;
            Name = name;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            Description = description;
            Items = items;
        }
        public Guid Id { get; set; }


        public string? Name { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Description { get; set; }

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
