using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallHistoryController : ControllerBase
    {
        private readonly ICallHistoryReponsitory _callHistoryReponsitory;

        public CallHistoryController(ICallHistoryReponsitory callHistoryReponsitory)
        {
            _callHistoryReponsitory = callHistoryReponsitory;
        }

        [HttpGet]
        public async Task<IActionResult> GetCallHistories()
        {
            try
            {
                var callHistories = await _callHistoryReponsitory.GetCallHistories();
                return Ok(callHistories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCallHistoryById(int id)
        {
            try
            {
                var callHistory = await _callHistoryReponsitory.GetCallHistorisbyId(id);

                if (callHistory == null)
                {
                    return NotFound($"Call history with ID {id} was not found.");
                }

                return Ok(callHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCallHistory([FromBody] CallHistory callHistory)
        {
            if (callHistory == null)
            {
                return BadRequest("Call history object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Gọi repository để thêm CallHistory và nhận lại CallId
                var callId = await _callHistoryReponsitory.AddCallHistory(callHistory);

                if (callId > 0)
                {
                    return Ok(new
                    {
                        Message = "Call history added successfully.",
                        CallId = callId
                    });
                }
                else
                {
                    return StatusCode(500, "An error occurred while adding the call history.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCallHistory(int id, [FromBody] CallHistory callHistory)
        {
            if (id != callHistory.CallId)
            {
                return BadRequest("Call history ID mismatch.");
            }

            if (callHistory == null)
            {
                return BadRequest("Call history object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _callHistoryReponsitory.UpdateCallHistory(callHistory);
                return Ok("Call history updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCallHistory(int id)
        {
            try
            {
                await _callHistoryReponsitory.DeleteCallHistory(id);
                return Ok($"Call history with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
