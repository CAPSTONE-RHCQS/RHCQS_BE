﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class LaborResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }

        public string? Type { get; set; }
        public string? Code { get; set; }
    }
}
