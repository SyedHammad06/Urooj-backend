using System.Text.Json.Serialization;

namespace Hauna.Urooj.Hauna.Urooj.Models
{
    public class UserInfoModel
    {
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string UserAddress { get; set; }
        public byte[] Phote { get; set; }
        public string Adhaar { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PersonalEmail { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
