using ApiServer.BLL.IBLL;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace ApiServer.BLL.BLL
{
    public class MailService : IMailService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">接收者邮箱</param>
        /// <param name="sendMsg">发送的邮件信息</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string toEmail, string sendMsg)
        {
            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "标题";
            message.Sender = new MailboxAddress("发件人姓名", "发件人Email地址");
            message.Body = new TextPart(TextFormat.Html) { Text = sendMsg };

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("账号", "密码");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
