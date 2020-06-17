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
    public class CustomersController : ControllerBase
    {
        public AdventureworksContext Context { get; }

        public CustomersController(AdventureworksContext context)
        {
            Context = context;
        }

        [HttpGet]
		public IActionResult Get(bool? useCached = true)
		{
            if (useCached == true) {
                return Ok(JsonSerializer.Deserialize<object[]>(JSON));
            }
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

        const string JSON = @"[
        {
           ""firstName"": ""Luis"",
           ""middleName"": ""N."",
           ""lastName"": ""Gee"",
           ""emailAddress"": ""luis@bot.com"",
           ""modifiedDate"": ""2005-08-01T00:00:00"",
           ""companyName"": ""A Bike Store"",
           ""title"": ""Mr."",
           ""phone"": ""245-555-0173""
        },
        {
            ""firstName"": ""Keith"",
            ""middleName"": null,
            ""lastName"": ""Harris"",
            ""emailAddress"": ""keith0@adventure-works.com"",
            ""modifiedDate"": ""2006-08-01T00:00:00"",
            ""companyName"": ""Progressive Sports"",
            ""title"": ""Mr."",
            ""phone"": ""170-555-0127""
        },
        {
            ""firstName"": ""Donna"",
            ""middleName"": ""F."",
            ""lastName"": ""Carreras"",
            ""emailAddress"": ""donna0@adventure-works.com"",
            ""modifiedDate"": ""2005-09-01T00:00:00"",
            ""companyName"": ""Advanced Bike Components"",
            ""title"": ""Ms."",
            ""phone"": ""279-555-0130""
        },
        {
           ""firstName"": ""Janet"",
           ""middleName"": ""M."",
           ""lastName"": ""Gates"",
           ""emailAddress"": ""janet1@adventure-works.com"",
           ""modifiedDate"": ""2006-07-01T00:00:00"",
           ""companyName"": ""Modular Cycle Systems"",
           ""title"": ""Ms."",
           ""phone"": ""710-555-0173""
        },
        {
           ""firstName"": ""Lucy"",
           ""middleName"": null,
           ""lastName"": ""Harrington"",
           ""emailAddress"": ""lucy0@adventure-works.com"",
           ""modifiedDate"": ""2006-09-01T00:00:00"",
           ""companyName"": ""Metropolitan Sports Supply"",
           ""title"": ""Mr."",
           ""phone"": ""828-555-0186""
        }    
        ]";
    }
}