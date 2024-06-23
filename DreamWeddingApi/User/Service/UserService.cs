using DreamWeddingApi.User.Repository;

namespace DreamWeddingApi.User.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

}