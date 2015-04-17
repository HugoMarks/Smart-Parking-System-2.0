using System;
using System.Configuration;

namespace SPS.Web.Common
{
    /// <summary>
    /// Provides access to some important application configuration entries.
    /// </summary>
    public static class ApplicationConfig
    {
        /// <summary>
        /// Gets the default email for the SPS system.
        /// </summary>
        public static string SPSMail 
        {
            get
            {
                return ConfigurationManager.AppSettings["MailAccount"];
            }
        }

        /// <summary>
        /// Gets the passowrd for the default SPS email.
        /// </summary>
        public static string SPSMailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPassword"];
            }
        }

        /// <summary>
        /// Gets the host name for the default email server used to send emails.
        /// </summary>
        public static string MailSMTPServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["MailSMTPServerAddress"];
            }
        }
    }
}