using System.Collections.Generic;
using AdventureWorks.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AdventureWorks.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public AdventureworksContext Context { get; }

        public ProductsController(AdventureworksContext context)
        {
            Context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return Context.Product.Select(p => new {
                Name = p.Name,
                Color = p.Color,
                ListPrice = p.ListPrice
            }).ToArray();
        }
    }
}