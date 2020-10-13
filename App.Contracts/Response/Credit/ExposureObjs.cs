using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class ExposureObjs
    {
        public class ExposureObj : GeneralEntity
        {
            public int ExposureParameterId { get; set; }
            public int? CustomerTypeId { get; set; }
            public string Description { get; set; }
            public string CustomerTypeName { get; set; }
            public decimal? Percentage { get; set; }
            public decimal? ShareHolderAmount { get; set; }
            public decimal? Amount { get; set; }
        }
        public class AddUpdateExposureObj
        {
            public int ExposureParameterId { get; set; }
            public int? CustomerTypeId { get; set; }
            public string Description { get; set; }
            public decimal? Percentage { get; set; }
            public decimal? ShareHolderAmount { get; set; }
            public decimal? Amount { get; set; }
        }

        public class ExposureRegRespObj
        {
            public int ExposureParameterId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ExposureRespObj
        {
            public IEnumerable<ExposureObj> Exposure { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ExposureSearchObj
        {
            public int ExposureParameterId { get; set; }
            public string SearchWord { get; set; }
        }
}
}
