using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RHCQS_BusinessObject.Payload.Response.Project
{
    public class ProjectHaveDrawingRequest
    {
        public Guid AccountId { get; set; }
        public string Address { get; set; }
        public double Area { get; set; }
        public List<IFormFile> PerspectiveImage {  get; set; }
        public List<IFormFile> ArchitectureImage { get; set; }
        public List<IFormFile> StructureImage { get; set; }
        public List<IFormFile> ElectricityWaterImage { get; set; }
    }
}
