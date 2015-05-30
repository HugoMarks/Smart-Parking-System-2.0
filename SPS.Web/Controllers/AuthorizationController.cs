using SPS.BO;
using SPS.Model;
using SPS.Web.Api;
using SPS.Web.Models;
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
        [HttpPost]
        public HttpResponseMessage Post([FromBody] AuthorizationModel model)
        {
            Tag tag = BusinessManager.Instance.Tags.Find(model.TagId);

            if (tag == null)
            {
                return MakeErrorResponse();
            }

            Parking parking = BusinessManager.Instance.Parkings.Find(model.ParkingCNPJ);

            if (parking == null)
            {
                return MakeErrorResponse();
            }

            if (!tag.Client.Parkings.Any(p => p.CNPJ == model.ParkingCNPJ))
            {
                return MakeErrorResponse();
            }

            UsageRecord lastRecord = BusinessManager.Instance.UsageRecords.FindAll()
                                        .Where(r => r.EnterDateTime.Date == DateTime.Now.Date && r.IsDirty)
                                        .OrderBy(r => r.EnterDateTime)
                                        .LastOrDefault();

            if (lastRecord == null)
            {
                lastRecord = new UsageRecord()
                {
                    Client = tag.Client,
                    EnterDateTime = DateTime.Now,
                    ExitDateTime = DateTime.Now,
                    IsDirty = true,
                    Parking = parking,
                    Tag = tag
                };

                BusinessManager.Instance.UsageRecords.Add(lastRecord);

                return MakeSuccessResponse(tag.Client.FirstName, (byte)0);
            }
            else
            {
                lastRecord.IsDirty = false;
                lastRecord.ExitDateTime = DateTime.Now;

                BusinessManager.Instance.UsageRecords.Update(lastRecord);

                return MakeSuccessResponse(tag.Client.FirstName, (byte)1);
            }
        }

        private HttpResponseMessage MakeSuccessResponse(string userName, byte control)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {
                    Control = control,
                    UserName = userName
                })
            };
        }

        private HttpResponseMessage MakeErrorResponse()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new JsonContent(new 
                {
                    Message = "Unauthorized"
                })
            };
        }
    }
}
