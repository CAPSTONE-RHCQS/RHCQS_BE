using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class PromotionResponse
    {
        public PromotionResponse(Guid id, string? name, string? code, int? value, DateTime? insDate, DateTime? startTime,
             DateTime? expTime, bool? isRunning)
        {
            Id = id;
            Name = name;
            Code = code;
            Value = value;
            InsDate = insDate;
            StartTime = startTime;
            ExpTime = expTime;
            IsRunning = isRunning;
        }
        public Guid Id { get; set; }

        public string? Code { get; set; }

        public int? Value { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Name { get; set; }

        public DateTime? ExpTime { get; set; }

        public bool? IsRunning { get; set; }
    }
}
