﻿using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IUtilitiesService
    {
        Task<IPaginate<UtilityResponse>> GetListUtilities(int page, int size);
    }
}
