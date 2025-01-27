using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Services.Interface
{
    public interface IStationaryService
    {
        void CreateStationary(StationaryModel stationary);
        void EditStationary(StationaryModel stationary);
        IEnumerable<StationaryModel> GetAll();
        StationaryModel GetStationaryById(int id);
        void Remove(int id, string ModifiedBy);
    }
}
