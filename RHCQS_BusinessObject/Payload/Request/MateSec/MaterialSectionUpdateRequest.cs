using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.MateSec
{
    public class MaterialSectionUpdateRequest
    {
        public string? Name { get; set; }
        [MaxLength(50, ErrorMessage = "Mã Code không được vượt quá 10 ký tự.")]
        public string? Code { get; set; }
    }
}
