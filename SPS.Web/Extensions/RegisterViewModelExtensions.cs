﻿using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System.Linq;

namespace SPS.Web.Extensions
{
    public static class RegisterViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this RegisterViewModel registerModel, UserType userType)
        {
            return new ApplicationUser
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.Email,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber,
                UserType = userType
            };
        }

        /// <summary>
        /// Converts an <see cref="RegisterViewModel"/> in a <see cref="MonthlyClient"/>.
        /// </summary>
        /// <param name="registerModel">The RegisterViewModel to be converted.</param>
        /// <param name="passwordHash">The hashed password to be saved in the database.</param>
        /// <returns>An instance of the <see cref="MonthlyClient"/> class.</returns>
        public static T ToUser<T>(this RegisterViewModel registerModel, string passwordHash) where T : User
        {
            var userType = typeof(T);

            if (userType == typeof(MonthlyClient))
            {
                return ToMonthlyClient(registerModel, passwordHash) as T;
            }
            else if (userType == typeof(LocalManager))
            {
                return ToLocalManager(registerModel, passwordHash) as T;
            }

            return null;
        }

        /// <summary>
        /// Converts an <see cref="RegisterViewModel"/> in a <see cref="MonthlyClient"/>.
        /// </summary>
        /// <param name="registerModel">The RegisterViewModel to be converted.</param>
        /// <param name="passwordHash">The hashed password to be saved in the database.</param>
        /// <returns>An instance of the <see cref="MonthlyClient"/> class.</returns>
        private static MonthlyClient ToMonthlyClient(this RegisterViewModel registerModel, string passwordHash)
        {
            return new MonthlyClient
            {
                Address = GetAddress(registerModel),
                CPF = registerModel.CPF,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Password = passwordHash,
                RG = registerModel.RG,
                Telephone = registerModel.PhoneNumber
            };
        }

        /// <summary>
        /// Converts an <see cref="RegisterViewModel"/> in a <see cref="LocalManager"/>.
        /// </summary>
        /// <param name="registerModel">The RegisterViewModel to be converted.</param>
        /// <param name="passwordHash">The hashed password to be saved in the database.</param>
        /// <returns>An instance of the <see cref="LocalManager"/> class.</returns>
        private static LocalManager ToLocalManager(this RegisterViewModel registerModel, string passwordHash)
        {
            return new LocalManager
            {
                Address = GetAddress(registerModel),
                CPF = registerModel.CPF,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Password = passwordHash,
                RG = registerModel.RG,
                Telephone = registerModel.PhoneNumber
            };
        }

        private static Address GetAddress(RegisterViewModel registerModel)
        {
            return BusinessManager.Instance.Addresses.FindAll().
                          Where(a => a.PostalCode == registerModel.PostalCode)
                          .FirstOrDefault() ??
                          new Address
                          {
                              City = registerModel.City,
                              Number = uint.Parse(registerModel.Number),
                              PostalCode = registerModel.PostalCode,
                              Square = registerModel.Square,
                              State = registerModel.State,
                              Street = registerModel.Street
                          };

        }
    }
}