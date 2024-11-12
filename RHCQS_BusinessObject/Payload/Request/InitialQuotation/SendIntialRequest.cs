using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.InitialQuotation
{
    public class SendInitialRequest
    {
        [Required(ErrorMessage = "VersionPresent là bắt buộc.")]
        [Range(-1, double.MaxValue, ErrorMessage = "VersionPresent không phải số âm.")]
        public double VersionPresent { get; set; }

        [Required(ErrorMessage = "ProjectId là bắt buộc.")]
        public Guid ProjectId { get; set; }
    }
}
