using System.Text.Json.Serialization;

namespace Hauna.Urooj.Hauna.Urooj.Models
{
    public class StationaryModel
    {
        public int StationaryId { get; set; }
        public string Title { get; set; }
        public string StationaryDescription { get; set; }
        public string StationaryPrice { get; set; }
        public string StationaryUrl { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
