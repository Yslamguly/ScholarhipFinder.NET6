using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScholarhipFinderAPI.Data;
using ScholarhipFinderAPI.Helpers;
using ScholarhipFinderAPI.Models;
using ScholarhipFinderAPI.Models.DTOs;

namespace ScholarhipFinderAPI.Controllers;
[Route("/api/[controller]")]
[ApiController]

public class ScholarshipController : ControllerBase
{
    // private readonly UserManager<IdentityUser> _userManager;
    private readonly ApiDbContext _context;
    // private readonly JwtConfig _config;
    private readonly TokenManager _tokenHandler;
    public ScholarshipController(ApiDbContext context, TokenManager tokenHandler)//, JwtConfig config)
    {
        _context = context;
        _tokenHandler = tokenHandler;
    }


    [Route("scholarships")]
    [HttpGet]
    public async Task<ActionResult<List<Scholarship>>> GetScholarshipsAsync()
    {
        try{
            var scholarships = await _context.Scholarships.ToListAsync();

            return Ok(scholarships);

        }catch
        {
            return BadRequest();
        }
    }
    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult<Scholarship>> GetScholarshipByIdAsync(int id){
        var scholarship = await _context.Scholarships.FindAsync(id);
            if(scholarship == null)
            {
                return BadRequest("Scholarship not found");
            }
            return Ok(scholarship);
    }
    [Route("getScholarshipsByCategory/{categoryId}")]
    [HttpGet]
    public async Task<ActionResult<List<Scholarship>>> GetScholarshipsByCategoryAsync(int categoryId){
        try{
            var scholarships = await _context.Scholarships
            .Join(_context.ScholarshipCategories,s=>s.Id,sc=>sc.CategoryId,(s,sc)=> new{Scholarship=s,ScholarshipCategory=sc})
            .Where(ss=>ss.ScholarshipCategory.CategoryId==categoryId)
            .Select(ss=> new {
                ss.Scholarship.Id,
                ss.Scholarship.Description,
                ss.Scholarship.Image,
                ss.Scholarship.Link,
                ss.Scholarship.Deadline
            }).ToListAsync();

            return Ok(scholarships);

        }
        catch
        {
            return BadRequest("Scholarshis are not found");
        }
    }
}