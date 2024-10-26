using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.HouseDesign
{
    public class FeedbackHouseDesignDrawingRequest
    {
        [Required(ErrorMessage = "Phản hồi là bắt buộc")]
        public string Note { get; set; }
    }
}
