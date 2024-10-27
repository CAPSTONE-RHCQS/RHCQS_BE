﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class AssignTaskResponse
    {
        public AssignTaskResponse(Guid id, Guid accountId, string accountName,
            string? name, string? status, DateTime? insDate)
        {
            Id = id;
            AccountId = accountId;
            AccountName = accountName;
            Name = name;
            Status = status;
            InsDate = insDate;
        }
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        public string AccountName { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

    }

    public class DesignStaffWorkResponse
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }

        public string? ProjectName { get; set; }
    }
}
