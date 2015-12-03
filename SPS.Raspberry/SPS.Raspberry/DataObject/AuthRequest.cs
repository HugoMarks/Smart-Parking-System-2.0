using SPS.Raspberry.Helpers;

namespace SPS.Raspberry.DataObject
{
    public class AuthRequest : IRequest
    {
        public string TagId { get; set; }

        public string ParkingCNPJ { get; set; }

        public string CarPlate { get; set; }

        public string Serialize()
        {
            return JsonHelper.Serialize(this);
        }
    }
}
