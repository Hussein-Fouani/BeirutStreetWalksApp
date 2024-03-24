using AutoMapper;
using BeirutWalksDomains.ApiResponse;
using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BeirutWalksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRegions regions;
        protected APIResponse _response;
        public RegionsController(IMapper mapper,IRegions regions)
        {
            this.mapper = mapper;
            _response = new APIResponse();
            this.regions = regions;
        }
        [HttpGet("{Id:guid}",Name ="getregion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="Reader")]
        public async Task<ActionResult<APIResponse>> GetRegionById([FromRoute]Guid Id)
        {
            var region = await regions.GetAsync(I=>I.Id==Id);
            if(region==null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Region not found" };
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            var regionDto = mapper.Map<RegionsDto>(region);
            _response.Result = regionDto;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        [HttpGet(Name ="regions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="Reader")]
        public async Task<ActionResult<APIResponse>> GetAllRegions([FromQuery]string? name = null, [FromQuery] string? sortBy=null, [FromQuery] bool? isAcending=true
            ,int pageNb=1,int pageSize=100)
        {
            try
            {
                var regionsList = await regions.GetAllAsync();
                if (regionsList == null || regionsList.Count <= 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No regions found" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if(!string.IsNullOrEmpty(name))
                {
                    regionsList = regionsList.FindAll(r => r.Name.ToLower().Contains(name.ToLower()));
                }
                if(!string.IsNullOrEmpty(name))
                {
                    regionsList = (List<Regions>)((bool)isAcending ? regionsList.OrderBy(r => r.Name): regionsList.OrderByDescending(i=>i.Name));
                }
                regionsList = regionsList.Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();
                var regionsDto = mapper.Map<List<RegionsDto>>(regionsList);
                _response.Result = regionsDto;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while retrieving regions." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpPost(Name ="addregion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles="Writer")]
        public async Task<ActionResult<APIResponse>> CreateRegion([FromBody] RegionsCreateDto regionsCreateDto)
        {
            try
            {
                if (regionsCreateDto == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Region object is null" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid Model State" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var region = mapper.Map<Regions>(regionsCreateDto);
                await regions.CreateAsync(region);
                _response.Result = region;
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("getregion", new { Id = region.Id }, _response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while creating the region." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpDelete("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<APIResponse>> DeleteRegionByID([FromRoute]Guid Id)
        {
            try
            {
                var region = await regions.GetAsync(I => I.Id == Id);
                if (region == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Region not found" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await regions.DeleteAsync(region);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while deleting the region." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpPut("{Id:guid}", Name = "editregion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<APIResponse>> UpdateRegions([FromBody] RegionUpdateDto regionUpdate, [FromRoute] Guid Id)
        {
            try
            {
                var region = await regions.GetAsync(I => I.Id == Id);

                if (region == null)
                {
                    return NotFound(new APIResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Region not found" },
                        StatusCode = HttpStatusCode.NotFound
                    });
                }

                if (regionUpdate == null)
                {
                    return BadRequest(new APIResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "No Region Object Supplied" },
                        StatusCode = HttpStatusCode.BadRequest
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new APIResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = (List<string>)ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)),
                        StatusCode = HttpStatusCode.BadRequest
                    });
                }

                if (regionUpdate.Id != Id || region.Id != Id)
                {
                    return BadRequest(new APIResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Region Id does not match" },
                        StatusCode = HttpStatusCode.BadRequest
                    });
                }

                // Update region
                var regionDto = mapper.Map(regionUpdate, region);
                await regions.UpdateAsync(region);

                return NoContent();
            }
            catch (Exception )
            {
                

                // Return internal server error
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred while updating the region." },
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

    }
}
