using SPS.Raspberry.Helpers;
using Windows.Web.Http;

namespace SPS.Raspberry.DataObject
{
    /// <summary>
    /// Represents the response for and <see cref="AuthRequest"/>.
    /// </summary>
    public class AuthResponse : IResponse
    {
        /// <summary>
        /// Gets or sets the response HTTP code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the response control byte.
        /// </summary>
        public byte Control { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthResponse"/> class.
        /// </summary>
        public AuthResponse()
        {

        }

        /// <summary>
        /// Deserializes the specified json string into an <see cref="AuthRequest"/> object.
        /// </summary>
        /// <param name="json">The json string to deserialize.</param>
        public void Deserialize(string json)
        {
            AuthResponse response = JsonHelper.Deserialize<AuthResponse>(json);

            UserName = response.UserName;
            Message = response.Message;
            Control = response.Control;
        }
    }
}
