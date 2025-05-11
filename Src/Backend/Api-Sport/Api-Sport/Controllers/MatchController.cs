using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer_DataLayer.Models;
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
        [HttpGet("MatchDetails/{id}")]
        public async Task<IActionResult> MatchDetails(int id)
        {
            Match getMatch = await _matcheService.GetMatchDetailsByIdAsync(id);
            return Ok(getMatch);
        }
    }
}
