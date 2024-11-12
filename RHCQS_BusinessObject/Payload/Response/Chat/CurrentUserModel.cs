using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.CurrentUserModel
{
    public class CurrentUserModel
    {
        public Guid Userid {  get; set; }
        public string? Username { get; set; }
        public string? UserProfileImage { get; set; }
        public string? Email { get; set; }
        public string?Phonenumber { get; set; } 
        public bool IsBan { get; set; }
        
    }
}
