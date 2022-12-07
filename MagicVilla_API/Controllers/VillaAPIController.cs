using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDto>> GetVillas()
		{
			return Ok(VillaStore.VillaList);
		}

		[HttpGet("{id:int}",Name ="GetVilla")]		
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[ProducesResponseType(200,Type =typeof(VillaDto))]
		public ActionResult<VillaDto> GetVilla(int id)
		{
			if (id==0)
			{
				return BadRequest();
			}

			var villa = VillaStore.VillaList.FirstOrDefault(a =>a.Id == id);

			if (villa == null)
			{
				return NotFound();
			}

			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDTO)
		{
			if (villaDTO == null)
			{
				return BadRequest();
			}

			if (villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			villaDTO.Id = VillaStore.VillaList.OrderByDescending(a=>a.Id).FirstOrDefault().Id + 1;
			//villaDTO.Id = VillaStore.VillaList.MaxBy(a=>a.Id).Id + 1;

			VillaStore.VillaList.Add(villaDTO);

			return CreatedAtRoute("GetVilla",new { id = villaDTO.Id },villaDTO);
		}
	}
}
