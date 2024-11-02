using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.DesignTemplate
{
    public class PackageHouseRequest
    {
        public Guid PackageId { get; set; }
        public Guid DesginTemplateId { get; set; }
        public string? Description { get; set; }
        public string PackageHouseImage { get; set; }
    }
}
