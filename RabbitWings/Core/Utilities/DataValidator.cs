using System;
using System.Text.RegularExpressions;

namespace RabbitWings.Core.Utilities
{
    /// <summary>
    /// Provides methods to validate common data formats.
    /// </summary>
    public static class DataValidator
    {
        public static bool IsValidEmail(string email) =>
            Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

        public static bool IsValidPhoneNumber(string phone) =>
            Regex.IsMatch(phone, @"^\+?[1-9]\d{1,14}$");

        public static bool IsValidPassword(string password) =>
            Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");

        public static void ValidateLoginData(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");
            if (!IsValidPassword(password))
                throw new ArgumentException("Password does not meet the security criteria.");
        }

        public static void ValidateRegisterData(string email, string phone, string password)
        {
            ValidateLoginData(email, password);
            if (!IsValidPhoneNumber(phone))
                throw new ArgumentException("Invalid phone number format.");
        }
    }
}
