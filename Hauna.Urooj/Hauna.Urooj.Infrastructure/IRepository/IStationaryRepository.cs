using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository
{
    public interface IStationaryRepository
    {
        IEnumerable<StationaryModel> GetAllStationary();
        void removeStationary(int stationaryId,string ModifiedBy);
        void CreateStationary(StationaryModel stationary);
        void EditStationary(StationaryModel editedStationary);
        StationaryModel GetStationaryById(int stationaryId);
    }
}
