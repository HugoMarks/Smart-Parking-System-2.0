using SPS.BO;
using SPS.Model;
using SPS.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPS.Web.Controllers
{
    [Route("api/authorize")]
    public class AuthorizationController : ApiController
    {
        public HttpResponseMessage Get(string tagId, string parkingCNPJ)
        {
            Tag tag = BusinessManager.Instance.Tags.Find(tagId);

            if (tag == null)
            {
                return MakeErrorResponse(AuthorizationStatusCode.InvalidTag);
            }

            Parking parking = BusinessManager.Instance.Parkings.Find(parkingCNPJ);

            if (parking == null)
            {
                return MakeErrorResponse(AuthorizationStatusCode.InvalidParking);
            }

            if (!tag.Client.Parkings.Any(p => p.CNPJ == parkingCNPJ))
            {
                return MakeErrorResponse(AuthorizationStatusCode.InvalidTagParking);
            }

            //List<UsageRecord> records = BusinessManager.Instance.UsageRecords.FindAll()
            //                            .Where(r => r.EnterDateTime.Date == DateTime.Now.Date)
            //                            .ToList();
            
            //UsageRecord record = new UsageRecord()
            //{
            //    Client = tag.Client,
            //    EnterDateTime = DateTime.Now,
            //    Parking = parking,
            //    Tag = tag
            //};

            //BusinessManager.Instance.UsageRecords.Add(record);

            return MakeSuccessResponse(AuthorizationStatusCode.Ok, "Teste", (byte)1);
        }

        private HttpResponseMessage MakeSuccessResponse(AuthorizationStatusCode code, string userName, byte control)
        {
            return new HttpResponseMessage()
            {
                Content = new JsonContent(new
                {
                    StatusCode = (byte)code,
                    Control = control,
                    UserName = userName
                })
            };
        }

        private HttpResponseMessage MakeErrorResponse(AuthorizationStatusCode code)
        {
            return new HttpResponseMessage()
            {
                Content = new JsonContent(new 
                {
                    StatusCode = (byte)code
                })
            };
        }
    }
}
