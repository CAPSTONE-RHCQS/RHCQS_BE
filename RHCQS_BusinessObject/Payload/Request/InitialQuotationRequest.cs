﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    using System.ComponentModel.DataAnnotations;

    public class InitialQuotationRequest
    {
        [Required(ErrorMessage = "AccountId là bắt buộc.")]
        public Guid AccountId { get; set; }

        public Guid? PromotionId { get; set; }

        [Required(ErrorMessage = "Phải có ít nhất một mục báo giá ban đầu.")]
        public List<InitialQuotationItemRequest>? InitialQuotationItemRequests { get; set; }
    }

    public class InitialQuotationItemRequest
    {

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc.")]
        public Guid ConstructionItemId { get; set; }

        public Guid? SubConstructionId { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0.")]
        public double? Area { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public double? Price { get; set; }
    }

}
