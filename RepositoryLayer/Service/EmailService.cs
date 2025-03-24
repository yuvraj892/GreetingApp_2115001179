﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RepositoryLayer.Helper;

namespace RepositoryLayer.Service
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public bool SendPasswordResetEmail(string email, string token, string baseUrl)
        {
            try
            {
                if (_smtpSettings == null)
                {
                    Console.WriteLine("SMTP settings are not initialized.");
                    return false;
                }
                if (string.IsNullOrEmpty(_smtpSettings.Server) ||
                    string.IsNullOrEmpty(_smtpSettings.SenderEmail) ||
                    string.IsNullOrEmpty(_smtpSettings.Username) ||
                    string.IsNullOrEmpty(_smtpSettings.Password))
                {
                    Console.WriteLine("One or more SMTP settings are missing.");
                    return false;
                }

                var resetLink = $"{baseUrl}/api/user/reset-password?token={token}";
                Console.WriteLine($"Generated Reset Link: {resetLink}");

                var mail = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = "Password Reset Request",
                    Body = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>Hello,</p>
                    <p>Click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>This link will expire in 1 hour.</p>
                </body>
                </html>",
                    IsBodyHtml = true
                };

                mail.To.Add(email);

                using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.Send(mail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
