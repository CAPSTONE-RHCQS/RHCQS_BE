using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class InitialQuotationForDesignStaffResponse
    {
        public string ProjectType { get; set; }
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public Guid ProjectId { get; set; }
        public double? Area { get; set; }
        //public int? TimeProcessing { get; set; }
        //public int? TimeOthers { get; set; }
        //public int? TimeRough { get; set; }
        //public string? Status { get; set; }
        public double? TotalRough { get; set; }

        public double? TotalUtilities { get; set; }
        public List<InitialQuotationItemResponseForDesign> ItemInitial { get; set; }
        public List<UtilityInfoForDesign>? UtilityInfos { get; set; }
    }

    public class InitialQuotationItemResponseForDesign
    {
        public InitialQuotationItemResponseForDesign(Guid id, string? name,
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

    public class UtilityInfoForDesign
    {
        public UtilityInfoForDesign(Guid id, string description, double coefficient,
            double price, int? quantity, double unitPrice, double totalPrice)
        {
            Id = id;
            Description = description;
            Coefficient = coefficient;
            Price = price;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double Coefficient { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
