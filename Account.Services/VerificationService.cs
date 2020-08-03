using Account.Services.Interfaces;
using Account.Services.Models;
//using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Account.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verificationRepository;
        public VerificationService(
            //IConfigurationRoot  configurationRoot,
            IVerificationRepository verificationRepository)
        {
            _verificationRepository = verificationRepository;
        }
        public async Task SaveVerificationcode(EmailVerification emailVerification)
        {
            await _verificationRepository.CreateEmailVerificationAsync(emailVerification);
        }
        private async Task SendEmailWithVerificationCodeAsync(EmailVerification emailVerification)
        {

            string subject = "Verification Code - Brix ";
            //string subject = ConfigurationManager.AppSettings["VerificationEmailSubject"]; 
            string body = $"Hello {emailVerification.Email}"+
               $" your verify number is {emailVerification.VerificationCode}";
            // body += @"Hello " + email + "</br>  your verify number is " + emailVerification.VerificationCode + "</br><a href='http://localhost:4200/verification'>our site</a>";
            //string htmlText = @"
            //        <head> 
            //            <style> 
            //                body{background-color:cadetblue;direction:rtl;text-align:center;}
            //                h1,h3,p{font-size:20px; text-align:center;color:blue;}
            //            </style>
            //        </head>
            //        <body>";
            //htmlText += "<h1> hello " + emailVerification.Email + "  </h1>" +
            //    "<p>" + " your verify number is " + emailVerification.VerificationCode + " </p>" +
            //    "</body>";

            await SendEmail(emailVerification.Email, subject, body);
        }
        private Task SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromMail = "brixbootcamp@gmail.com";
                //string fromMail = ConfigurationManager.AppSettings["BrixEmailAddress"];
                string fromPassword = "brix2020";
                //string fromPassword = ConfigurationManager.AppSettings["BrixEmailPassword"];
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(fromMail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                //SmtpServer.Port = ConfigurationManager.AppSettings["PortNumber"];
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromMail, fromPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }
        private int GenerateRandomNomber(int min, int max)
        {
            Random _rdm = new Random();
            return _rdm.Next(min, max);
        }
        public async Task<bool> VerifyEmail(EmailVerification verification)
        {
            return await _verificationRepository.VerifyEmail(verification);
        }
        public async Task ReSendVerificationCodeAsync(string email)
        {
            var minutes = ConfigurationManager.GetSection("someMinutesForVerificationEmail");
            if (minutes == null)
            {
                minutes = 5;
            }

            EmailVerification emailVerification = new EmailVerification()
            {
                Email = email,
                ExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(minutes)),
                VerificationCode = GenerateRandomNomber(1000, 9999)
            };
            await _verificationRepository.UpdateVerificationCodeAsync(emailVerification);
            await SendEmailWithVerificationCodeAsync(emailVerification);
        }
        public async Task SendVerificationCodeAsync(string email)
        {
            EmailVerification emailVerification = new EmailVerification()
            {
                Email = email,
                ExpirationTime = DateTime.Now.AddMinutes(5),
                VerificationCode = GenerateRandomNomber(1000, 9999)
            };
            await SaveVerificationcode(emailVerification);
            await SendEmailWithVerificationCodeAsync(emailVerification);
        }
    }
}
