using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_BusinessObject.Payload.Request.InitialQuotation
{
    public class UpdateInitialRequest
    {
        [Required(ErrorMessage = "VersionPresent là bắt buộc.")]
        [Range(1, double.MaxValue, ErrorMessage = "VersionPresent phải lớn hơn 0.")]
        public double VersionPresent { get; set; }

        [Required(ErrorMessage = "AccountId là bắt buộc.")]
        public Guid? AccountId { get; set; }

        [Required(ErrorMessage = "ProjectId là bắt buộc.")]
        public Guid ProjectId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Diện tích phải là một số dương.")]
        public double? Area { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Thời gian thi công phải là một số dương.")]
        public int? TimeProcessing { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Thời gian thi công thô phải là một số dương.")]
        public int? TimeRough { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Thời gian khác phải là một số dương.")]
        public int? TimeOthers { get; set; }

        public string? OthersAgreement { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền thô phải là một số dương.")]
        public double? TotalRough { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền tiện ích phải là một số dương.")]
        public double? TotalUtilities { get; set; }

        public List<InitialQuotaionItemUpdateRequest>? Items { get; set; }
        public List<PackageQuotationUpdateRequest>? Packages { get; set; }

        public List<UtilitiesUpdateRequest>? Utilities { get; set; }
        public PromotionUpdateRequest? Promotions { get; set; }
        public List<BatchPaymentUpdateRequest> BatchPayments { get; set; }
    }

    public class InitialQuotaionItemUpdateRequest
    {
        [Required(ErrorMessage = "Tên hạng mục là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc.")]
        public Guid ConstructionItemId { get; set; }

        public Guid? SubConstructionId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Diện tích phải là một số dương.")]
        public double? Area { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        public double? Price { get; set; }
    }

    public class PackageQuotationUpdateRequest
    {
        [Required(ErrorMessage = "PackageId là bắt buộc.")]
        public Guid PackageId { get; set; }

        [Required(ErrorMessage = "Loại gói là bắt buộc.")]
        public string Type { get; set; }
    }

    public class UtilitiesUpdateRequest
    {
        [Required(ErrorMessage = "UltilitiesItemId là bắt buộc.")]
        public Guid UtilitiesItemId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hệ số phải là một số dương.")]
        public double? Coefiicient { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        public double? Price { get; set; }

        public string? Description { get; set; }
    }

    public class PromotionUpdateRequest
    {
        //[Required(ErrorMessage = "Mã khuyến mãi là bắt buộc.")]
        public Guid Id { get; set; }
    }

    public class BatchPaymentUpdateRequest
    {

        public double? Price { get; set; }

        public string? Percents { get; set; }

        public string? Description { get; set; }
    }
}
