using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Construction
{
    public class ListConstructionWorkResponse
    {
        public ListConstructionWorkResponse(Guid id, string? workName,
            Guid? constructionId, DateTime? insDate, string? unit, string? code)
        {
            Id = id;
            WorkName = workName;
            ConstructionId = constructionId;
            InsDate = insDate;
            Unit = unit;
            Code = code;
        }
        public Guid Id { get; set; }

        public string? WorkName { get; set; }

        public Guid? ConstructionId { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Unit { get; set; }

        public string? Code { get; set; }
    }
}
