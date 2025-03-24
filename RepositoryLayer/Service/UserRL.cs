﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Helper;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingDbContext _context;
        private readonly Password_Hash _passwordHash;
        private readonly JwtHelper _jwtHelper;

        public UserRL(GreetingDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _passwordHash = new Password_Hash();
            _jwtHelper = jwtHelper;
        }

        public UserDTO Register(RegisterDTO registerDTO)
        {
            string hashedPassword = _passwordHash.HashPassword(registerDTO.Password);

            var userEntity = new UserEntity
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();

            return new UserDTO
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
            };
        }

        public string Login(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDTO.Email);
            if (user == null || !_passwordHash.VerifyPassword(loginDTO.Password, user.PasswordHash))
            {
                return null;
            }

            string token = _jwtHelper.GenerateToken(user.Email);

            return token;
        }

        public bool ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user != null;
        }

        public bool ResetPassword(string email, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = _passwordHash.HashPassword(newPassword);
            _context.SaveChanges();
            return true;
        }
    }
}
