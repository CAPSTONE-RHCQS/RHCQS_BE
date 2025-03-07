﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.HouseDesign
{
    public class HouseDesignVersionItemResponse
    {
        public HouseDesignVersionItemResponse(Guid id,
            string? name,
            double version,
            string fileUrl,
            DateTime? insDate,
            string? note,
            string? reason,
            Guid? relatedDrawingId,
            Guid? previousDrawingId)
        {
            Id = id;
            Name = name;
            Version = version;
            FileUrl = fileUrl;
            InsDate = insDate;
            Note = note;
            Reason = reason;
            RelatedDrawingId = relatedDrawingId;
            PreviousDrawingId = previousDrawingId;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double Version { get; set; }
        public string FileUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Note { get; set; }
        public string? Reason { get; set; }

        public Guid? RelatedDrawingId { get; set; }

        public Guid? PreviousDrawingId { get; set; }
    }
}
