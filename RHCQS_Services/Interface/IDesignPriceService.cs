using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IDesignPriceService
    {
        Task<IPaginate<DesignPriceResponse>> GetListDesignPrice(int page, int size);
        Task<DesignPriceResponse> GetDetailDesignPrice(Guid id);
        Task<bool> CreateDesignPrice(DesignPriceRequest request);
        Task<bool> UpdateDesignPrice(Guid id, DesignPriceRequest request);
    }
}
