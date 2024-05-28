using API_JWT.Application.Domain;
using API_JWT.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_JWT.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("creating")]
        [ProducesResponseType(typeof(User),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] User usuarioDTO)
        {
            try
            {
                var user = new User(usuarioDTO.Username, usuarioDTO.Password, usuarioDTO.Name);

                var result = await _userRepository.Add(user); // INSERE NO BANCO

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] User model)
        {
            var user = await _userRepository.GetUserByUsername(model.Username);

            //VERIFICA SE ENCONTROU ALGUM USUÁRIO
            if (user == null)
                return NotFound(new { message = "Usuário Não Encontrado!"});
            
            //VERIFICA A SENHA INFORMADA COM A DO USUÁRIO
            if(user.Password != model.Password)
                return BadRequest(new { message = "Senha Inválida!" });

            //GERA O TOKEN PARA O USUARIO
            var token = TokenService.GenerateToken(user);

            return Ok(new
            {
                Token = token
            });
        }

        [Authorize]
        [HttpPost]
        [Route("creating-authorize")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> PostAuthorize([FromBody] User usuarioDTO)
        { // SÓ VAI RODAR ESSE MÉTODO SE TIVERMOS O TOKEN
            try
            {
                var user = new User(usuarioDTO.Username, usuarioDTO.Password, usuarioDTO.Name);

                var result = await _userRepository.Add(user); // INSERE NO BANCO

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
