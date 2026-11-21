using System;
using System.Net.Mail;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.DAO;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class AuthController
    {
        private readonly AuthDAO authDAO = new AuthDAO();

        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public bool Register(
            string fullName,
            string username,
            string email,
            string password,
            string confirmPassword,
            out string message)
        {
            message = "";

            fullName = (fullName ?? "").Trim();
            username = (username ?? "").Trim();
            email = (email ?? "").Trim();

            if (fullName == "" || username == "" || email == "" ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                message = "Please fill in all fields.";
                return false;
            }

            if (username.Length > 50 || fullName.Length > 150 ||
                email.Length > 150)
            {
                message = "One or more fields exceed the allowed length.";
                return false;
            }

            try
            {
                MailAddress parsedEmail = new MailAddress(email);

                if (parsedEmail.Address != email)
                {
                    message = "Enter a valid email address.";
                    return false;
                }
            }
            catch (FormatException)
            {
                message = "Enter a valid email address.";
                return false;
            }

            if (password.Length < 8)
            {
                message = "Password must contain at least 8 characters.";
                return false;
            }

            if (password != confirmPassword)
            {
                message = "Passwords do not match.";
                return false;
            }

            AuthUser user = new AuthUser
            {
                FullName = fullName,
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = "Staff"
            };

            try
            {
                bool registered = authDAO.RegisterUser(user);

                if (registered)
                {
                    message = "Account created successfully. Please log in.";
                    return true;
                }

                message = "Account could not be created.";
                return false;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    message = "That username or email is already registered.";
                else
                    message = "Database error. Check your database connection.";

                return false;
            }
            catch (Exception)
            {
                message = "An unexpected error occurred while registering.";
                return false;
            }
        }

        public AuthUser Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
                return null;

            try
            {
                AuthUser user = authDAO.GetUserByUsername(username.Trim());

                if (user == null)
                    return null;

                if (!VerifyPassword(password, user.PasswordHash))
                    return null;

                // Do not expose the stored password hash to the form.
                user.PasswordHash = null;

                return user;
            }
            catch (Exception)
            {
                // A database failure must never grant access.
                return null;
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];

            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (Rfc2898DeriveBytes derive =
                new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = derive.GetBytes(HashSize);

                return Iterations + ":" +
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string storedValue)
        {
            try
            {
                string[] parts = storedValue.Split(':');

                if (parts.Length != 3)
                    return false;

                int iterations = int.Parse(parts[0]);

                // Prevent invalid or unreasonable iteration counts.
                if (iterations < 100000 || iterations > 1000000)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                if (salt.Length != SaltSize ||
                    expectedHash.Length != HashSize)
                    return false;

                using (Rfc2898DeriveBytes derive =
                    new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    byte[] actualHash = derive.GetBytes(HashSize);
                    int difference = 0;

                    for (int i = 0; i < expectedHash.Length; i++)
                        difference |= expectedHash[i] ^ actualHash[i];

                    return difference == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}