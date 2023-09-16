

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // shirts mapping
            CreateMap<Shirt, ShirtDto>()
                .ForMember(x => x.Player, o => o.MapFrom(s => s.Player!.Name));
            CreateMap<ShirtDto, Shirt>()
                .ForMember(x => x.PlayerId, o => o.Ignore())
                .ForMember(x => x.Player, o => o.Ignore());

            // players mapping
            CreateMap<Player,PlayerDto>()
                .ReverseMap();
        }
    }
}
