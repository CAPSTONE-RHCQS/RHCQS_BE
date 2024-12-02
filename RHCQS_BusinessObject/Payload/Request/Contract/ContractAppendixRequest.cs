using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.Contract
{
    public class ContractAppendixRequest
    {
        [Required(ErrorMessage = "Hợp đồng là bắt buộc.")]
        public Guid ContractId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc.")]
        public DateTime? EndDate { get; set; }

        [Range(1, 100000, ErrorMessage = "Thời hạn hợp đồng phải từ 1 đến 100000 tháng.")]
        public int? ValidityPeriod { get; set; }

        public string? TaxCode { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị hợp đồng phải là số dương.")]
        public double? ContractValue { get; set; }

        public string? UrlFile { get; set; }

        public string? Note { get; set; }
        public List<CancelBatchPayment> CancelBatchPaymnetContract { get; set; }
        public List<BatchPaymentRequest>? BatchPaymentRequests { get; set; }
    }

    public class CancelBatchPayment
    {
        public Guid BatchPaymentId { get; set; }
    }
}

