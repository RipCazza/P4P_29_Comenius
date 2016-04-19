using System;
namespace DateChecker
{
    public class Mail
    {
        string msgBody;
        public Mail(string msgBody)
        {
            //
            // TODO: Add constructor logic here
            //
            this.msgBody = msgBody;
        }
        public bool SendMail(string mailTo, string subject)
        {
            System.Net.Mail.MailMessage mailmessage = new System.Net.Mail.MailMessage("profielkeuze@csg-comenius.nl", mailTo);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential("profielkeuze@csg-comenius.nl", "Blaatschaap10");
            smtp.Host = "ex2013.csg-comenius.nl";
            smtp.Port = 578;
            smtp.EnableSsl = true;
            mailmessage.Subject = subject;
            mailmessage.IsBodyHtml = true;
            mailmessage.Body = msgBody;
            /*string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(dir, "test.txt");
            mailmessage.Attachments.Add(new System.Net.Mail.Attachment(path));*/
            try
            {
                smtp.Send(mailmessage);
                Console.WriteLine("Mail send to {0} With the subject {1}", mailTo, subject);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
