using API_JWT.Application.Domain;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace API_JWT.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDistributedCache _redis;
        public UserRepository(IDistributedCache redis)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        }

        public async Task<User> Add(User user)
        {
            //AQUI SE INFORMA O USERNAME COMO UMA "CHAVE", e Passa o objeto em JSON
            await _redis.SetStringAsync(user.Username, JsonSerializer.Serialize(user));
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            //BUSCA O OBJETO COM BASE NA "CHAVE" USERNAME
            var data = await _redis.GetStringAsync(username);
            //VERIFICA SE TROUXE ALGUM VALOR NA VAR DATA
            if (string.IsNullOrEmpty(data))
                return null;

            //CONSEGUE CONVERTER PARA UM OBJETO PARA SER UTILIZADO
            var user = JsonSerializer.Deserialize<User>(data);

            return user;
        }
    }
}
