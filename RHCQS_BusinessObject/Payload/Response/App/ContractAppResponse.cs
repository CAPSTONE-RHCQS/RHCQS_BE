using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.App
{
    public class ContractAppResponse
    {
        public ContractAppResponse(Guid id, string file)
        {
            Id = id;
            File = file;
        }

        public Guid Id { get; set; }
        public string File { get; set; }
    }
}
