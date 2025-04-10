﻿using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
        }

        // Register a new user
        public UserDTO Register(RegisterDTO registerDTO)
        {
            try
            {
                var userDTO = _userRL.Register(registerDTO);

                if (userDTO == null)
                {
                    throw new Exception("Registration failed.");
                }

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in registration process: " + ex.Message);
            }
        }

        public string Login(LoginDTO loginDTO)
        {
            try
            {
                var token = _userRL.Login(loginDTO);

                if (token == null)
                {
                    throw new Exception("Invalid email or password.");
                }

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in login process: " + ex.Message);
            }
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                return _userRL.ForgotPassword(email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in forgot password process: " + ex.Message);
            }
        }

        public bool ResetPassword(string token, string newPassword)
        {
            try
            {
                return _userRL.ResetPassword(token, newPassword);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in resetting password: " + ex.Message);
            }
        }
    }
}
