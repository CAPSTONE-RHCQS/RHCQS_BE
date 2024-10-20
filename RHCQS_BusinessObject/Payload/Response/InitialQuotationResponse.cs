using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class InitialQuotationListResponse
    {
        public InitialQuotationListResponse(Guid id, string customerName, double? version, double? area, string? status)
        {
            Id = id;
            CustomerName = customerName;
            Version = version;
            Area = area;
            Status = status;
        }

        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public double? Version { get; set; }
        public double? Area { get; set; }
        public string? Status { get; set; }
    }

    public class InitialQuotationResponse
    {
        public InitialQuotationResponse() { }

        public InitialQuotationResponse(Guid id, string accountName, Guid projectId, Guid? promotionId,
            double? area, int? timeProccessing, int? timeOthers, string? othersAgreement, DateTime? insDate, string? status,
            double? version, bool deflag, string? note, double? totalRough, double? totalUtilities, string? unit,
            PackageQuotationList packageQuotationList, List<InitialQuotationItemResponse> itemInitial,
            PromotionInfo promotionInfo, List<BatchPaymentInfo> batchPaymentInfos)
        {
            Id = id;
            AccountName = accountName;
            ProjectId = projectId;
            PromotionId = promotionId;
            Area = area;
            TimeProcessing = timeProccessing;
            TimeOthers = timeOthers;
            OthersAgreement = othersAgreement;
            InsDate = insDate;
            Status = status;
            Version = version;
            Deflag = deflag;
            Note = note;
            TotalRough = totalRough;
            TotalUtilities = totalUtilities;
            Unit = unit;
            PackageQuotationList = packageQuotationList;
            ItemInitial = itemInitial;
            PromotionInfo = promotionInfo;
            BatchPaymentInfos = batchPaymentInfos;
        }

        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid PackageId { get; set; }
        public double? Area { get; set; }
        public int? TimeProcessing { get; set; }
        public int? TimeOthers { get; set; }
        public string? OthersAgreement { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Status { get; set; }
        public double? Version { get; set; }
        public bool Deflag { get; set; }
        public string? Note { get; set; }
        public double? TotalRough { get; set; }

        public double? TotalUtilities { get; set; }

        public string? Unit { get; set; }
        public string? ReasonReject { get; set; }
        public PackageQuotationList PackageQuotationList { get; set; }
        public List<InitialQuotationItemResponse> ItemInitial { get; set; }
        public List<UtilityInfo>? UtilityInfos { get; set; }
        public PromotionInfo? PromotionInfo { get; set; }
        public List<BatchPaymentInfo> BatchPaymentInfos { get; set; }
    }


    public class InitialQuotationItemResponse
    {
        public InitialQuotationItemResponse(Guid id, string? name, 
            Guid constructionItemId,
            string? subConstruction, Guid? subConstructionId, 
            double? area,
            double? price,
            string? unitPrice, double? subCoefficient, double? coefficient)
        {
            Id = id;
            Name = name;
            ConstructionItemId = constructionItemId;
            SubConstruction = subConstruction;
            SubConstructionId = subConstructionId;
            Area = area;
            Price = price;
            UnitPrice = unitPrice;
            SubCoefficient = subCoefficient;
            Coefficient = coefficient;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public Guid ConstructionItemId { get; set; }

        public string? SubConstruction { get; set; }
        public Guid? SubConstructionId { get; set; }

        public double? Area { get; set; }

        public double? Price { get; set; }

        public string? UnitPrice { get; set; }

        public double? SubCoefficient { get; set; }

        public double? Coefficient { get; set; }
    }

    public class PackageQuotationList
    {
        public PackageQuotationList(Guid idPackageRough, string packageRough, double unitPackageRough,
            Guid idPackageFinished, string packageFinished, double unitPackageFinished, string unit)
        {
            IdPackageRough = idPackageRough;
            PackageRough = packageRough;
            UnitPackageRough = unitPackageRough;
            IdPackageFinished = idPackageFinished;
            PackageFinished = packageFinished;
            UnitPackageFinished = unitPackageFinished;
            Unit = unit;
        }

        public Guid IdPackageRough { get; set; }
        public string PackageRough { get; set; }
        public double UnitPackageRough { get; set; }
        public Guid IdPackageFinished { get; set; }
        public string PackageFinished { get; set; }
        public double UnitPackageFinished { get; set; }
        public string Unit { get; set; }
    }

    public class PromotionInfo
    {
        public PromotionInfo() { }
        public PromotionInfo(Guid idPromotion, string? name, int? value)
        {
            Id = idPromotion;
            Name = name;
            Value = value;
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Value { get; set; }
    }

    public class UtilityInfo
    {
        public UtilityInfo(Guid id, string description, double coefficient, double price)
        {
            Id = id;
            Description = description;
            Coefficient = coefficient;
            Price = price;
        }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double Coefficient { get; set; }
        public double Price { get; set; }
    }

    public class BatchPaymentInfo
    {
        public BatchPaymentInfo(Guid id, string? description, string? percents, double? price,
            string? unit, string? status, DateTime? paymentDate, DateTime? paymentPhase)
        {
            PaymentId = id;
            Description = description;
            Percents = percents;
            Price = price;
            Unit = unit;
            Status = status;
            PaymentDate = paymentDate;
            PaymentPhase = paymentPhase;
        }
        public Guid PaymentId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Percents { get; set; }
        public double? Price { get; set; }
        public string? Unit { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentPhase { get; set; }
    }
}
