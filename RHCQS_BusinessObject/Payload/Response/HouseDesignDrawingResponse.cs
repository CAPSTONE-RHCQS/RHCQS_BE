using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class HouseDesignDrawingResponse
    {
        public HouseDesignDrawingResponse() { }
        public HouseDesignDrawingResponse(Guid id, Guid projectId, string? name, int? step, 
            string? status, string? type, bool? isCompany, DateTime? insDate, 
            List<HouseDesignVersionResponse> versions)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Step = step;
            Status = status;
            Type = type;
            IsCompany = isCompany;
            InsDate = insDate;
            Versions = versions;
        }

        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string? Name { get; set; }

        public int? Step { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public bool? IsCompany { get; set; }

        public DateTime? InsDate { get; set; }
        public List<HouseDesignVersionResponse> Versions { get; set; }
    }

    public class HouseDesignVersionResponse
    {
        public HouseDesignVersionResponse() { }
        public HouseDesignVersionResponse(Guid id, string? name, double? version, string? status, 
            DateTime? insDate, string? upVersion, string? note)
        {
            Id = id;
            Name = name;
            Version = version;
            Status = status;
            InsDate = insDate;
            UpVersion = upVersion;
            Note = note;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Version { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }
        public string? UpVersion { get; set; }

        public string? Note { get; set; }
    }
}
