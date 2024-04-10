using AutoMapper.QueryableExtensions;
using BaseStarter.DAL;
using BaseStarter.DataViews;
using BaseStarter.Environment;
using BaseStarter.Filters;
using BaseStarter.Models;
using Microsoft.EntityFrameworkCore;


namespace BaseStarter.Managers
{

    /// <summary>
    /// Manager for work with Client
    /// </summary>
    public static class ClientManager
    {

        /// <summary>
        /// Return list of trips filtered and sorted, with paggination
        /// </summary>
        /// <param name="webEnvironment"></param>
        /// <param name="filter"></param>
        /// <param name="dataViewRequest"></param>
        /// <returns></returns>
        public static async Task<DataViewResult<ClientDVRow>> ListAsync(GlobalEnvironment globalEnvironment, ClientFilter filter, DataViewRequest dataViewRequest)
        {
            var list = globalEnvironment.DbContext.Clients 
                .AsNoTracking()
                .FilterClients(filter)
                .ProjectTo<ClientDVRow>(globalEnvironment.Mapper.ConfigurationProvider)
                .OrderClients(dataViewRequest.SortName, dataViewRequest.SortAsc);

            var totalCount = list.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)dataViewRequest.PageSize);



            var data = await list.Skip((dataViewRequest.PageIndex - 1) * dataViewRequest.PageSize).Take(dataViewRequest.PageSize).ToListAsync();
            DataViewResult<ClientDVRow> result = new DataViewResult<ClientDVRow>() { Data = data, TotalCount = totalCount, TotalPages = totalPages };
            return result;
        }

        /// <summary>
        /// Return Client by Id
        /// </summary>
        /// <param name="webEnvironment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Client?> GetClientAsync(GlobalEnvironment globalEnvironment, int id)
        {
            return await globalEnvironment.DbContext.Clients.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Insert or Update Client
        /// (Id = 0 - insert, Id > 0 - update)
        /// </summary>
        /// <param name="webEnvironment"></param>
        /// <param name="trip"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<SaveResultWithValidation> SaveAsync(GlobalEnvironment globalEnvironment, Client client)
        {
            Client _client;
            if (client.Id == 0) //Insert
            {
                _client = client;
                globalEnvironment.DbContext.Add(_client);
            }
            else //Update
            {
                Client? cl = await globalEnvironment.DbContext.Clients.FirstOrDefaultAsync(t => t.Id == client.Id);
                if (cl == null)
                {
                    throw new NotFoundException();
                }

                cl.CheckConcurrencyCheckWithValue(client.ConcurrencyCheck);

                _client = cl;
                globalEnvironment.Mapper.Map(client, _client);
            }

            
            var validationResult = await globalEnvironment.SaveChangesWithValidationAsync(null);
            SaveResultWithValidation result = new SaveResultWithValidation();
            if (validationResult != null && validationResult.Count > 0)
            {
                result.Success = false;
                result.validationResults = validationResult;
            }
            else
            {
                result.Success = true;
                result.Id = client.Id;
            }
            return result;
        }
    }
}
