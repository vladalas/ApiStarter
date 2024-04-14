using AutoMapper;
using BaseStarter.DataViews;


namespace BaseStarter
{
    
    /// <summary>
    /// Mapping object by AutoMapper
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Client, Models.Client>();
            CreateMap<Models.Client, DTO.Client>();
            CreateMap<DTO.Client, Models.Client>();
            CreateMap<Models.Client, ClientDVRow>();
            
        }
    }
}
