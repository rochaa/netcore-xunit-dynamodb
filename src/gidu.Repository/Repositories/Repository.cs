using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using gidu.Domain.Helpers;

namespace gidu.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly GiduContext _context;
        private DynamoDBOperationConfig _config;

        public Repository(GiduContext context, string table)
        {
            _context = context;
            _config = new DynamoDBOperationConfig()
            {
                OverrideTableName = table,
            };
        }

        protected async Task<List<TEntity>> GetByFiltersAsync(IEnumerable<ScanCondition> filter)
        {
            return await _context.ScanAsync<TEntity>(filter, _config).GetNextSetAsync();
        }

        protected async Task<List<TEntity>> GetByFiltersAsync(IEnumerable<ScanCondition> filter, int limit)
        {
            ScanOperationConfig scanConfig = new ScanOperationConfig()
            {
                Limit = limit
            };
            _config.QueryFilter = filter != null ? filter.ToList() : null;

            return await _context.FromScanAsync<TEntity>(scanConfig, _config).GetNextSetAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await GetByFiltersAsync(null);
        }

        public async Task<List<TEntity>> GetAllAsync(int limit)
        {
            return await GetByFiltersAsync(null, limit);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return await _context.LoadAsync<TEntity>(id, _config);
        }

        public async Task<List<TEntity>> GetBySearchFieldAsync(string search)
        {
            List<ScanCondition> filters = new List<ScanCondition> {
                new ScanCondition ("Search", ScanOperator.Contains, search.ToLower().RemoveDiacritics())
            };

            return await GetByFiltersAsync(filters);
        }

        public async Task SaveAsync(TEntity item)
        {
            if (string.IsNullOrEmpty(item.Id))
                item.Id = Guid.NewGuid().ToString();

            await _context.SaveAsync(item, _config);
        }

        public async Task DeleteAsync(TEntity item)
        {
            await _context.DeleteAsync(item, _config);
        }
    }
}