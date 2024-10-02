using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    using System.ComponentModel.DataAnnotations;

    public class HouseDesignVersionRequest
    {
        [Required(ErrorMessage = "Account is required")]
        public Guid AccountId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "HouseDesignDrawingId is required")]
        public Guid? HouseDesignDrawingId { get; set; }

        [Url(ErrorMessage = "FileUrl must be a valid URL")]
        public string? FileUrl { get; set; }

        public Guid? RelatedDrawingId { get; set; }

        public Guid? PreviousDrawingId { get; set; }
    }

    public class HouseDesignVersionUpdateRequest
    {

        [Url(ErrorMessage = "FileUrl must be a valid URL")]
        public string? FileUrl { get; set; }
    }

    public class HouseDesginVersionAvailableRequest
    {
        [Required(ErrorMessage = "Id is required!")]
        public Guid HouseDesignVersionId { get; set; }

       
    }
}
