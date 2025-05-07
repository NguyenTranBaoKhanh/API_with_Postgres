using API_with_Postgres.Business.Interfaces;
using API_with_Postgres.Common.Response;
using API_with_Postgres.Models;
using API_with_Postgres.Repositories;

namespace API_with_Postgres.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        public async Task<CommonResponse<IEnumerable<User>>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<CommonResponse<User>> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }

        public async Task<CommonResponse<IEnumerable<User>>> GetUsersByEmail(string email)
        {
            return await _userRepository.GetUsersByEmail(email);
        }
    }
}
