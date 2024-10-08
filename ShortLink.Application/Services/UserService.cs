﻿using System.Text;
using System;
using System.Security.Cryptography;
using ShortLink.Application.Interfaces;
using ShortLink.Domain;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Application.Services
{
    public class UserService(IUserRepository userRepository):IUserService
    {

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            User user = await userRepository.GetUserAsync(email);
            if (user is not null && !VerifyPassword(password, user.Password))
                return null;
            return user;
        }
        public async Task<bool> InsertUserAsync(string email,string password)
        {
            if (await UserExists(email))
                return false;

            User user = new()
            {
                Email=email,
                Password=HashPassword(password)
            };
           await userRepository.InsertUserAsync(user);
            //confirmar vir email a inscrição
            //redirecionar a tela de login
            return true;

        }
        private async Task<bool> UserExists(string email) => await userRepository.GetUserAsync(email) is not null;
        #region Password Verifications
        private static bool VerifyPassword(string password, string storedPassword) => HashPassword(password) == storedPassword;
        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        #endregion
    }

}
