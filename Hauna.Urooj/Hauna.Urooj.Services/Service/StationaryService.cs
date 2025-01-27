using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class StationaryService : IStationaryService
    {
        private readonly IStationaryRepository _stationaryRepository;

        public StationaryService(IStationaryRepository stationaryRepository)
        {
            _stationaryRepository = stationaryRepository;
        }

        public IEnumerable<StationaryModel> GetAll()
        {
            var stationaries = _stationaryRepository.GetAllStationary();
            return stationaries;
        }

        public StationaryModel GetStationaryById(int id)
        {
            var stationary = _stationaryRepository.GetStationaryById(id);
            return stationary;
        }

        public void Remove(int id, string ModifiedBy)
        {
            _stationaryRepository.removeStationary(id, ModifiedBy);
        }

        public void CreateStationary(StationaryModel stationary)
        {
            _stationaryRepository.CreateStationary(stationary);
        }

        public void EditStationary(StationaryModel stationary)
        {
            _stationaryRepository.EditStationary(stationary);
        }
    }
}
