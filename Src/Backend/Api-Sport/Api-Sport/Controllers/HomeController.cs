using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Sport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMatcheService _matcheService;
        public HomeController(IMatcheService matcheService)
        {
            _matcheService = matcheService;
        }
        [Authorize]
        [HttpGet("Home")]
        public async Task<IActionResult> Home()
        {
            var getListMatches = await _matcheService.GetAllmatchesAsync();
            return Ok(getListMatches);
        }

    }
}
