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

        public FinalQuotationResponse(Guid id, string accountName,string phone, string email, Guid projectId,double? area, Guid initailQuotationId,
            double? initailQuotationVersion, List<HouseDrawingVersionInf>? houseDrawingVersionInf, PackageQuotationList packageQuotationList,
            string? projectType,string? projectAddress,double? discount, double? totalPrice, string? note,
            string? othersAgreement, double? version, DateTime? insDate,
            DateTime? upsDate, string? status, bool? deflag, string? reasonReject,List<InitQuotationInfo> initQuotationInfos,
            List<BatchPaymentResponse> batchPaymentInfos, List<EquipmentItemsResponse> equipmentItems,
            List<FinalQuotationItemResponse> finalQuotationItems, PromotionInfo? promotionInfo,
            List<UtilityInf>? utilityInfos, ConstructionSummary constructionRough, ConstructionSummary constructionFinished, ConstructionSummary equitment)
        {
            Id = id;
            AccountName = accountName;
            PhoneNumber = phone;
            Email = email;
            ProjectId = projectId;
            Area = area;
            InitailQuotationId = initailQuotationId;
            InitailQuotationVersion = initailQuotationVersion;
            HouseDrawingVersionInf = houseDrawingVersionInf;
            PackageQuotationList = packageQuotationList;
            ProjectType = projectType;
            ProjectAddress = projectAddress;
            Discount = discount;
            TotalPrice = totalPrice;
            Note = note;
            OthersAgreement = othersAgreement;
            Version = version;
            InsDate = insDate;
            UpsDate = upsDate;
            Status = status;
            Deflag = deflag;
            ReasonReject = reasonReject;
            InitQuotationInfos = initQuotationInfos;
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
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid ProjectId { get; set; }
        public double? Area { get; set; }
        public Guid InitailQuotationId { get; set; }
        public double? InitailQuotationVersion { get; set; }
        public List<HouseDrawingVersionInf>? HouseDrawingVersionInf { get; set; }
        public PackageQuotationList PackageQuotationList { get; set; }
        public string? ProjectType { get; set; }
        public string? ProjectAddress { get; set; }

        public double? Discount { get; set; }

        public double? TotalPrice { get; set; }

        public string? Note { get; set; }

        public string? OthersAgreement { get; set; }

        public double? Version { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public string? Status { get; set; }

        public bool? Deflag { get; set; }

        public string? ReasonReject { get; set; }

        public List<InitQuotationInfo> InitQuotationInfos { get; set; }

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
            int percents,
            double? price,
            string? unit,
            DateTime? paymentDate,
            DateTime? paymentPhase,
            int? numberOfBatch)
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
            NumberOfBatch = numberOfBatch;
        }

        public Guid PaymentId { get; set; }
        public Guid PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public Guid? ContractId { get; set; }

        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
        public DateTime? UpsDate { get; set; }
        public double? Price { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentPhase { get; set; }
        public string? Unit { get; set; }
        public int Percents { get; set; }
        public string? Description { get; set; }
        public int? NumberOfBatch { get; set; }
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
    public class InitQuotationInfo
    {
        public InitQuotationInfo( string? constructionName, double? area)
        {
            ConstructionName = constructionName;
            Area = area;
        }

        public string? ConstructionName { get; set; }
        public Double? Area { get; set; }

    }
    public class FinalQuotationItemResponse
    {
        public FinalQuotationItemResponse(Guid id, Guid contructionId, string? contructionName,
            string? type,DateTime? insDate, List<QuotationItemResponse> quotationItems)
        {
            Id = id;
            ConstructionId = contructionId;
            ContructionName = contructionName;
            Type = type;
            InsDate = insDate;
            QuotationItems = quotationItems;
        }

        public Guid Id { get; set; }
        public Guid ConstructionId { get; set; }

        public string? ContructionName { get; set; }
        public string? Type { get; set; }

        public DateTime? InsDate { get; set; }

        public  List<QuotationItemResponse> QuotationItems { get; set; }
    }

    public class QuotationItemResponse
    {
        public QuotationItemResponse(Guid id, Guid? workid,string? name,string? unit, double? weight, double? unitPriceLabor, double? unitPriceRough,
            double? unitPriceFinished, double? totalPriceLabor, double? totalPriceRough, double? totalPriceFinished,
            DateTime? insDate, DateTime? upsDate, string? note)
        {
            Id = id;
            WorkTemplateId = workid;
            WorkName = name;
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

        }

        public Guid Id { get; set; }

        public Guid? WorkTemplateId { get; set; }
        public string? WorkName { get; set; }
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


    }

    public class UtilityInf
    {
        public UtilityInf(Guid id, Guid? utilitiesItemId, Guid? utilitiesSectionId, string name, string description,
            double coefficient, double price, double unitPrice, string unit, int? quantity)
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
            Quantity = quantity;
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
        public int? Quantity { get; set; }
    }
    public class HouseDrawingVersionInf
    {
        public HouseDrawingVersionInf(Guid designId, string versionName, string url, double? version)
        {
            DesignId = designId;
            VersionName = versionName;
            Url = url;
            Version = version;
        }

        public Guid DesignId { get; set; }
        public string VersionName { get; set; }
        public string Url { get; set; }
        public double? Version { get; set; }

    }
    public class ConstructionSummary
    {
        public string Type { get; set; }
        public double TotalPriceFinished { get; set; }
        public double TotalPriceRough { get; set; }
        public double TotalPriceLabor { get; set; }

        public ConstructionSummary()
        {
        }

        public ConstructionSummary(string type, double totalPriceRough, double totalPriceFinished, double totalPriceLabor)
        {
            Type = type;
            TotalPriceRough = totalPriceRough;
            TotalPriceFinished = totalPriceFinished;
            TotalPriceLabor = totalPriceLabor;
        }
    }
}
