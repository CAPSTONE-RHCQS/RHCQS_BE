﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Construction
{
    public class ConstructionWorkItemResponse
    {
        public Guid Id { get; set; }

        public string? WorkName { get; set; }

        public Guid? ConstructionId { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Unit { get; set; }

        public string? Code { get; set; }
        public List<ConstructionWorkResourceItem> Resources { get; set; }
    }

    public class ConstructionWorkResourceItem
    {
        public Guid Id { get; set; }

        public Guid? MaterialSectionId { get; set; }

        public double? MaterialSectionNorm { get; set; }

        public Guid? LaborId { get; set; }

        public double? LaborNorm { get; set; }

        public DateTime? InsDate { get; set; }
    }

}
