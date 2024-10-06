using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class PackageRequest
    {

        public Guid PackageTypeId { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public List<PackageDetailsRequest> PackageDetails { get; set; }

        public List<PackageHousesRequest> PackageHouses { get; set; }
    }
    public class PackageDetailsRequest
    {

        public string? Action { get; set; }

        public string? Type { get; set; }
        public List<PackageLaborRequest> PackageLabors { get; set; }

        public List<PackageMaterialRequest> PackageMaterials { get; set; }
    }
    public class PackageLaborRequest
    {

        public Guid LaborId { get; set; }

        public double? TotalPrice { get; set; }

        public int? Quantity { get; set; }
    }
    public class PackageMaterialRequest
    {
        public Guid MaterialSectionId { get; set; }

    }
    public class PackageHousesRequest
    {
        public Guid DesignTemplateId { get; set; }

        public string? ImgUrl { get; set; }

    }
}
