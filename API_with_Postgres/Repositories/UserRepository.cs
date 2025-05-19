using API_with_Postgres.Common.Response;
using API_with_Postgres.Data;
using API_with_Postgres.Models;
using Common.Library.Collection.Dapper;
using Dapper;

namespace API_with_Postgres.Repositories
{
    public interface IUserRepository
    {
        Task<CommonResponse<IEnumerable<User>>> GetAll();
        Task<CommonResponse<User>> CreateUser(User user);
        Task<CommonResponse<IEnumerable<User>>> GetUsersByEmail(string email);
        Task<CommonResponse<IEnumerable<User>>> GetUsersById(int id);
    }

    public class UserRepository : BaseRepository_Old, IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<CommonResponse<IEnumerable<User>>> GetAll()
        {
            var query = @"SELECT * FROM ""Users""";
            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS ""Users"" (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""Name"" TEXT NOT NULL,
                    ""Email"" TEXT NOT NULL
                );
            ";

            await _context.CreateConnection().ExecuteAsync(createTableQuery);
            using (var connection = _context.CreateConnection())
            {
                var users = await QueryExecuteAsync<User>(connection, query);
                return new CommonResponse<IEnumerable<User>>
                {
                    Success = true,
                    Payload = users
                };
            }
        }

        public async Task<CommonResponse<User>> CreateUser(User user)
        {
            var query = @"INSERT INTO ""Users"" (""Name"", ""Email"") VALUES (@Name, @Email) RETURNING ""Id""";
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(query, user);
                user.Id = id;
                return new CommonResponse<User>
                {
                    Success = true,
                    Payload = user
                };
            }
        }

        public async Task<CommonResponse<IEnumerable<User>>> GetUsersByEmail(string email)
        {
            var query = @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email";
            using (var connection = _context.CreateConnection())
            {
                var users = await QueryExecuteAsync<User>(connection, query, new { Email = email });
                return new CommonResponse<IEnumerable<User>>
                {
                    Success = true,
                    Payload = users
                };
            }
        }

        public async Task<CommonResponse<IEnumerable<User>>> GetUsersById(int id)
        {
            var query = @"SELECT * FROM ""Users"" WHERE ""Id"" = @Id";
            using (var connection = _context.CreateConnection())
            {
                var users = await QueryExecuteAsync<User>(connection, query, new { Id =  id });
                return new CommonResponse<IEnumerable<User>>
                {
                    Success = true,
                    Payload = users
                };
            }
        }
    }
}
