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

		public PostalServiceResult GetAdrressFromPostalCode(string postalCode)
		{
			try
			{
				var result = this.client.consultaCEP(postalCode);

				return new PostalServiceResult
				{
					Address = new Address
					{
						City = result.cidade,
						State = result.uf,
						Street = result.end,
						ZipCode = result.cep
					},
					Message = null
				};
			}
			catch
			{
				return new PostalServiceResult
				{
					Address = null,
					Message = "CEP não encontrado"
				};
			}
		}
	}

	public class PostalServiceResult
	{
		public Address Address { get; set; }

		public string Message { get; set; }
	}
}