using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class RoleResponse
    {
        public RoleResponse() { }
        public RoleResponse(Guid id, string? roleName)
        {
            Id = id;
            RoleName = roleName;
        }

        public Guid Id { get; set; }

        public string? RoleName { get; set; }
    }
}
