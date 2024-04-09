using AutoMapper;
using BaseStarter.DataViews;
using BaseStarter.Models;

namespace BaseStarter
{
    
    /// <summary>
    /// Mapping object by AutoMapper
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, Client>();
            CreateMap<Client, ClientDVRow>();
            
        }
    }
}
