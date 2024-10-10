using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.FinalQuotation
{
    public class FinalQuotationRequest
    {
    }
    public class AssignQuotaionFinal
    {
        [Required(ErrorMessage = "AccountId là bắt buộc.")]
        public Guid accountId { get; set; }
        [Required(ErrorMessage = "Báo gia chi tiết là bắt buộc.")]
        public Guid finalQuotationId { get; set; }
    }

}
