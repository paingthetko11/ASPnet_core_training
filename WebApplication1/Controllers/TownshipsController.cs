using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownshipsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TownshipsController(AppDbContext context)

        {
            _context = context;
        }

        [HttpPost]
        [EndpointSummary("Create new township")]
        [EndpointDescription("Creating a new township")]
        public async Task<IActionResult> PostTownship([FromBody] Township township)
        {
            if (await _context.Townships.AnyAsync(x => x.TownshipId == township.TownshipId))
            {
                return BadRequest("Township Already Exist");
            }

            _ = _context.Townships.Add(township);
            _ = await _context.SaveChangesAsync();

            return Created("api/Townships", new DefaultResponseModel()
            {
                Success = true,
                Statuscode = StatusCodes.Status200OK,
                Data = township,
                Message = "Successfully saved."
            });
        }

        [HttpGet]
        [EndpointSummary("Get all townships")]
        [EndpointDescription("Get all townships")]
        public async Task<IActionResult> GetTownships()
        {
            List<Township> townships = await _context.Townships.ToListAsync();
            int totalTownships = townships.Count;
            return Ok(new
            {
                TotalTownships = totalTownships,
                Townships = townships
            });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete township")]
        [EndpointDescription("Delete a township")]
        public async Task<IActionResult> DeleteTownshipAsync(string id)
        {
            Township? township = await _context.Townships.FirstOrDefaultAsync(x => x.TownshipId == id);
            if (township == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Township Not Found."
                });
            }
            _ = _context.Townships.Remove(township);
            return _context.SaveChanges() > 0
                ? Created("api/townships/{id}", new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Data = township,
                    Message = "Successfully deleted."
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Failed to save."
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update township")]
        [EndpointDescription("Update a township")]
        public async Task<IActionResult> PutTownship(string id, [FromBody] Township township)
        {
            Township? townshipData = await _context.Townships.FirstOrDefaultAsync(x => x.TownshipId == id);
            if (townshipData is null)
            {
                return NotFound();
            }
            townshipData.TownshipName = township.TownshipName;
            townshipData.Longitude = township.Longitude;
            townshipData.Lattitude = township.Lattitude;

            return _context.SaveChanges() > 0
                ? Created("api/townships/{id}", new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Data = townshipData,
                    Message = "Sucessfully Updated"
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = " failed to Updated"
                });
        }

        [HttpGet("tspagination")]
        [EndpointSummary("Get all townships with pagination")]
        [EndpointDescription("Get all townships with pagination")]

        public IActionResult GetTownshipsWithPagination(int page = 1, int pageSize = 10)
        {
            List<Township> townships = _context.Townships.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(townships);
        }




    }
}


