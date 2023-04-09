﻿using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FormatController : ControllerBase
	{
		private readonly AppDbContext _context;
		public FormatController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IEnumerable<Format> Get()
		{
			return _context.formats.Include(x => x.Presentations).ToArray();
		}
		[HttpPost]
		public void Post([FromBody] Format value)
		{
			_context.formats.Add(value);
			_context.SaveChanges();
		}

	}
}
