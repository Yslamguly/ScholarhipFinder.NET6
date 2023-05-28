using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScholarhipFinderAPI.Data;
using ScholarhipFinderAPI.Models;
using ScholarshipFinderAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using ScholarhipFinderAPI.Models.DTOs;
using Npgsql;
using ScholarshipFinderAPI.Models;

namespace ScholarshipFinderAPI.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/api/[controller]")]
[ApiController]

public class WishListController : ControllerBase {

    private readonly ApiDbContext _context;
    private readonly TokenManager _tokenHandler;
    public WishListController(ApiDbContext context, TokenManager tokenHandler)//, JwtConfig config)
    {
        _context = context;
        _tokenHandler = tokenHandler;
    }

    // [Route("wishlists")]
    [HttpGet]
    public async Task<ActionResult<WishListItem>> GetUserWishListItems(){
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var claimsPrincipal = _tokenHandler.ValidateToken(token);

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

    // [Route("addScholarship")]
    [HttpPost("{id}")]
    public async Task<ActionResult<WishListItem>> AddScholarshipToWishList(int id)
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var claimsPrincipal = _tokenHandler.ValidateToken(token);
        var userId = claimsPrincipal.FindFirst("Id")?.Value;

        var wishListId = await _context.WishLists
                .Where(wl=>wl.UserId == userId)
                .Select(wl=>wl.Id)
                .FirstOrDefaultAsync();
        
        if(wishListId != default){
            var newItem = new WishListItem{
                WishListId = wishListId,
                ScholarshipId = id,
                DateAdded = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            try{
                _context.WishListItems.Add(newItem);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(DbUpdateException ex){
                Npgsql.PostgresException innerException = (PostgresException)(ex.InnerException as NpgsqlException);

                if(innerException.SqlState == "23505"){
                        return Conflict(new AuthResult{
                        Result = false,
                        Errors = new List<string>{"The scholarship is already in wish list."}
                    });
                }
                else{
                    return BadRequest();
                }
            }
        }

        return BadRequest();

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<WishListItem>> DeleteScholarshipFromWishList(int id)
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var claimsPrincipal = _tokenHandler.ValidateToken(token);
        var userId = claimsPrincipal.FindFirst("Id")?.Value;

        var wishListId = await _context.WishLists
                .Where(wl=>wl.UserId == userId)
                .Select(wl=>wl.Id)
                .FirstOrDefaultAsync();
        
        var itemToDelete = _context.WishListItems
                    .Where(item => item.WishListId == wishListId && item.ScholarshipId == id)
                    .SingleOrDefault();
        if (itemToDelete != null)
        {
            try{
                _context.WishListItems.Remove(itemToDelete);
                await _context.SaveChangesAsync();
                 
                return Ok();
            }
            catch(DbUpdateException ex){
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        return BadRequest();
    }


}