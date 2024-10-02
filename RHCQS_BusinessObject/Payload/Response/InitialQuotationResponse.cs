using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class InitialQuotationListResponse
    {
        public InitialQuotationListResponse(Guid id, string customerName, string? version, double? area, string? status)
        {
            Id = id;
            CustomerName = customerName;
            Version = version;
            Area = area;
            Status = status;
        }

        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string? Version { get; set; }
        public double? Area { get; set; }
        public string? Status { get; set; }
    }

    public class InitialQuotationResponse
    {
        public InitialQuotationResponse() { }

        public InitialQuotationResponse(Guid id, string accountName, Guid projectId, Guid? promotionId, Guid packageId,
            double? area, int? timeProccessing, int? timeOthers, string? othersAgreement, DateTime? insDate, string? status,
            string? version, bool deflag, string? note, PackageQuotationList packageQuotationList, List<InitialQuotationItemResponse> itemInitial)
        {
            Id = id;
            AccountName = accountName;
            ProjectId = projectId;
            PromotionId = promotionId;
            PackageId = packageId;
            Area = area;
            TimeProcessing = timeProccessing;
            TimeOthers = timeOthers;
            OthersAgreement = othersAgreement;
            InsDate = insDate;
            Status = status;
            Version = version;
            Deflag = deflag;
            Note = note;
            PackageQuotationList = packageQuotationList;
            ItemInitial = itemInitial;
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
        public string? Version { get; set; }
        public bool Deflag { get; set; }
        public string? Note { get; set; }
        public PackageQuotationList PackageQuotationList { get; set; }
        public List<InitialQuotationItemResponse> ItemInitial { get; set; }
    }


    public class InitialQuotationItemResponse
    {
        public InitialQuotationItemResponse(Guid id, string? name, string? subConstruction, double? area, double? price,
            string? unitPrice, string constructionName)
        {
            Id = id;
            Name = name;
            SubConstruction = subConstruction;
            Area = area;
            Price = price;
            UnitPrice = unitPrice;
            ConstructionName = constructionName;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? SubConstruction { get; set; }

        public double? Area { get; set; }

        public double? Price { get; set; }

        public string? UnitPrice { get; set; }

        public string ConstructionName { get; set; }
    }

    public class PackageQuotationList
    {
        public PackageQuotationList(Guid idPackageRough, string packageRough, Guid idPackageFinished, string packageFinished)
        {
            IdPackageRough = idPackageRough;
            PackageRough = packageRough;
            IdPackageFinished = idPackageFinished;
            PackageFinished = packageFinished;
        }

        public Guid IdPackageRough { get; set; }
        public string PackageRough { get; set; }
        public Guid IdPackageFinished { get; set; }
        public string PackageFinished { get; set; }
    }
}
