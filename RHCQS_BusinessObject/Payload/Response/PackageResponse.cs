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
        public PackageDetailsResponse(Guid id, string? type, DateTime? insDate,
            List<PackageLaborResponse> packageLabors, List<PackageMaterialResponse> packageMaterials)
        {
            Id = id;
            Type = type;
            InsDate = insDate;
            PackageLabors = packageLabors;
            PackageMaterials = packageMaterials;
        }

        public Guid Id { get; set; }

        public string? Type { get; set; }

        public DateTime? InsDate { get; set; }
        public List<PackageLaborResponse> PackageLabors { get; set; }

        public List<PackageMaterialResponse> PackageMaterials { get; set; }
    }
    public class PackageLaborResponse
    {

        public PackageLaborResponse(Guid id, Guid laborId, string? nameOfLabor, string? type, double? totalPrice, DateTime? insDate)
        {
            Id = id;
            LaborId = laborId;
            NameOfLabor = nameOfLabor;
            Type = type;
            TotalPrice = totalPrice;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid LaborId { get; set; }

        public string? NameOfLabor { get; set; }
        public string? Type { get; set; }

        public double? TotalPrice { get; set; }

        public DateTime? InsDate { get; set; }
    }
    public class PackageMaterialResponse
    {

        public PackageMaterialResponse(Guid id, Guid? materialSectionId, string? materialSectionName, string? materialName
            , double? price, string? unit, string? size, string? shape, string? imgUrl, string? description, DateTime? insDate)
        {
            Id = id;
            MaterialSectionId = materialSectionId;
            MaterialSectionName = materialSectionName;
            MaterialName = materialName;
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
        public PackageResponseForMoblie(Guid id, Guid packageTypeId, string? packageType, string? packageName,
            string? unit, double? price, string? status, DateTime? insDate, DateTime? upsDate,
            List<PackageDetailsResponseForMoblie> packageDetails, List<PackageHousesResponse> packageHouses)
        {
            Id = id;
            PackageTypeId = packageTypeId;
            PackageType = packageType;
            PackageName = packageName;
            Unit = unit;
            Price = price;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            PackageDetails = packageDetails;
            PackageHouses = packageHouses;
        }

        public Guid Id { get; set; }

        public Guid PackageTypeId { get; set; }

        public string? PackageType { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        public List<PackageDetailsResponseForMoblie> PackageDetails { get; set; }

        public List<PackageHousesResponse> PackageHouses { get; set; }


    }
    public class PackageDetailsResponseForMoblie
    {
        public PackageDetailsResponseForMoblie(Guid id, string? type, DateTime? insDate,
            List<PackageLaborResponseForMoblie> packageLabors, List<PackageMaterialResponse> packageMaterials)
        {
            Id = id;
            Type = type;
            InsDate = insDate;
            PackageLabors = packageLabors;
            PackageMaterials = packageMaterials;
        }

        public Guid Id { get; set; }

        public string? Type { get; set; }

        public DateTime? InsDate { get; set; }
        public List<PackageLaborResponseForMoblie> PackageLabors { get; set; }

        public List<PackageMaterialResponse> PackageMaterials { get; set; }
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
