using ManageCustomer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManageCustomer.Controllers
{
    [Route("api")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _service;

        public LeaderboardController(ILeaderboardService service)
        {
            _service = service;
        }

        [HttpPost("customer/{customerId}/score/{scoreDelta}")]
        public IActionResult UpdateScore(long customerId, decimal scoreDelta)
        {
            var score = _service.UpdateScore(customerId, scoreDelta);
            return Ok(score);
        }

        [HttpGet("leaderboard")]
        public IActionResult GetCustomersByRank(int start=0, int end=0)
        {
            var customers = _service.GetCustomersByRank(start, end);
            return Ok(customers);
        }

        [HttpGet("leaderboard/{customerId}")]
        public IActionResult GetCustomerById(long customerId, int high = 0, int low = 0)
        {
            var customer = _service.GetCustomerById(customerId, high, low);
            return Ok(customer);
        }
    }

}