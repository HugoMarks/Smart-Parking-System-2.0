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
        /// Gets the default email address.
        /// </summary>
        public static string Mail 
        {
            get
            {
                return ConfigurationManager.AppSettings["MailAccount"];
            }
        }

        /// <summary>
        /// Gets the passowrd for the default SPS email.
        /// </summary>
        public static string MailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPassword"];
            }
        }

        /// <summary>
        /// Gets the host name for the default email server used to send emails.
        /// </summary>
        public static string MailServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["MailSMTPServerAddress"];
            }
        }

        /// <summary>
        /// Gets the default SMS service login.
        /// </summary>
        public static string SMSLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSLogin"];
            }
        }

        /// <summary>
        /// Gets the passowrd for the default SMS service.
        /// </summary>
        public static string SMSPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSPassword"];
            }
        }

        /// <summary>
        /// Gets the SMS domain used to send SMS messages.
        /// </summary>
        public static string SMSDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["SMSDomain"];
            }
        }

        /// <summary>
        /// Gets the host name for the default SMS server used to send SMS messages.
        /// </summary>
        public static string SMSServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SmsSMTPServerAddress"];
            }
        }
    }
}