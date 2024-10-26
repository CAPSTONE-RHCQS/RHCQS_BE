using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.App
{
    public class FinalAppResponse
    {
        public FinalAppResponse(Guid id, double? version, string file, string status)
        {
            Id = id;
            Version = version;
            File = file;
            Status = status;
        }

        public Guid Id { get; set; }
        public double? Version { get; set; }
        public string File { get; set; }
        public string Status { get; set; }
    }
}
