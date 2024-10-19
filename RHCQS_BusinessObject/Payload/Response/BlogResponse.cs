using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class BlogResponse
    {
        public BlogResponse(Guid id, string? accountName, string? roleName, string? heading, string? subHeading,
            string? context, string? imgUrl, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            AccountName = accountName;
            RoleName = roleName;
            Heading = heading;
            SubHeading = subHeading;
            Context = context;
            ImgUrl = imgUrl;
            InsDate = insDate;
            UpsDate = upsDate;
        }

        public Guid Id { get; set; }

        public string? AccountName { get; set; }

        public string? RoleName { get; set; }

        public string? Heading { get; set; }

        public string? SubHeading { get; set; }

        public string? Context { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
    }
}
