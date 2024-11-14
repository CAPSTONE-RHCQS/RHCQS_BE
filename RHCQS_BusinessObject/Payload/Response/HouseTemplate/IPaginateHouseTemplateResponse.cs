using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.HouseTemplate
{
    public class IPaginateHouseTemplateResponse
    {
        public IPaginateHouseTemplateResponse() { }

        public IPaginateHouseTemplateResponse(Guid id, string name, string? description, int? numberOfFloor, int? numberOfBed,
            string? imgUrl, DateTime? insDate,
            List<IPaginateSubTemplatesResponse> subTemplates,
            List<IPaginatePackageHouseResponse> packageHouses, List<IPaginateMediaResponse> exteriorsUrls)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfFloor = numberOfFloor;
            NumberOfBed = numberOfBed;
            ImgUrl = imgUrl;
            InsDate = insDate;
            SubTemplates = subTemplates;
            PackageHouses = packageHouses;
            ExteriorsUrls = exteriorsUrls;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public List<IPaginateSubTemplatesResponse> SubTemplates { get; set; }

        public List<IPaginatePackageHouseResponse> PackageHouses { get; set; }

        public List<IPaginateMediaResponse> ExteriorsUrls { get; set; }

    }
 
    public class IPaginateSubTemplatesResponse
    {
        public IPaginateSubTemplatesResponse(Guid id, double? buildingArea, double? floorArea, DateTime? insDate, string? size,
            string? url, double? totalRough, List<TemplateItemReponse> templateItems, List<IPaginateMediaResponse> designdrawings)
        {
            Id = id;
            BuildingArea = buildingArea;
            FloorArea = floorArea;
            InsDate = insDate;
            Size = size;
            Url = url;
            TotalRough = totalRough;
            TemplateItems = templateItems;
            Designdrawings = designdrawings;
        }

        public Guid Id { get; set; }

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Size { get; set; }

        public string? Url { get; set; }
        public double? TotalRough { get; set; }

        public List<TemplateItemReponse> TemplateItems { get; set; }

        public List<IPaginateMediaResponse> Designdrawings { get; set; }
    }
    public class IPaginateMediaResponse
    {
        public IPaginateMediaResponse(Guid id, string? name, string? url, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            Name = name;
            Url = url;
            InsDate = insDate;
            UpsDate = upsDate;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
    }
    
    public class IPaginatePackageHouseResponse
    {
        public IPaginatePackageHouseResponse(Guid id, Guid packageId, string? packageName, string? imgUrl, DateTime? insDate, string? description)
        {
            Id = id;
            PackageId = packageId;
            PackageName = packageName;
            ImgUrl = imgUrl;
            InsDate = insDate;
            Description = description;
        }

        public Guid Id { get; set; }

        public Guid PackageId { get; set; }

        public string? PackageName { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Description { get; set; }

    }
}



