using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using MagicVilla_API.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_API.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;

        public VillaNumberAPIController(ILogging logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbvilla, IMapper mapper)
        {
            _logger = logger;
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbvilla;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            _logger.Log("Getting All VillaNumbers", "info");

            try
            {
                IEnumerable<VillaNumber> VillaNumberList = await _dbVillaNumber.GetAllAsync();

                _response.Result = _mapper.Map<List<VillaNumberDto>>(VillaNumberList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Get villaNumber with Id:" + id, "error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _dbVillaNumber.GetAsync(a => a.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createDTO)
        {
            try
            {
                // Add custom modelstate validation
                if (await _dbVillaNumber.GetAsync(a => a.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number already exists!");
                    return BadRequest(ModelState);
                }

                if (await _dbVilla.GetAsync(a => a.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is invalid!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                await _dbVillaNumber.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(a => a.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                await _dbVillaNumber.RemoveAsync(villaNumber);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(a => a.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is invalid!");
                    return BadRequest(ModelState);
                }

                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                await _dbVillaNumber.UpdateAsync(model);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            return _response;
        }
    }
}
