using AutoMapper;
using BeirutWalksDomains.ApiResponse;
using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Writers;
using System.Linq.Expressions;
using System.Net;

namespace BeirutWalksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalks walks;
        protected APIResponse apiResponse;

        public WalksController(IMapper mapper,IWalks walks)
        {
            this.mapper = mapper;
            this.walks = walks;
            apiResponse= new();
        }

        [HttpGet("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetWalks([FromRoute]Guid Id)
        {
            try
            {
                var walk = await walks.GetAsync(filter: w => w.Id == Id);
                if (walk == null)
                {
                    apiResponse.ErrorMessages = new List<string> { "No Walk Found" };
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(apiResponse);
                }
                apiResponse.IsSuccess = true;
                apiResponse.Result = walk;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(apiResponse);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while updating the region." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllWalks([FromQuery]string? name,int pagesize=5,int pagenb=0 )
        {
            var query = await walks.GetAllAsync();



            if (query != null)
            {
                query.Where(w => w.Name.Contains(name));
                apiResponse.IsSuccess=true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(apiResponse);
            }

            apiResponse.IsSuccess = true;
            apiResponse.Result=query;
            apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(apiResponse);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateWalks([FromBody] CreateWalksDto walksDto)
        {
            try
            {
                if (walksDto == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages = new List<string> { "NO Supplied Data" };
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(apiResponse);
                }
                if (!ModelState.IsValid)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessages = new List<string> { "Supplied Data Not Supported"};
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(apiResponse);
                }
                var wal = mapper.Map<Walks>(walksDto);
                await walks.CreateAsync(wal);
                apiResponse.IsSuccess = true;
                apiResponse.Result = wal;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetWalks", new { Id = wal.Id }, apiResponse);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while updating the region." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpDelete("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteWalks(Guid Id)
        {
            var walk = await walks.GetAsync(i=>i.Id == Id);
            if (walk == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string> { "NO walk Found"};
                return BadRequest(apiResponse);
            }
           await walks.DeleteAsync(walk);
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.NoContent;
            return Ok(apiResponse);
        }
        [HttpPut("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateWalks(Guid Id, [FromBody] UpdateWalksDto walksDto)
        {
            var walk = await walks.GetAsync(i=>i.Id==Id);
            if (walk == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string> { "No walk found"};
                return NotFound(apiResponse);
            }
            if(Id!=walksDto.Id)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.NotFound;
                apiResponse.ErrorMessages = new List<string> { " Data Not found" };
                return BadRequest(apiResponse);
            }
            await walks.UpdateAsync(walk);

            apiResponse.IsSuccess = true;
            apiResponse.StatusCode= HttpStatusCode.Accepted;
            apiResponse.Result = mapper.Map<Walks>(walksDto);
            return Ok(apiResponse);
        }
    }

}
