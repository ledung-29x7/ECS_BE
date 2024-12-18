using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallStatusController : ControllerBase
    {
        private readonly ICallStatusRepository _callStatusRepository;

        public CallStatusController(ICallStatusRepository callStatusRepository)
        {
            _callStatusRepository = callStatusRepository;
        }

        // GET: api/CallStatus
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var callStatuses = await _callStatusRepository.GetAllCallStatus();
            return Ok(callStatuses);
        }

        // GET: api/CallStatus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var callStatus = await _callStatusRepository.GetCallStatusById(id);
            if (callStatus == null)
            {
                return NotFound();
            }
            return Ok(callStatus);
        }

        // POST: api/CallStatus
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CallStatus callStatus)
        {
            if (callStatus == null || string.IsNullOrEmpty(callStatus.StatusName))
            {
                return BadRequest("Invalid data.");
            }

            await _callStatusRepository.AddCallStatus(callStatus);
            return CreatedAtAction(nameof(GetById), new { id = callStatus.StatusId }, callStatus);
        }

        // PUT: api/CallStatus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CallStatus callStatus)
        {
            if (id != callStatus.StatusId || callStatus == null || string.IsNullOrEmpty(callStatus.StatusName))
            {
                return BadRequest("Invalid data.");
            }

            await _callStatusRepository.UpdateCallStatus(callStatus);
            return NoContent();
        }

        // DELETE: api/CallStatus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _callStatusRepository.DeleteCallStatus(id);
            return NoContent();
        }
    }
}
