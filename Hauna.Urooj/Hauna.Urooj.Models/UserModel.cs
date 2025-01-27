namespace Hauna.Urooj.Hauna.Urooj.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string EncryptedPassword { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int InfoId {  get; set; }
    }
}
