using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using AdventureWorks.Data;
using Category = AdventureWorks.Data.Models.ProductCategory;

namespace AdventureWorks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class CategoriesController : ControllerBase
    {
		public AdventureworksContext Context { get; }

		public CategoriesController(AdventureworksContext context)
		{
			Context = context;
		}

        [HttpGet]
		public IActionResult Get()
		{
			return Ok(
 				Context.ProductCategory.Select(record => new {
					 Id = record.ProductCategoryId,
					 Name = record.Name,
					 ModifiedDate = record.ModifiedDate,
					 Products = record.Product.Select(item => new {
						 Id = item.ProductId,
						 item.Name,
						 item.ListPrice,
						 item.Color,
						 item.ModifiedDate
					 }).ToArray()
				 })
			);
		}
    }
}