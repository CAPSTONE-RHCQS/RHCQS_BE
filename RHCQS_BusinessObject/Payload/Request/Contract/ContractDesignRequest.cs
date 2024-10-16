using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.Contract
{
    public class ContractDesignRequest
    {
        [Required(ErrorMessage = "Dự án là bắt buộc.")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Loại hợp đồng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Độ dài của loại hợp đồng không được vượt quá 100 ký tự.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc.")]
        public DateTime? EndDate { get; set; }

        [Range(1, 60, ErrorMessage = "Thời hạn hợp đồng phải từ 1 đến 60 tháng.")]
        public int? ValidityPeriod { get; set; }

        //[StringLength(15, ErrorMessage = "Mã số thuế không được vượt quá 15 ký tự.")]
        public string? TaxCode { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị hợp đồng phải là số dương.")]
        public double? ContractValue { get; set; }

        public string? UrlFile { get; set; }

        public string? Note { get; set; }
        public List<BatchPaymentRequest>? BatchPaymentRequests { get; set; }
    }

    public class BatchPaymentRequest
    {
        [Required(ErrorMessage = "Số đợt thanh toán là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số đợt thanh toán phải lớn hơn hoặc bằng 1.")]
        public int NumberOfBatches { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Ngày thanh toán là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage = "Định dạng ngày không hợp lệ.")]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Giai đoạn thanh toán là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage = "Định dạng ngày không hợp lệ.")]
        public DateTime? PaymentPhase { get; set; }
        [Required(ErrorMessage = "Phần trăm số tiền thanh toán là bắt buộc.")]
        public string? Percents { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        public string? Description { get; set; }
    }
}
