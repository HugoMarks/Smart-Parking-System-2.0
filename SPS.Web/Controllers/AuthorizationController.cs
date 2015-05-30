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

            bool isNew;
            byte control;

            BusinessManager.Instance.AddOrUpdateRecord(tag, parking, out isNew);
            control = Convert.ToByte(isNew);

            return MakeSuccessResponse(tag.Client.FirstName, control);
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
