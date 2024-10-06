using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class PackageResponse
    {
        public PackageResponse(Guid id, Guid packageTypeId, string? packageName, string? unit, double? price, string? status,
            DateTime? insDate, DateTime? upsDate, List<PackageDetailsResponse> packageDetails, List<PackageHousesResponse> packageHouses,
            PackageTypeResponse packageType)
        {
            Id = id;
            PackageTypeId = packageTypeId;
            PackageName = packageName;
            Unit = unit;
            Price = price;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            PackageDetails = packageDetails;
            PackageHouses = packageHouses;
            PackageType = packageType;
        }

        public Guid Id { get; set; }

        public Guid PackageTypeId { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        public List<PackageDetailsResponse> PackageDetails { get; set; }

        public List<PackageHousesResponse> PackageHouses { get; set; }

        public PackageTypeResponse PackageType { get; set; } = null!;

    }
    public class PackageDetailsResponse
    {
        public PackageDetailsResponse(Guid id, string? action, string? type, DateTime? insDate,
            List<PackageLaborResponse> packageLabors, List<PackageMaterialResponse> packageMaterials)
        {
            Id = id;
            Action = action;
            Type = type;
            InsDate = insDate;
            PackageLabors = packageLabors;
            PackageMaterials = packageMaterials;
        }

        public Guid Id { get; set; }

        public string? Action { get; set; }

        public string? Type { get; set; }

        public DateTime? InsDate { get; set; }
        public List<PackageLaborResponse> PackageLabors { get; set; }

        public List<PackageMaterialResponse> PackageMaterials { get; set; }
    }
    public class PackageLaborResponse
    {
        public PackageLaborResponse(Guid id, Guid laborId, string? nameOfLabor, double? totalPrice, int? quantity, DateTime? insDate)
        {
            Id = id;
            LaborId = laborId;
            NameOfLabor = nameOfLabor;
            TotalPrice = totalPrice;
            Quantity = quantity;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid LaborId { get; set; }

        public string? NameOfLabor { get; set; }

        public double? TotalPrice { get; set; }

        public int? Quantity { get; set; }

        public DateTime? InsDate { get; set; }
    }
    public class PackageMaterialResponse
    {

        public PackageMaterialResponse(Guid id, Guid materialSectionId, string? materialSectionName, string? materialnName,
            int? inventoryQuantity, double? price, string? unit, string? size, string? shape, string? imgUrl, string? description, DateTime? insDate)
        {
            Id = id;
            MaterialSectionId = materialSectionId;
            MaterialSectionName = materialSectionName;
            MaterialnName = materialnName;
            InventoryQuantity = inventoryQuantity;
            Price = price;
            Unit = unit;
            Size = size;
            Shape = shape;
            ImgUrl = imgUrl;
            Description = description;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid MaterialSectionId { get; set; }
        public string? MaterialSectionName { get; set; }
        public string? MaterialnName { get; set; }
        public int? InventoryQuantity { get; set; }

        public double? Price { get; set; }

        public string? Unit { get; set; }

        public string? Size { get; set; }

        public string? Shape { get; set; }

        public string? ImgUrl { get; set; }

        public string? Description { get; set; }

        public DateTime? InsDate { get; set; }
    }

    public class PackageHousesResponse
    {
        public PackageHousesResponse(Guid id, Guid designTemplateId, string? imgUrl, DateTime? insDate)
        {
            Id = id;
            DesignTemplateId = designTemplateId;
            ImgUrl = imgUrl;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid DesignTemplateId { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }
    }
}
