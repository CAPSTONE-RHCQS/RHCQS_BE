using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Project
{
    public class ProjectDesignStaffResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }

        public string AccountName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Mail { get; set; }
        public double? Area { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? ProjectCode { get; set; }

        public string StaffName { get; set; }
        public string StaffPhone { get; set; }
        public string StaffAvatar { get; set; }
        public bool? IsDrawing { get; set; }

        public List<InitialInfo>? InitialInfo { get; set; }
        public List<HouseDesignDrawingInfo>? HouseDesignDrawingInfo { get; set; }
    }
}
