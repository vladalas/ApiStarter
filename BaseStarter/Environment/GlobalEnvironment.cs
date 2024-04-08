using AutoMapper;
using BaseStarter.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BaseStarter.Environment
{
    
    /// <summary>
    /// Object with global features for base work (DbContext, Logger, Mapper, ...)
    /// </summary>
    public class GlobalEnvironment
    {
        private StarterDbContext _dbContext;
        private IConfiguration _configuration;
        private ILogger<Object> _logger;
        private IMapper _mapper;

        public StarterDbContext DbContext { get { return _dbContext; } }
        public ILogger Logger { get { return _logger; } }
        public IMapper Mapper { get { return _mapper; } }

        public GlobalEnvironment(StarterDbContext dbContext, IConfiguration configuration, ILogger<Object> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;

        }

       


    }
}
