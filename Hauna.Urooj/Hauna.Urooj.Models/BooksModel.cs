using System.Text.Json.Serialization;

namespace Hauna.Urooj.Hauna.Urooj.Models
{
    public class BooksModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public string BookContent { get; set; }
        public string Subject { get; set; }
        public string Class { get; set; }
        public string HProgram { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }
        public string BookUrl { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
