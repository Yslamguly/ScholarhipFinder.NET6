using System;
using Microsoft.EntityFrameworkCore;

namespace ScholarhipFinderAPI.Data
{
	public class ApiDbContext : DbContext
	{
		public ApiDbContext(DbContextOptions<ApiDbContext> options):base(options)
		{

		}

	}
}