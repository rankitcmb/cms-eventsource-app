using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Server.WebApi.Core.Entity;
using Dapper;

namespace CMS.Core.Server.WebApi.Persistence.MSSQL.Repository
{
    public class MemberRepository : IRepository<Member,int>
    {
        private readonly string _connectionString;
        public MemberRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public Task<Member> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Member>> GetAllAsync()
        {
            await using var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();

            return dbConnection.QueryAsync<Member>("SELECT * FROM Member").Result.ToList();
        }

        public async Task<int> AddAsync(Member entity)
        {
            await using var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();

            await using var transaction = dbConnection.BeginTransaction();

            try
            {
                var rows = await dbConnection.ExecuteAsync("usp_InsertMember", entity, transaction, null, CommandType.StoredProcedure);

                transaction.Commit();
                return rows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> UpdateAsync(Member entity)
        {
            await using var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();

            await using var transaction = dbConnection.BeginTransaction();

            try
            {
                var rows = await dbConnection.ExecuteAsync("usp_UpdateMember"
                    , new { entity.FirstName
                        , entity.LastName
                        , entity.Dbo
                        , entity.Email
                        , entity.Gender
                        , entity.Mobile
                        , entity.AggregateId }
                    , transaction
                    , null
                    , CommandType.StoredProcedure);

                transaction.Commit();
                return rows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
