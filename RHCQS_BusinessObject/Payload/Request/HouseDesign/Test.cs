using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.HouseDesign
{
    public class HouseTemplateResponseTest
    {
        public HouseTemplateResponseTest() { }

        public HouseTemplateResponseTest(Guid id, string name, string? description, int? numberOfFloor, int? numberOfBed,
            string? imgUrl, DateTime? insDate,
            Guid packageRoughId,
            double packageRoughPrice,
            string packageRoughName
            //List<SubTemplatesResponseTest> subTemplates,
            //List<PackageHouseResponse> packageHouses, List<MediaResponse> exteriorsUrls
            )
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfFloor = numberOfFloor;
            NumberOfBed = numberOfBed;
            ImgUrl = imgUrl;
            InsDate = insDate;
            PackageRoughId = packageRoughId;
            PackageRoughPrice = packageRoughPrice;
            PackageRoughName = packageRoughName;
            //SubTemplates = subTemplates;
            //PackageHouses = packageHouses;
            //ExteriorsUrls = exteriorsUrls;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }
        public Guid PackageRoughId { get; set; }
        public double PackageRoughPrice { get; set; }
        public string PackageRoughName { get; set; }

        //public List<SubTemplatesResponseTest> SubTemplates { get; set; }

        //public List<PackageHouseResponse> PackageHouses { get; set; }

        //public List<MediaResponse> ExteriorsUrls { get; set; }

    }

    public class SubTemplatesResponseTest
    {
        public SubTemplatesResponseTest(Guid id, double buildingArea, double? floorArea, DateTime? insDate, string? size,
            string? url, double totalRough
            //List<TemplateItemReponseTest> templateItems,
            //List<MediaResponse> designdrawings
            )
        {
            Id = id;
            BuildingArea = buildingArea;
            FloorArea = floorArea;
            InsDate = insDate;
            Size = size;
            Url = url;
            TotalRough = totalRough;
            //TemplateItems = templateItems;
            //Designdrawings = designdrawings;
        }

        public Guid Id { get; set; }

        public double BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Size { get; set; }

        public string? Url { get; set; }
        public double TotalRough { get; set; }

        //public List<TemplateItemReponseTest>? TemplateItems { get; set; }

        //public List<MediaResponse>? Designdrawings { get; set; }
    }

    public class MediaResponseTest
    {
        public MediaResponseTest(Guid id, string? name, string? url, DateTime? insDate, DateTime? upsDate)
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
    public class TemplateItemReponseTest
    {
        public TemplateItemReponseTest(Guid id, string? name, Guid contructionid, Guid? subcontructionid,
            double? coefficient, double? area, string? unit, DateTime? insDate, double price)
        {
            Id = id;
            Name = name;
            ConstructionId = contructionid;
            SubConstructionId = subcontructionid;
            Coefficient = coefficient;
            Area = area;
            Unit = unit;
            InsDate = insDate;
            Price = price;
        }
        public Guid Id { get; set; }

        public Guid ConstructionId { get; set; }

        public Guid? SubConstructionId { get; set; }

        public String Name { get; set; }

        public double? Coefficient { get; set; }

        public double? Area { get; set; }

        public string? Unit { get; set; }

        public DateTime? InsDate { get; set; }
        public double Price { get; set; }

    }
}
