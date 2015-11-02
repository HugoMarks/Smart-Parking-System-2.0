using SPS.Raspberry.DataObject;
using System;

namespace SPS.Raspberry.Logic
{
    public class AuthResponseEventArgs : EventArgs
    {
        public AuthResponse Reponse { get; set; }

        public AuthResponseEventArgs()
        {

        }

        public AuthResponseEventArgs(AuthResponse response)
        {
            Reponse = response;
        }
    }
}
