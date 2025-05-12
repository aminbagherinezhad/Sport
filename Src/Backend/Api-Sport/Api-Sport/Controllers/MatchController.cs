using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer_DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Sport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatcheService _matcheService;
        public MatchController(IMatcheService matcheService)
        {
            _matcheService = matcheService;
        }
        [Authorize]
        [HttpGet("MatchDetails/{id}")]
        public async Task<IActionResult> MatchDetails(int id)
        {
            Match getMatch = await _matcheService.GetMatchDetailsByIdAsync(id);
            if (getMatch == null)
                return NotFound();
            // Use Dapper
            return Ok(getMatch);
        }

        [Authorize]
        [HttpGet("CreateMatch")]
        public async Task<IActionResult> CreateMatch([FromBody] Match match)
        {
            if (match == null)
                return BadRequest();
            await _matcheService.CreateMatchAsync(match);
            return Ok(match);
        }
    }
}
