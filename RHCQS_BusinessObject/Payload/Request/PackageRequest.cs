using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class PackageRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
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

    }
    public class PackageMaterialRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid MaterialSectionId { get; set; }

    }
    public class PackageHousesRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid DesignTemplateId { get; set; }

        public string? ImgUrl { get; set; }

    }
}
