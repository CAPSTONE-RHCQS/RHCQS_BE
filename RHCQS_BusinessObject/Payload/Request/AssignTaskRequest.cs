using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class AssignTaskRequest
    {
        [Required(ErrorMessage = "Account is required")]
        public Guid AccountId { get; set; }
        [Required(ErrorMessage = "House design drawing is required")]
        public Guid HouseDesignDrawingId { get; set; }
    }
}
