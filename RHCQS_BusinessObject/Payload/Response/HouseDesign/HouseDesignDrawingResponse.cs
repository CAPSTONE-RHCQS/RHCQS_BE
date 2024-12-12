using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Response.HouseDesign
{
    public class HouseDesignDrawingResponse
    {
        public HouseDesignDrawingResponse(
            string projectType,
            Guid initialQuotationId,
            Guid id, Guid projectId,
            string staffName, double? versionPresent,
            string? name, int? step,
            string? status, string? type,
            bool? isCompany, DateTime? insDate,
            List<DependOnVersion> dependOnVersion,
            List<HouseDesignVersionResponse>? versions)
        {
            ProjectType = projectType;
            InitialQuotationId = initialQuotationId;
            Id = id;
            ProjectId = projectId;
            StaffName = staffName;
            VersionPresent = versionPresent;
            Name = name;
            Step = step;
            Status = status;
            Type = type;
            IsCompany = isCompany;
            InsDate = insDate;
            DependOnVersion = dependOnVersion;
            Versions = versions;
        }
        public string ProjectType { get; set; }
        public Guid InitialQuotationId {  get; set; }
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string StaffName { get; set; }
        public double? VersionPresent { get; set; }

        public string? Name { get; set; }

        public int? Step { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public bool? IsCompany { get; set; }

        public DateTime? InsDate { get; set; }
        public List<DependOnVersion>? DependOnVersion { get; set; }
        public List<HouseDesignVersionResponse>? Versions { get; set; }
    }

    public class HouseDesignVersionResponse
    {
        public HouseDesignVersionResponse() { }
        public HouseDesignVersionResponse(Guid id, string? name, double? version, string? fileUrl,
            DateTime? insDate, string? note, string? namePrevious)
        {
            Id = id;
            Name = name;
            Version = version;
            FileUrl = fileUrl;
            InsDate = insDate;
            Note = note;
            NamePrevious = namePrevious;
        }

        public HouseDesignVersionResponse(Guid id, string? name, double? version, string? fileUrl,
            DateTime? insDate, Guid? previousDrawingId, string? note, string? reason)
        {
            Id = id;
            Name = name;
            Version = version;
            FileUrl = fileUrl;
            InsDate = insDate;
            PreviousDrawingId = previousDrawingId;
            Note = note;
            Reason = reason;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double? Version { get; set; }
        public string? FileUrl { get; set; }
        public DateTime? InsDate { get; set; }
        public Guid? PreviousDrawingId { get; set; }
        public string? NamePrevious { get; set; }

        public string? Note { get; set; }
        public string? Reason { get; set; }
    }

    public class DependOnVersion
    {
        public Guid HouseDesginVersionId { get; set; }
        public string HouseDesignVersionName { get; set; }
        public double HouseDesignVersion { get; set; }
        public string FileDesignVersion { get; set; }
    }
}
