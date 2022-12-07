using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
	public static class VillaStore
	{
		public static List<VillaDto> VillaList = new List<VillaDto>
			{
				new VillaDto {Id=1,Name="Pool View",Occupancy=3,Sqft=100},
				new VillaDto {Id=2,Name="Beach View",Occupancy=4,Sqft=200}
			};
	}
}
