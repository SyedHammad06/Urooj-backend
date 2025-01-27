namespace Hauna.Urooj.Hauna.Urooj.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string UserName {  get; set; }
        public string Password {  get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
