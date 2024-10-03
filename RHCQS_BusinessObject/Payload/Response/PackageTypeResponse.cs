using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class PackageTypeResponse
    {
        public PackageTypeResponse(Guid id, string? name, DateTime? insdate)
        {
            Id = id;
            Name = name;
            InsDate = insdate;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime? InsDate { get; set; }
    }
}
