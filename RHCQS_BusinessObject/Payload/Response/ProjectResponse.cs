using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class ProjectResponse
    {
        public ProjectResponse() { }
        public ProjectResponse(Guid id, string accountName, string? name, string? type, string? status,
            DateTime? insDate, DateTime? upsDate, string? projectCode)
        {
            Id = id;
            AccountName = accountName;
            Name = name;
            Type = type;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            ProjectCode = projectCode;
        }
        public Guid Id { get; set; }

        public string AccountName { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? ProjectCode { get; set; }
    }

    public class ProjectDetail
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string AccountName { get; set; }
        public string? Address { get; set; }
        public double? Area { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? ProjectCode { get; set; }

        public List<InitialInfo>? InitialInfo { get; set; }
        public List<HouseDesignDrawingInfo>? HouseDesignDrawingInfo { get; set; }
        public List<DetailedInfo>? DetailedInfo { get; set; }
    }

    public class InitialInfo
    {
        public Guid Id { get; set; }
        public string? AccountName { get; set; }
        public string? Version { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
    }

    public class HouseDesignDrawingInfo
    {
        public Guid Id { get; set; }
        public string? AccountName { get; set; }
        public string? Version { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
    }

    public class DetailedInfo
    {
        public Guid Id { get; set; }
        public string? AccountName { get; set; }
        public string? Version { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
    }
}
