using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hauna.Urooj.Hauna.Urooj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationaryController : ControllerBase
    {
        private readonly IStationaryService _stationaryService;
        public StationaryController(IStationaryService stationaryService) 
        {
            _stationaryService = stationaryService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() 
        { 
            var stationaries = _stationaryService.GetAll();
            if (stationaries == null) return NotFound();
            return Ok(stationaries);
        }

        [HttpGet("GetById/{stationaryId}")]
        public IActionResult Get(int stationaryId)
        {
            var stationary = _stationaryService.GetStationaryById(stationaryId);
            if (stationary == null) return NotFound();
            return Ok(stationary);
        }

        [Authorize]
        [HttpPost("Remove/{stationaryId}")]
        public IActionResult Remove(int stationaryId, string ModifiedBy)
        {
            _stationaryService.Remove(stationaryId, ModifiedBy);
            return Ok();
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult Create(StationaryModel model)
        {
            _stationaryService.CreateStationary(model);
            return Ok();
        }

        [Authorize]
        [HttpPost("Update")]
        public IActionResult Update(StationaryModel model)
        {
            _stationaryService.EditStationary(model);
            return Ok();
        }
    }
}
