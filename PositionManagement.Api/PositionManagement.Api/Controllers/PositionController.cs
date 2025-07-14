using Microsoft.AspNetCore.Mvc;
using PositionManagement.Api.Model;
using PositionManagement.Api.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PositionManagement.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase {
        private readonly ILogger<PositionController> _logger;
        private readonly IPositionService _positionService;

        public PositionController(ILogger<PositionController> logger, IPositionService positionService) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _positionService = positionService ?? throw new ArgumentNullException(nameof(positionService));
        }

        // GET: api/<PositionController>
        [HttpGet]
        public async Task<ActionResult<CurrentPosition>> Get() {
            try {
                var positions = await _positionService.GetPositions();
                return Ok(positions);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving positions");
                return StatusCode(500, "Internal server error");
            }
        }

        // INSERT
        // POST api/<PositionController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Model.Position position) {
            try {
                await _positionService.InsertAsync(position);
                return Ok();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error inserting position");
                return StatusCode(500, "Internal server error");
            }
        }

        // UPDATE
        // PUT api/<PositionController>
        [HttpPut()]
        public async Task<ActionResult> Put([FromBody] Model.Position position) {
            try {
                await _positionService.UpdateAsync(position);
                return Ok();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error updating position");
                return StatusCode(500, "Internal server error");
            }
        }

        // CANCEL
        // DELETE api/<PositionController>
        [HttpDelete()]
        public async Task<ActionResult> DeleteAsync(Model.Position position) {
            try {
                await _positionService.CancelAsync(position);
                return Ok();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error canelling position");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
