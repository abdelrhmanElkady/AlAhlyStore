

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Shirt, ShirtDto>()
                .ForMember(x => x.Player, o => o.MapFrom(s => s.Player!.Name))
                .ForMember(x => x.Color, o => o.MapFrom(s => s.Color!.Name))
                .ForMember(x => x.Size, o => o.MapFrom(s => s.Size!.Name));
        }
    }
}
