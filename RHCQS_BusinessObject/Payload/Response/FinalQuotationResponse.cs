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
/*        public FinalQuotationResponse(Guid id, string accountName, Guid projectId, Guid initialId, string type, string adress
            , double? totalPrice, string? note, double? version, DateTime? insDate, DateTime? upsDate,
            string? status, bool? deflag, string? reasonReject,
            List<BatchPaymentResponse> batchPaymentInfos, List<EquipmentItemsResponse> equipmentItems,
            List<FinalQuotationItemResponse> finalQuotationItems, PromotionInfo? promotionInfo, List<UtilityInf>? utilityInfos,
            ConstructionSummary constructionRough, ConstructionSummary constructionFinished, ConstructionSummary equitment)
        {
            Id = id;
            AccountName = accountName;
            ProjectId = projectId;
            InitailQuotationId = initialId;
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
            ConstructionRough = constructionRough;
            ConstructionFinished = constructionFinished;
            Equitment = equitment;
        }*/

        public FinalQuotationResponse(Guid id, string accountName, Guid projectId, Guid initailQuotationId,
            double? initailQuotationVersion, List<HouseDrawingVersionInf>? houseDrawingVersionInf, string? projectType,
            string? projectAddress, double? totalPrice, string? note, double? version, DateTime? insDate,
            DateTime? upsDate, string? status, bool? deflag, string? reasonReject,
            List<BatchPaymentResponse> batchPaymentInfos, List<EquipmentItemsResponse> equipmentItems,
            List<FinalQuotationItemResponse> finalQuotationItems, PromotionInfo? promotionInfo,
            List<UtilityInf>? utilityInfos, ConstructionSummary constructionRough, ConstructionSummary constructionFinished, ConstructionSummary equitment)
        {
            Id = id;
            AccountName = accountName;
            ProjectId = projectId;
            InitailQuotationId = initailQuotationId;
            InitailQuotationVersion = initailQuotationVersion;
            HouseDrawingVersionInf = houseDrawingVersionInf;
            ProjectType = projectType;
            ProjectAddress = projectAddress;
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
            ConstructionRough = constructionRough;
            ConstructionFinished = constructionFinished;
            Equitment = equitment;
        }

        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InitailQuotationId { get; set; }
        public double? InitailQuotationVersion { get; set; }
        public List<HouseDrawingVersionInf>? HouseDrawingVersionInf { get; set; }

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
        public ConstructionSummary ConstructionRough { get; set; }
        public ConstructionSummary ConstructionFinished { get; set; }
        public ConstructionSummary Equitment { get; set; }
    }
    public class BatchPaymentResponse
    {
        public BatchPaymentResponse(
            Guid paymentId,
            Guid paymentTypeId,
            string? paymentTypeName,
            Guid? contractId,
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
            PaymentTypeId = paymentTypeId;
            PaymentTypeName = paymentTypeName;
            ContractId = contractId;
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
        public Guid PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        //public Guid InitailQuotationId { get; set; }

        public Guid? ContractId { get; set; }

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
        public EquipmentItemsResponse(Guid id, string? name, string? unit, int? quantity, double? unitOfMaterial, double? totalOfMaterial, string? note, string? type)
        {
            Id = id;
            Name = name;
            Unit = unit;
            Quantity = quantity;
            UnitOfMaterial = unitOfMaterial;
            TotalOfMaterial = totalOfMaterial;
            Note = note;
            Type = type;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Unit { get; set; }

        public int? Quantity { get; set; }

        public double? UnitOfMaterial { get; set; }

        public double? TotalOfMaterial { get; set; }

        public string? Note { get; set; }
        public string? Type { get; set; }
    }
    public class FinalQuotationItemResponse
    {
        public FinalQuotationItemResponse(Guid id, Guid contructionId, Guid? subcontructionId, string? contructionName, string? type/*, double? coefficient*/,
            DateTime? insDate, List<QuotationItemResponse> quotationItems)
        {
            Id = id;
            ConstructionId = contructionId;
            SubconstructionId = subcontructionId;
            ContructionName = contructionName;
            Type = type;
            //Coefficient = coefficient;
            InsDate = insDate;
            QuotationItems = quotationItems;
        }

        public Guid Id { get; set; }
        public Guid ConstructionId { get; set; }

        public Guid? SubconstructionId { get; set; }
        public string? ContructionName { get; set; }

        public string? Type { get; set; }

        //public double? Coefficient { get; set; }

        public DateTime? InsDate { get; set; }

        public  List<QuotationItemResponse> QuotationItems { get; set; }
    }

    public class QuotationItemResponse
    {
        public QuotationItemResponse(Guid id,string? name/*, string? code*/, string? unit, double? weight, double? unitPriceLabor, double? unitPriceRough,
            double? unitPriceFinished, double? totalPriceLabor, double? totalPriceRough, double? totalPriceFinished,
            DateTime? insDate, DateTime? upsDate, string? note/*, List<QuotationLaborResponse> quotationLabors, List<QuotationMaterialResponse> quotationMaterials*/)
        {
            Id = id;
            Name = name;
            //Code = code;
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
/*            QuotationLabors = quotationLabors;
            QuotationMaterials = quotationMaterials;*/
        }

        public QuotationItemResponse(Guid id, Guid laborId, string? name/*, string? code*/, string? unit, double? weight,
            double? unitPriceLabor, double? totalPriceLabor, DateTime? insDate, DateTime? upsDate, string? note)
        {
            Id = id;
            LaborId = laborId;
            Name = name;
            //Code = code;
            Unit = unit;
            Weight = weight;
            UnitPriceLabor = unitPriceLabor;
            TotalPriceLabor = totalPriceLabor;
            InsDate = insDate;
            UpsDate = upsDate;
            Note = note;
        }

        public QuotationItemResponse(Guid id, Guid materialId, string? name/*, string? code*/, string? unit, double? weight,
            double? unitPriceRough, double? unitPriceFinished, double? totalPriceRough,
            double? totalPriceFinished, DateTime? insDate, DateTime? upsDate, string? note)
        {
            Id = id;
            MaterialId = materialId;
            Name = name;
            //Code = code;
            Unit = unit;
            Weight = weight;
            UnitPriceRough = unitPriceRough;
            UnitPriceFinished = unitPriceFinished;
            TotalPriceRough = totalPriceRough;
            TotalPriceFinished = totalPriceFinished;
            InsDate = insDate;
            UpsDate = upsDate;
            Note = note;
        }

        public Guid Id { get; set; }
        public Guid? LaborId { get; set; }
        public Guid? MaterialId { get; set; }
        public string? Name { get; set; }

        //public string? Code { get; set; }
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

        //public List<QuotationLaborResponse> QuotationLabors { get; set; }

        //public List<QuotationMaterialResponse> QuotationMaterials { get; set; }
    }
    //public class QuotationLaborResponse
    //{
    //    public QuotationLaborResponse(Guid id, Guid laborId, string? laborName, double? laborPrice)
    //    {
    //        Id = id;
    //        LaborId = laborId;
    //        LaborName = laborName;
    //        LaborPrice = laborPrice;
    //    }

    //    public Guid Id { get; set; }
    //    public Guid LaborId { get; set; }
    //    public string? LaborName { get; set; }

    //    public double? LaborPrice { get; set; }

    //}
    //public class QuotationMaterialResponse
    //{
    //    public QuotationMaterialResponse(Guid id, Guid materialId, string? materialName, string? unit, double? materialPrice)
    //    {
    //        Id = id;
    //        MaterialId = materialId;
    //        MaterialName = materialName;
    //        Unit = unit;
    //        MaterialPrice = materialPrice;
    //    }

    //    public Guid Id { get; set; }

    //    public Guid MaterialId { get; set; }

    //    public string? MaterialName { get; set; }

    //    public string? Unit { get; set; }

    //    public double? MaterialPrice { get; set; }


    //}
    public class UtilityInf
    {
        public UtilityInf(Guid id, Guid? utilitiesItemId, Guid? utilitiesSectionId, string name, string description,
            double coefficient, double price, double unitPrice, string unit)
        {
            Id = id;
            this.utilitiesItemId = utilitiesItemId;
            this.utilitiesSectionId = utilitiesSectionId;
            Name = name;
            Description = description;
            Coefficient = coefficient;
            Price = price;
            UnitPrice = unitPrice;
            Unit = unit;
        }

        public Guid Id { get; set; }
        public Guid? utilitiesItemId { get; set; }
        public Guid? utilitiesSectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Coefficient { get; set; }
        public double Price { get; set; }
        public double UnitPrice { get; set; }
        public string Unit { get; set; }
    }
    public class HouseDrawingVersionInf
    {
        public HouseDrawingVersionInf(Guid versionid, string versionname, double? version)
        {
            VersionId = versionid;
            VersionName = versionname;
            Version = version;
        }

        public Guid VersionId { get; set; }
        public string VersionName { get; set; }
        public double? Version { get; set; }

    }
    public class ConstructionSummary
    {
        public string Type { get; set; }
        public double TotalPriceRough { get; set; }
        public double TotalPriceLabor { get; set; }

        public ConstructionSummary()
        {
        }

        public ConstructionSummary(string type, double totalPriceRough, double totalPriceLabor)
        {
            Type = type;
            TotalPriceRough = totalPriceRough;
            TotalPriceLabor = totalPriceLabor;
        }
    }
}
