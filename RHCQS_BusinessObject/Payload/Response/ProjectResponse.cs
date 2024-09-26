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
}
