using System.Collections.Generic;
using AdventureWorks.Data;
using AdventureWorks.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

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
            //return Context.Product.Select(p => new {
            //    Name = p.Name,
            //    Color = p.Color,
            //    ListPrice = p.ListPrice
            //}).ToArray();

            return JsonSerializer.Deserialize<object[]>(JSON);
        }

        const string JSON = @"[
            { ""name"": ""HL Road Frame - Red, 58"", ""color"": ""Red"",""listPrice"": 1431.5 },
            { ""name"": ""Sport-100 Helmet, Red"",""color"": ""Red"",""listPrice"": 34.99 },
			{ ""name"": ""Sport-100 Helmet, Black"", ""color"": ""Black"",""listPrice"": 34.99 },
            { ""name"": ""Mountain Bike Socks, M"",""color"": ""White"",""listPrice"": 9.5 }
		]";
    }
}