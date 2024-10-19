using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_BusinessObject.Payload.Request.FinalQuotation
{
    public class FinalRequest
    {
        [Required(ErrorMessage = "VersionPresent là bắt buộc.")]
        [Range(1, double.MaxValue, ErrorMessage = "VersionPresent phải lớn hơn 0.")]
        public double VersionPresent { get; set; }

        [Required(ErrorMessage = "ProjectId là bắt buộc.")]
        public Guid ProjectId { get; set; }

        public Guid? PromotionId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải là một số dương.")]
        public double? TotalPrice { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public List<BatchPaymentInfoRequest> BatchPaymentInfos { get; set; }

        [Required(ErrorMessage = "Danh sách các thiết bị là bắt buộc.")]
        public List<EquipmentItemsRequest> EquipmentItems { get; set; }

        [Required(ErrorMessage = "Danh sách các mục báo giá là bắt buộc.")]
        public List<FinalQuotationItemRequest> FinalQuotationItems { get; set; }
    }

    public class BatchPaymentInfoRequest
    {
        [Required(ErrorMessage = "InitIntitialQuotationId là bắt buộc.")]
        public Guid InitIntitialQuotationId { get; set; }

        [Required(ErrorMessage = "PaymentTypeId là bắt buộc.")]
        public Guid PaymentTypeId { get; set; }
        public Guid ContractId { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        public double? Price { get; set; }

        public string? Percents { get; set; }

        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class EquipmentItemsRequest
    {
        [Required(ErrorMessage = "Tên thiết bị là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Đơn vị là bắt buộc.")]
        public string? Unit { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int? Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá đơn vị phải là một số dương.")]
        public double? UnitOfMaterial { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền vật liệu phải là một số dương.")]
        public double? TotalOfMaterial { get; set; }

        public string? Note { get; set; }
    }

    public class FinalQuotationItemRequest
    {
        [Required(ErrorMessage = "ConstructionItemId là bắt buộc.")]
        public Guid ConstructionItemId { get; set; }

        [Required(ErrorMessage = "Tên hạng mục là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Đơn vị là bắt buộc.")]
        public string? Unit { get; set; }

        public string? Weight { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá đơn vị lao động phải là một số dương.")]
        public double? UnitPriceLabor { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá đơn vị thô phải là một số dương.")]
        public double? UnitPriceRough { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá đơn vị hoàn thiện phải là một số dương.")]
        public double? UnitPriceFinished { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng giá lao động phải là một số dương.")]
        public double? TotalPriceLabor { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng giá thô phải là một số dương.")]
        public double? TotalPriceRough { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng giá hoàn thiện phải là một số dương.")]
        public double? TotalPriceFinished { get; set; }
    }
}
