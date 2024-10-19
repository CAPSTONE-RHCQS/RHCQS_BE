using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class BlogRequest
    {
        [JsonIgnore]
        public Guid AccountId { get; set; }

        public string? Heading { get; set; }

        public string? SubHeading { get; set; }

        public string? Context { get; set; }

        public string? ImgUrl { get; set; }

    }

}
