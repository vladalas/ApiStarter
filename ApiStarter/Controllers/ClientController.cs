using ApiStarter.Environment;
using BaseStarter;
using BaseStarter.DAL;
using BaseStarter.DataViews;
using BaseStarter.Filters;
using BaseStarter.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiStarter.Controllers
{
    /// <summary>
    /// Controller for working with Client
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController
    {

        public ClientController(WebEnvironment webEnvironment) : base(webEnvironment)
        {

        }

        /// <summary>
        /// Return list of clients filtered and sorted, with paggination
        /// </summary>
        /// <param name="dataViewRequest"></param>
        /// <returns></returns>
        /// <response code="200">Return list of clients filtered and sorted, with paggination</response>
        /// <response code="500">Internal error</response>
        [HttpPost]
        [Route("List")]
        public async Task<ActionResult<DataViewResult<ClientDVRow>>> Post(DataViewRequestFilter<ClientFilter> dataViewRequest)
        {
            try
            {
                var dvResult = await ClientManager.ListAsync(WebEnvironment.GlobalEnvironment, dataViewRequest.Filter, dataViewRequest);
                return Ok(dvResult);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }



        /// <summary>
        /// Return Client by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return Client by Id</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseStarter.DTO.Client>> Get(int id)
        {
            try
            {
                var client = await ClientManager.GetClientAsync(WebEnvironment.GlobalEnvironment, id);
                if (client == null)
                {
                    return NotFound();
                }
                BaseStarter.DTO.Client clientResult = new();
                WebEnvironment.GlobalEnvironment.Mapper.Map(client, clientResult);
                return Ok(clientResult);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        /// <summary>
        /// Insert or Update Client
        /// (Id = 0 - insert, Id > 0 - update)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        /// <response code="200">Client successfully Save, return Id</response>
        /// <response code="400">Problem with validation</response>
        /// <response code="404">Objekt with Id not found</response>
        /// <response code="500">Internal error</response>
        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult<SaveResultWithValidation>> Save(BaseStarter.DTO.Client client)
        {
            try
            {
                BaseStarter.Models.Client cln = new();
                WebEnvironment.GlobalEnvironment.Mapper.Map(client, cln);
                var res = await ClientManager.SaveAsync(WebEnvironment.GlobalEnvironment, cln);

                if (res.Success == false)
                {
                    return BadRequest(res);
                }

                return Ok(res);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
