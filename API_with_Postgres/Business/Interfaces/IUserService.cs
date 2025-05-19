using API_with_Postgres.Common.Response;
using API_with_Postgres.Models;

namespace API_with_Postgres.Business.Interfaces
{
    public interface IUserService
    {
        Task<CommonResponse<IEnumerable<User>>> GetAll();
        Task<CommonResponse<User>> CreateUser(User user);
        Task<CommonResponse<IEnumerable<User>>> GetUsersByEmail(string email);
        Task<CommonResponse<IEnumerable<User>>> GetUsersById(int id);

    }
}
