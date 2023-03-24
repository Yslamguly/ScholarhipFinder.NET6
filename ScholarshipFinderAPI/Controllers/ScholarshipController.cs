using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScholarhipFinderAPI.Data;
using ScholarhipFinderAPI.Models;

namespace ScholarhipFinderAPI.Controllers;
[Route("/api/[controller]")]
[ApiController]

public class ScholarshipController : ControllerBase
{
    // private readonly UserManager<IdentityUser> _userManager;
    private readonly ApiDbContext _context;
    // private readonly JwtConfig _config;
    private readonly IConfiguration _configuration;
    public ScholarshipController(ApiDbContext context, IConfiguration configuration)//, JwtConfig config)
    {
        _context = context;
        _configuration = configuration;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("scholarships")]
    [HttpGet]
    public async Task<ActionResult<Scholarship>> GetScholarshipsAsync()
    {
        var claimsPrincipal = ValidateToken();
        var userId = claimsPrincipal.FindFirst("Id")?.Value;
        var scholarships = await _context.Scholarships
        .Join(_context.WishListItems, s => s.Id, wli => wli.ScholarshipId, (s, wli) => new { Scholarship = s, WishListItem = wli })
        .Join(_context.WishLists, sw => sw.WishListItem.WishListId, wl => wl.Id, (sw, wl) => new { Scholarship = sw.Scholarship, WishList = wl })
        .Where(sw => sw.WishList.UserId == userId)
        .Select(sw => new
        {
            sw.Scholarship.Id,
            sw.Scholarship.Title,
            sw.Scholarship.Description,
            sw.Scholarship.Deadline
        })
        .ToListAsync();


        return Ok(scholarships);

    }


    private ClaimsPrincipal ValidateToken()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);


        // Set the validation parameters
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        // Validate the token
        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        return claimsPrincipal;
    }
}