// ============================================================
//  E-Wallet Financial System
//  File: SecurityHelper.cs
//  Purpose: Password hashing and PIN utilities
// ============================================================
using System;
using System.Security.Cryptography;
using System.Text;

namespace E_Wallet
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Computes a SHA-256 hex string of the input.
        /// In a production system, use BCrypt or PBKDF2 with salt.
        /// </summary>
        public static string HashPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hash  = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

        /// <summary>Validates that a PIN is exactly 4 digits.</summary>
        public static bool IsValidPIN(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin) || pin.Length != 4)
                return false;
            foreach (char c in pin)
                if (!char.IsDigit(c)) return false;
            return true;
        }

        /// <summary>Validates password meets basic complexity rules.</summary>
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
        }

        /// <summary>Masks a card number showing only last 4 digits.</summary>
        public static string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return "****";
            return new string('*', cardNumber.Length - 4)
                   + cardNumber.Substring(cardNumber.Length - 4);
        }
    }
}
