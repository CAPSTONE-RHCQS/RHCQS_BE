using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Construction
{
    public class AutoConstructionResponse
    {
        public AutoConstructionResponse(Guid id, Guid? subConstructionId, string? name, double? coefficient, string type)
        {
            Id = id;
            SubConstructionId = subConstructionId;
            Name = name;
            Coefficient = coefficient;
            Type = type;
        }
        public Guid Id { get; set; }

        public Guid? SubConstructionId { get; set; }

        public string? Name { get; set; }

        public double? Coefficient { get; set; }
        public string Type { get; set; }
    }

    public class AutoSubConstructionResponse
    {
        public AutoSubConstructionResponse(Guid id, Guid constructionId, string? name, double? coefficient)
        {
            Id = id;
            ConstructionId = constructionId;
            Name = name;
            Coefficient = coefficient;
        }
        public Guid Id { get; set; }
        public Guid ConstructionId { get; set; }

        public string? Name { get; set; }

        public double? Coefficient { get; set; }
    }
}
