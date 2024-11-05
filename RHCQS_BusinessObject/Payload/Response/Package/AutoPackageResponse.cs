using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Package
{
    public class AutoPackageResponse
    {
        public AutoPackageResponse(Guid packageId,
            string packageName,
            string type,
            double price)
        {
            PackageId = packageId;
            PackageName = packageName;
            Type = type;
            Price = price;
        }
        public Guid PackageId { get; set; }
        public string PackageName { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
    }
}
