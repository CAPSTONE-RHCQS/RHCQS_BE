using RHCQS_DataAccessObjects.Models;
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

        [Required(ErrorMessage = "Danh sách các đợt thanh toán là bắt buộc.")]
        public List<BatchPaymentInfoRequest> BatchPaymentInfos { get; set; }

        [Required(ErrorMessage = "Danh sách các thiết bị là bắt buộc.")]
        public List<EquipmentItemsRequest> EquipmentItems { get; set; }

        [Required(ErrorMessage = "Danh sách các mục báo giá là bắt buộc.")]
        public List<FinalQuotationItemRequest> FinalQuotationItems { get; set; }

        public Guid? QuotationUtilitiesId { get; set; }

    }

    public class BatchPaymentInfoRequest
    {
        [Required(ErrorMessage = "InitIntitialQuotationId là bắt buộc.")]
        public Guid InitIntitialQuotationId { get; set; }

        [Required(ErrorMessage = "PaymentTypeId là bắt buộc.")]
        public Guid PaymentTypeId { get; set; }

        [Required(ErrorMessage = "ContractId là bắt buộc.")]
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

        public List<QuotationItemRequest> QuotationItems { get; set; }
    }

    public class QuotationItemRequest
    {

        [Required(ErrorMessage = "Đơn vị là bắt buộc.")]
        public string? Unit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Khối lượng phải là số dương.")]
        public double? Weight { get; set; }

        public double? UnitPriceLabor { get; set; }

        public double? UnitPriceRough { get; set; }

        public double? UnitPriceFinished { get; set; }

        public double? TotalPriceLabor { get; set; }

        public double? TotalPriceRough { get; set; }

        public double? TotalPriceFinished { get; set; }
        public string ? Note { get; set; }

        public List<QuotationLaborRequest> QuotationLabors { get; set; }

        public List<QuotationMaterialRequest> QuotationMaterials { get; set; }
    }

    public class QuotationLaborRequest
    {
        [Required(ErrorMessage = "LaborId là bắt buộc.")]
        public Guid LaborId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá lao động phải là một số dương.")]
        public double? LaborPrice { get; set; }
    }

    public class QuotationMaterialRequest
    {
        [Required(ErrorMessage = "MaterialId là bắt buộc.")]
        public Guid MaterialId { get; set; }

        [Required(ErrorMessage = "Đơn vị là bắt buộc.")]
        public string? Unit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá vật liệu phải là một số dương.")]
        public double? MaterialPrice { get; set; }

    }

}
