using SPS.Model;
using SPS.Web.CorreiosService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SPS.Web.Services
{
	public class PostalCodeService
	{
		private AtendeClienteClient client;

		public PostalCodeService()
		{
			this.client = new AtendeClienteClient("AtendeClientePort");
        }

		public Address GetAdrressFromPostalCodeAsync(string postalCode)
		{
			var result = this.client.consultaCEP(postalCode);

			return new Address
			{
				City = result.cidade,
				State = result.uf,
				Street = result.end,
				ZipCode = result.cep
			};
		}
	}
}