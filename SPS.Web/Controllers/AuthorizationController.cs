using SPS.BO;
using SPS.BO.Exceptions;
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
        public const string UnauthorizedMessage = "Unauthorized";
        public const string FullParkingMessage = "Full";

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AuthorizationModel model)
        {
            Tag tag = BusinessManager.Instance.Tags.Find(model.TagId);

            if (tag == null)
            {
                return MakeErrorResponse(HttpStatusCode.Unauthorized, UnauthorizedMessage);
            }

            Parking parking = BusinessManager.Instance.Parkings.Find(model.ParkingCNPJ);

            if (parking == null)
            {
                return MakeErrorResponse(HttpStatusCode.Unauthorized, UnauthorizedMessage);
            }

            if (!tag.Client.Parkings.Any(p => p.CNPJ == model.ParkingCNPJ))
            {
                return MakeErrorResponse(HttpStatusCode.Unauthorized, UnauthorizedMessage);
            }

            bool isNew;
            byte control;

            try
            {
                BusinessManager.Instance.AddOrUpdateRecord(tag, parking, out isNew);
            }
            catch (FullParkingException)
            {
                return MakeErrorResponse(HttpStatusCode.BadRequest, FullParkingMessage);
            }

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

        private HttpResponseMessage MakeErrorResponse(HttpStatusCode statusCode, string message)
        {
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new JsonContent(new 
                {
                    Message = message
                })
            };
        }
    }
}
