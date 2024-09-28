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
        public Guid Id { get; set; }


        public string? Name { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Description { get; set; }

    }
}
