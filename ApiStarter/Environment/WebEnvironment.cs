using AutoMapper;
using BaseStarter.DAL;
using BaseStarter.Environment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApiStarter.Environment
{

    /// <summary>
    /// Object with global features added transient in program.cs (DbContext, Logger, Mapper, ...)
    /// </summary> 
    public class WebEnvironment
    {
        private GlobalEnvironment _globalEnvironment;
        private IWebHostEnvironment _webHostEnvironment;

        public WebEnvironment(StarterDbContext dbContext, IConfiguration configuration, ILogger<Object> logger, IMapper mapper, [FromServices] IWebHostEnvironment env)
        {
            _globalEnvironment = new GlobalEnvironment(dbContext, configuration, logger, mapper);
            _webHostEnvironment = env;

        }

        public IWebHostEnvironment WebHostEnvironment { get { return _webHostEnvironment; } }
        public GlobalEnvironment GlobalEnvironment { get { return _globalEnvironment; } }

    }
}
