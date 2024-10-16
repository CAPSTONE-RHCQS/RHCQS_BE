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
    }
}
