using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class FinalQuotationListResponse
    {
        public FinalQuotationListResponse(Guid id, string customerName, double? version, string? status)
        {
            Id = id;
            CustomerName = customerName;
            Version = version;
            Status = status;
        }

        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public double? Version { get; set; }
        public string? Status { get; set; }
    }
    public class FinalQuotationResponse
    {
        public FinalQuotationResponse(Guid id, string accountName, Guid projectId, string type, string adress
            , double? totalPrice, string? note, double? version, DateTime? insDate, DateTime? upsDate,
            string? status, bool? deflag, string? reasonReject,
            List<BatchPaymentResponse> batchPaymentInfos, List<EquipmentItemsResponse> equipmentItems,
            List<FinalQuotationItemResponse> finalQuotationItems, PromotionInfo? promotionInfo, List<UtilityInf>? utilityInfos
/*            ConstructionSummary constructionRough, ConstructionSummary constructionFinished*/)
        {
            Id = id;
            AccountName = accountName;
            ProjectId = projectId;
            ProjectType = type;
            ProjectAddress = adress;
            TotalPrice = totalPrice;
            Note = note;
            Version = version;
            InsDate = insDate;
            UpsDate = upsDate;
            Status = status;
            Deflag = deflag;
            ReasonReject = reasonReject;
            BatchPaymentInfos = batchPaymentInfos;
            EquipmentItems = equipmentItems;
            FinalQuotationItems = finalQuotationItems;
            PromotionInfo = promotionInfo;
            UtilityInfos = utilityInfos;
/*            ConstructionRough = constructionRough;
            ConstructionFinished = constructionFinished;*/
        }

        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid ProjectId { get; set; }
        public string? ProjectType { get; set; }
        public string? ProjectAddress { get; set; }

        public double? TotalPrice { get; set; }

        public string? Note { get; set; }

        public double? Version { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Status { get; set; }

        public bool? Deflag { get; set; }

        public string? ReasonReject { get; set; }

        public List<BatchPaymentResponse> BatchPaymentInfos { get; set; }

        public virtual List<EquipmentItemsResponse> EquipmentItems { get; set; }

        public virtual List<FinalQuotationItemResponse> FinalQuotationItems { get; set; }

        public PromotionInfo? PromotionInfo { get; set; }

        public List<UtilityInf>? UtilityInfos { get; set; }
/*        public ConstructionSummary ConstructionRough { get; set; }
        public ConstructionSummary ConstructionFinished { get; set; }*/
    }
    public class BatchPaymentResponse
    {
        public BatchPaymentResponse(
            Guid paymentId,
            DateTime? insDate,
            string? status,
            DateTime? upsDate,
            string? description,
            string? percents,
            double? price,
            string? unit,
            DateTime? paymentDate,
            DateTime? paymentPhase)
        {
            InsDate = insDate;
            PaymentId = paymentId;
            Status = status;
            UpsDate = upsDate;
            Price = price;
            Description = description;
            Percents = percents;
            Unit = unit;
            PaymentDate = paymentDate;
            PaymentPhase = paymentPhase;
        }

        public Guid PaymentId { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
        public DateTime? UpsDate { get; set; }
        public double? Price { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentPhase { get; set; }
        public string? Unit { get; set; }
        public string? Percents { get; set; }
        public string? Description { get; set; }
    }

    public class EquipmentItemsResponse
    {
        public EquipmentItemsResponse(Guid id, string? name, string? unit, int? quantity,
            double? unitOfMaterial, double? totalOfMaterial, string? note)
        {
            Id = id;
            Name = name;
            Unit = unit;
            Quantity = quantity;
            UnitOfMaterial = unitOfMaterial;
            TotalOfMaterial = totalOfMaterial;
            Note = note;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Unit { get; set; }

        public int? Quantity { get; set; }

        public double? UnitOfMaterial { get; set; }

        public double? TotalOfMaterial { get; set; }

        public string? Note { get; set; }
    }
    public class FinalQuotationItemResponse
    {
        public FinalQuotationItemResponse(Guid id, string? contructionName, string? type, double? coefficient,
            DateTime? insDate, List<QuotationItemResponse> quotationItems)
        {
            Id = id;
            ContructionName = contructionName;
            Type = type;
            Coefficient = coefficient;
            InsDate = insDate;
            QuotationItems = quotationItems;
        }

        public Guid Id { get; set; }

        public string? ContructionName { get; set; }

        public string? Type { get; set; }

        public double? Coefficient { get; set; }

        public DateTime? InsDate { get; set; }

        public  List<QuotationItemResponse> QuotationItems { get; set; }
    }

    public class QuotationItemResponse
    {
        public QuotationItemResponse(Guid id, string? unit, double? weight, double? unitPriceLabor, double? unitPriceRough,
            double? unitPriceFinished, double? totalPriceLabor, double? totalPriceRough, double? totalPriceFinished,
            DateTime? insDate, DateTime? upsDate, string? note, List<QuotationLaborResponse> quotationLabors, List<QuotationMaterialResponse> quotationMaterials)
        {
            Id = id;
            Unit = unit;
            Weight = weight;
            UnitPriceLabor = unitPriceLabor;
            UnitPriceRough = unitPriceRough;
            UnitPriceFinished = unitPriceFinished;
            TotalPriceLabor = totalPriceLabor;
            TotalPriceRough = totalPriceRough;
            TotalPriceFinished = totalPriceFinished;
            InsDate = insDate;
            UpsDate = upsDate;
            Note = note;
            QuotationLabors = quotationLabors;
            QuotationMaterials = quotationMaterials;
        }

        public Guid Id { get; set; }

        public string? Unit { get; set; }

        public double? Weight { get; set; }

        public double? UnitPriceLabor { get; set; }

        public double? UnitPriceRough { get; set; }

        public double? UnitPriceFinished { get; set; }

        public double? TotalPriceLabor { get; set; }

        public double? TotalPriceRough { get; set; }

        public double? TotalPriceFinished { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Note { get; set; }

        public List<QuotationLaborResponse> QuotationLabors { get; set; } 

        public List<QuotationMaterialResponse> QuotationMaterials { get; set; }
    }
    public class QuotationLaborResponse
    {
        public QuotationLaborResponse(Guid id, string? laborName, double? laborPrice)
        {
            Id = id;
            LaborName = laborName;
            LaborPrice = laborPrice;
        }

        public Guid Id { get; set; }

        public string? LaborName { get; set; }

        public double? LaborPrice { get; set; }

    }
    public class QuotationMaterialResponse
    {
        public QuotationMaterialResponse(Guid id, string? materialName, string? unit, double? materialPrice)
        {
            Id = id;
            MaterialName = materialName;
            Unit = unit;
            MaterialPrice = materialPrice;
        }

        public Guid Id { get; set; }

        public string? MaterialName { get; set; }

        public string? Unit { get; set; }

        public double? MaterialPrice { get; set; }


    }
    public class UtilityInf
    {
        public UtilityInf(Guid id, string description, double coefficient, double price, double unitPrice, string unit)
        {
            Id = id;
            Description = description;
            Coefficient = coefficient;
            Price = price;
            UnitPrice = unitPrice;
            Unit = unit;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public double Coefficient { get; set; }
        public double Price { get; set; }
        public double UnitPrice { get; set; }
        public string Unit { get; set; }
    }
/*    public class ConstructionSummary
    {
        public string Type { get; set; }
        public double TotalPriceRough { get; set; }
        public double TotalPriceLabor { get; set; }

        public ConstructionSummary(string type, double totalPriceRough, double totalPriceLabor)
        {
            Type = type;
            TotalPriceRough = totalPriceRough;
            TotalPriceLabor = totalPriceLabor;
        }
    }*/
}
