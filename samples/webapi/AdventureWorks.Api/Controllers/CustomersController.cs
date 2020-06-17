using System.Linq;
using Microsoft.AspNetCore.Mvc;

using AdventureWorks.Data;

namespace AdventureWorks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class CustomersController : ControllerBase
    {
		public AdventureworksContext Context { get; }

		public CustomersController(AdventureworksContext context)
		{
			Context = context;
		}

        [HttpGet]
		public IActionResult Get()
		{
			return Ok(Context.Customer.Select(c => new {
				c.FirstName,
				c.MiddleName,
				c.LastName,
				c.EmailAddress,
				c.ModifiedDate,
				c.CompanyName,
				c.Title,
				c.Phone
			}).ToArray());
		}
    }
}