using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class HouseTemplateResponse
    {
        public HouseTemplateResponse() { }
        public HouseTemplateResponse(Guid id, string name, string? description, int? numberOfFloor, int? numberOfBed,
            int? numberOfFront, string? imgUrl, DateTime? insDate,/* List<PackageHouseResponse> packageHouses,*/ List<SubTemplatesResponse> subTemplates)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfFloor = numberOfFloor;
            NumberOfBed = numberOfBed;
            NumberOfFront = numberOfFront;
            ImgUrl = imgUrl;
            InsDate = insDate;
 /*           PackageHouses = packageHouses;*/
            SubTemplates = subTemplates;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public int? NumberOfFront { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public List<SubTemplatesResponse> SubTemplates { get; set; }

/*        public List<PackageHouseResponse> PackageHouses { get; set; }*/

    }
    public class SubTemplatesResponse
    {
        public SubTemplatesResponse(Guid id, double? buildingArea, double? floorArea, string? size, DateTime? insDate, List<TemplateItemReponse> templateItems)
        {
            Id = id;
            BuildingArea = buildingArea;
            FloorArea = floorArea;
            Size = size;
            InsDate = insDate;
            TemplateItems = templateItems;
        }
        public Guid Id { get; set; }

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Size { get; set; }

        public List<TemplateItemReponse> TemplateItems { get; set; }
    }
    public class TemplateItemReponse
    {
        public TemplateItemReponse(Guid id, string? name, Guid contructionid, Guid? subcontructionid, double? coefficient, double? area, string? unit, DateTime? insDate)
        {
            Id = id;
            Name = name;
            ConstructionId = contructionid;
            SubConstructionId = subcontructionid;
            Coefficient = coefficient;
            Area = area;
            Unit = unit;
            InsDate = insDate;
        }
        public Guid Id { get; set; }

        public Guid ConstructionId { get; set; }

        public Guid? SubConstructionId { get; set; }

        public String Name { get; set; }

        public double? Coefficient { get; set; }

        public double? Area { get; set; }

        public string? Unit { get; set; }

        public DateTime? InsDate { get; set; }

    }
    public class PackageHouseResponse
    {
        public PackageHouseResponse(Guid id, Guid packageId, string? imgUrl, DateTime? insDate)
        {
            Id = id;
            PackageId = packageId;
            ImgUrl = imgUrl;
            InsDate = insDate;
        }
        public Guid Id { get; set; }

        public Guid PackageId { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

    }
}
