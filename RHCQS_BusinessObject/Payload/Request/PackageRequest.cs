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
        [Required(ErrorMessage = "Loại gói là bắt buộc phải có.")]
        public string PackageType { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public List<PackageLaborRequest> PackageLabors { get; set; }

        public List<PackageMaterialRequest> PackageMaterials { get; set; }

        //public List<PackageHousesRequest>? PackageHouses { get; set; }

        //public List<WorkTemplateRequest>? WorkTemplate { get; set; }
    }
    //public class WorkTemplateRequest
    //{
    //    public Guid ConstructionWorKid { get; set; }
    //    public double LaborCost { get; set; }
    //    public double MaterialCost { get; set; }
    //    public double MaterialFinishedCost { get; set; }

    //}
    public class PackageLaborRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid LaborId { get; set; }

    }
    public class PackageMaterialRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid MaterialId { get; set; }


    }
    //public class PackageHousesRequest
    //{
    //    [Required(ErrorMessage = "Id là bắt buộc phải có.")]
    //    public Guid DesignTemplateId { get; set; }

    //    public string? ImgUrl { get; set; }

    //    public string? Description { get; set; }

    //}
}
