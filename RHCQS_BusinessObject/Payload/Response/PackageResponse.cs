﻿using RHCQS_DataAccessObjects.Models;
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
            List<PackageHousesResponse> packageHouses, List<PackagePromotionResponse> packagePromotion, List<WorkTemplateResponse> workTemplates)
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
            WorkTemplates = workTemplates;
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
        public List<WorkTemplateResponse> WorkTemplates { get; set; }

    }
    public class WorkTemplateResponse
    {
        public WorkTemplateResponse(Guid id, Guid? constructionWorkId, string? constructionWorkName, double? laborCost, double? materialCost, double? materialFinishedCost)
        {
            Id = id;
            ConstructionWorkId = constructionWorkId;
            ConstructionWorkName = constructionWorkName;
            LaborCost = laborCost;
            MaterialCost = materialCost;
            MaterialFinishedCost = materialFinishedCost;
        }

        public Guid Id { get; set; }
        public Guid? ConstructionWorkId { get; set; }
        public string? ConstructionWorkName { get; set; }
        public Double? LaborCost { get; set; }
        public Double? MaterialCost { get; set; }
        public Double? MaterialFinishedCost { get; set; }
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

        public PackageMaterialResponse(Guid id, Guid? materialId, Guid? materialSectionId, string? materialSectionName, string? materialName, string? type
            , double? price, string? unit/*, string? size, string? shape, string? imgUrl, string? description, DateTime? insDate*/)
        {
            Id = id;
            MaterialId = materialId;
            MaterialSectionId = materialSectionId;
            MaterialSectionName = materialSectionName;
            MaterialName = materialName;
            Type = type;
            Price = price;
            Unit = unit;
            //Size = size;
            //Shape = shape;
            //ImgUrl = imgUrl;
            //Description = description;
            //InsDate = insDate;
        }

        public Guid Id { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? MaterialSectionId { get; set; }
        public string? MaterialSectionName { get; set; }
        public string? MaterialName { get; set; }
        public string? Type { get; set; }

        public double? Price { get; set; }

        public string? Unit { get; set; }

        //public string? Size { get; set; }

        //public string? Shape { get; set; }

        //public string? ImgUrl { get; set; }

        //public string? Description { get; set; }

        //public DateTime? InsDate { get; set; }
    }

    public class PackageHousesResponse
    {
        public PackageHousesResponse(Guid id, Guid designTemplateId, string? name, string? imgUrl,
            string? description, int? numberOfBed, int? numberOfFloor, DateTime? insDate)
        {
            Id = id;
            DesignTemplateId = designTemplateId;
            DesignName = name;
            ImgUrl = imgUrl;
            Description = description;
            NumberOfBed = numberOfBed;
            NumberOfFloor = numberOfFloor;
            InsDate = insDate;
        }

        public Guid Id { get; set; }

        public Guid DesignTemplateId { get; set; }
        public string? DesignName { get; set; }
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public int? NumberOfBed { get; set; }
        public int? NumberOfFloor { get; set; }
        public DateTime? InsDate { get; set; }
    }
    public class PackageResponseForMobile
    {
        public PackageResponseForMobile(Guid id, string? packageType, string? packageName,
            string? unit, double? price, string? status, DateTime? insDate, DateTime? upsDate/*,
            List<PackageLaborResponseForMoblie> packageLabor, List<PackageMaterialResponse> packagematerial,
            List<PackageHousesResponse> packageHouses*/)
        {
            Id = id;
            PackageType = packageType;
            PackageName = packageName;
            Unit = unit;
            Price = price;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            //PackageLabors = packageLabor;
            //PackageMaterials = packagematerial;
            //PackageHouses = packageHouses;
        }

        public Guid Id { get; set; }

        public string? PackageType { get; set; }

        public string? PackageName { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }

        public string? Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
        //public List<PackageLaborResponseForMoblie> PackageLabors { get; set; }

        //public List<PackageMaterialResponse> PackageMaterials { get; set; }

        //public List<PackageHousesResponse> PackageHouses { get; set; }


    }
    //public class PackageLaborResponseForMoblie
    //{
    //    public PackageLaborResponseForMoblie(string? nameOfLabor, string? type)
    //    {
    //        NameOfLabor = nameOfLabor;
    //        Type = type;
    //    }

    //    public string? NameOfLabor { get; set; }
    //    public string? Type { get; set; }

    //}
}
