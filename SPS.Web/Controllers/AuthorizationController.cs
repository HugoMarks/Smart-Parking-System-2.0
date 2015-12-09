using SPS.BO;
using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Web.Api;
using SPS.Web.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPS.Web.Controllers
{
    [Route("api/authorize")]
    public class AuthorizationController : ApiController
    {
        private const string SuccessMessage = "Ok";

        private const string UnauthorizedMessage = "Unauthorized";

        private const string FullParkingMessage = "Full";

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AuthorizationModel model)
        {
            bool controler = false; // true - usado para placa , false - usado pra tag

            Plate plate = BusinessManager.Instance.Plates.Find(model.CarPlate);

            Tag tag = BusinessManager.Instance.Tags.Find(model.TagId);



            if (plate == null && tag == null)
            {         
                return MakeResponse(UnauthorizedMessage, string.Empty, 0, HttpStatusCode.Unauthorized);
            }

            if (plate != null)
            {
                controler = true;
            }
                


            Parking parking = BusinessManager.Instance.Parkings.Find(model.ParkingCNPJ);

            if (parking == null)
            {
                return MakeResponse(UnauthorizedMessage, string.Empty, 0, HttpStatusCode.Unauthorized);
            }


            if (controler)
            { // is plate
                if (!plate.Client.Parkings.Any(p => p.CNPJ == model.ParkingCNPJ))
                {
                    return MakeResponse(UnauthorizedMessage, string.Empty, 0, HttpStatusCode.Unauthorized);
                }
            }
            else
            { //is tag
                if (!tag.Client.Parkings.Any(p => p.CNPJ == model.ParkingCNPJ))
                {
                    return MakeResponse(UnauthorizedMessage, string.Empty, 0, HttpStatusCode.Unauthorized);
                }
            }
           

            bool isNew;
            byte control;

            try
            {
                if (controler)
                {
                    BusinessManager.Instance.AddOrUpdateRecord(plate, parking, out isNew);
                }
                else
                {
                    BusinessManager.Instance.AddOrUpdateRecord(tag, parking, out isNew);
                }
                
            }
            catch (FullParkingException)
            {
                return MakeResponse(FullParkingMessage, string.Empty, 0, HttpStatusCode.BadRequest);
            }

            control = Convert.ToByte(isNew);

            if (controler)
            {
                return MakeResponse(SuccessMessage, plate.Client.FirstName, control, HttpStatusCode.OK);
            }
            else
            {
                return MakeResponse(SuccessMessage, tag.Client.FirstName, control, HttpStatusCode.OK);
            }

           
        }

        private HttpResponseMessage MakeResponse(string message, string userName, byte control, HttpStatusCode code)
        {
            return new HttpResponseMessage()
            {
                StatusCode = code,
                Content = new JsonContent(new
                {
                    Message = message,
                    Control = control,
                    UserName = userName
                })
            };
        }
    }
}
