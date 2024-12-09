using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallHistoryController : ControllerBase
    {
        private readonly ICallHistoryRepository callHistoryRepository;

        public CallHistoryController(ICallHistoryRepository callHistoryRepository)
        {
            this.callHistoryRepository = callHistoryRepository;
        }

        // PUT: api/CallHistory
        // [HttpPut("{callId}")]
        // public async Task<IActionResult> UpdateCallHistory(int callId, [FromBody] CallHistory request)
        // {
        //     if (request.CallId != callId)
        //         return BadRequest(new { message = "CallId in the body does not match the URL CallId." });

        //     if (request.CallId <= 0)
        //         return BadRequest(new { message = "Invalid CallId." });

        //     try
        //     {
        //         await callHistoryRepository.UpdateCallHistoryAsync(request.CallId, request.Status, request.Notes);
        //         return Ok(new { message = "Call history updated successfully." });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { error = "An error occurred while updating the call history." });
        //     }
        // }

        [HttpPut]
        public async Task<IActionResult> UpdateCallHistory([FromBody] CallHistory callHistory)
        {
            try
            {
                return Ok(new { message = "Updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
