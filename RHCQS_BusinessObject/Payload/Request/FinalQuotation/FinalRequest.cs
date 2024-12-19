using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RHCQS_BusinessObject.Payload.Request.FinalQuotation
{
    public class FinalRequest
    {
        [Required(ErrorMessage = "VersionPresent là bắt buộc.")]
        [Range(-1, double.MaxValue, ErrorMessage = "VersionPresent không phải số âm.")]
        public double VersionPresent { get; set; }
        [Required(ErrorMessage = "Chủ đầu tư là bắt buộc.")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mã dự án là bắt buộc.")]
        public Guid ProjectId { get; set; }

        public Guid? PromotionId { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải là một số dương.")]
        //public double? TotalPrice { get; set; }

        //public bool IsDraft { get; set; }
        public string? Note { get; set; }

        public List<BatchPaymentInfoRequest>? BatchPaymentInfos { get; set; }

        public List<EquipmentItemsRequest> EquipmentItems { get; set; }

        public List<UtilitiesUpdateRequestForFinal>? Utilities { get; set; }

        public List<FinalQuotationItemRequest>? FinalQuotationItems { get; set; }

    }

    public class BatchPaymentInfoRequest
    {
        //[Required(ErrorMessage = "InitInitialQuotationId là bắt buộc.")]
        //public Guid InitInitialQuotationId { get; set; }

        //[Required(ErrorMessage = "PaymentTypeId là bắt buộc.")]
        //public Guid PaymentTypeId { get; set; }

        //public Guid? ContractId { get; set; }

        //[Required(ErrorMessage = "Giá là bắt buộc.")]
        //[Range(1, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        //public double? Price { get; set; }

        //[Required(ErrorMessage = "Phần trăm là bắt buộc.")]
        //[Range(1, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        //public string? Percents { get; set; }

        //public string? Description { get; set; }

        //[Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        //public string? Status { get; set; }
        [Required(ErrorMessage = "NumberOfBatch là bắt buộc.")]
        public int NumberOfBatch { get; set; }
        [Required(ErrorMessage = "Price là bắt buộc.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Ngày thanh toán là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage = "Định dạng ngày không hợp lệ.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Giai đoạn thanh toán là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage = "Định dạng ngày không hợp lệ.")]
        public DateTime PaymentPhase { get; set; }
    }

    public class EquipmentItemsRequest
    {
        [Required(ErrorMessage = "Tên thiết bị là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Đơn vị là bắt buộc.")]
        public string? Unit { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải là mốt số dương.")]
        public int? Quantity { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Giá đơn vị phải là một số dương.")]
        public double? UnitOfMaterial { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "Tổng tiền vật liệu phải là một số dương.")]
        //public double? TotalOfMaterial { get; set; }
        public string? Note { get; set; }
        public string? Type { get; set; }
    }

    public class FinalQuotationItemRequest
    {
        [Required(ErrorMessage = "Mã hạng mục là bắt buộc.")]
        public Guid ConstructionId { get; set; }
        //public Guid? SubconstructionId { get; set; }
        public List<QuotationItemRequest>? QuotationItems { get; set; }
    }

    public class UtilitiesUpdateRequestForFinal
    {
        [Required(ErrorMessage = "UltilitiesItemId là bắt buộc.")]
        public Guid UtilitiesItemId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hệ số phải là một số dương.")]
        public double? Coefficient { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        public double? Price { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }

    }
    public class QuotationItemRequest
    {
        //public Guid? LaborId { get; set; }

        //public Guid? MaterialId { get; set; }

        public string? Unit { get; set; }
        [Required(ErrorMessage = "Mã công việc là bắt buộc.")]
        public Guid WorkTemplateId { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "Khối lượng phải là số dương.")]
        public double? Weight { get; set; }

        public double? UnitPriceLabor { get; set; }

        public double? UnitPriceRough { get; set; }

        public double? UnitPriceFinished { get; set; }

        public double? TotalPriceLabor { get; set; }

        public double? TotalPriceRough { get; set; }

        public double? TotalPriceFinished { get; set; }
        public string ? Note { get; set; }

/*        public QuotationLaborRequest? QuotationLabors { get; set; }

        public QuotationMaterialRequest? QuotationMaterials { get; set; }*/
    }

/*    public class QuotationLaborRequest
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

    }*/

}
