using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Venn.Client.Services
{
    public static class EmailService
    {
        public static void SendMail(string email, string subject, string body)
        {
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    string senderEmail = "dma.venn@gmail.com";
                    string senderPassword = "siti ufmn yeuq ugzu";

                    string recipientEmail = email;


                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(senderEmail);
                    message.Subject = subject;
                    message.To.Add(new MailAddress(recipientEmail));
                    message.Body = body;
                    message.IsBodyHtml = true;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(senderEmail, senderPassword),
                        EnableSsl = true
                    };

                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + $" to {email}");
                }
            }
            else
            {
                MessageBox.Show("empty");
            }
        }
    }
}
