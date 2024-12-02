using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class PackageListResponse
    {
        public PackageListResponse(Guid id, string? packageName, string? unit, double? price, string? status, string packageType)
        {
            Id = id;
            PackageName = packageName;
            Unit = unit;
            Price = price;
            Status = status;
            PackageType = packageType;
        }

        public Guid Id { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public string PackageType { get; set; } = null!;
    }
    public class PackageResponse
    {
        public PackageResponse(Guid id, string? packageName, string packageType, string? unit, double? price, string? status,
            DateTime? insDate, DateTime? upsDate, List<PackageLaborResponse> packageLabor, List<PackageMaterialResponse> packageMaterial,
            List<PackageHousesResponse> packageHouses, List<PackagePromotionResponse> packagePromotion)
        {
            Id = id;
            PackageName = packageName;
            PackageType = packageType;
            Unit = unit;
            Price = price;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            PackageLabors = packageLabor;
            PackageMaterials = packageMaterial;
            PackageHouses = packageHouses;
            PackageMapPromotions = packagePromotion;
        }

        public Guid Id { get; set; }

        public string? PackageName { get; set; }
        public string PackageType { get; set; } = null!;
        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        public List<PackageLaborResponse> PackageLabors { get; set; }

        public List<PackageMaterialResponse> PackageMaterials { get; set; }

        public List<PackageHousesResponse> PackageHouses { get; set; }
        public List<PackagePromotionResponse> PackageMapPromotions { get; set; }

    }
    public class PackagePromotionResponse
    {
        public PackagePromotionResponse(Guid id, Guid promotionId, string? promotionName, int? value, DateTime? startDate, DateTime? insDate)
        {
            Id = id;
            PromotionId = promotionId;
            PromotionName = promotionName;
            Value = value;
            StartDate = startDate;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public int? Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? InsDate { get; set; }
    }
    public class PackageLaborResponse
    {

        public PackageLaborResponse(Guid id, Guid laborId, string? nameOfLabor, string? type, double? totalPrice, DateTime? insDate)
        {
            Id = id;
            LaborId = laborId;
            NameOfLabor = nameOfLabor;
            Type = type;
            Price = totalPrice;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid LaborId { get; set; }

        public string? NameOfLabor { get; set; }
        public string? Type { get; set; }

        public double? Price { get; set; }

        public DateTime? InsDate { get; set; }
    }
    public class PackageMaterialResponse
    {

        public PackageMaterialResponse(Guid id, Guid? materialSectionId, string? materialSectionName, string? materialName, string? type
            , double? price, string? unit, string? size, string? shape, string? imgUrl, string? description, DateTime? insDate)
        {
            Id = id;
            MaterialSectionId = materialSectionId;
            MaterialSectionName = materialSectionName;
            MaterialName = materialName;
            Type = type;
            Price = price;
            Unit = unit;
            Size = size;
            Shape = shape;
            ImgUrl = imgUrl;
            Description = description;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid? MaterialSectionId { get; set; }
        public string? MaterialSectionName { get; set; }
        public string? MaterialName { get; set; }
        public string? Type { get; set; }

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
    public class PackageResponseForMoblie
    {
        public PackageResponseForMoblie(Guid id, string? packageType, string? packageName,
            string? unit, double? price, string? status, DateTime? insDate, DateTime? upsDate,
            List<PackageLaborResponseForMoblie> packageLabor, List<PackageMaterialResponse> packagematerial,
            List<PackageHousesResponse> packageHouses)
        {
            Id = id;
            PackageType = packageType;
            PackageName = packageName;
            Unit = unit;
            Price = price;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            PackageLabors = packageLabor;
            PackageMaterials = packagematerial;
            PackageHouses = packageHouses;
        }

        public Guid Id { get; set; }

        public string? PackageType { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        public List<PackageLaborResponseForMoblie> PackageLabors { get; set; }

        public List<PackageMaterialResponse> PackageMaterials { get; set; }

        public List<PackageHousesResponse> PackageHouses { get; set; }


    }
    public class PackageLaborResponseForMoblie
    {
        public PackageLaborResponseForMoblie(string? nameOfLabor, string? type)
        {
            NameOfLabor = nameOfLabor;
            Type = type;
        }

        public string? NameOfLabor { get; set; }
        public string? Type { get; set; }

    }
}
