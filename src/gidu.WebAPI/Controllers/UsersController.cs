using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gidu.Domain.Core.Users;
using System.IO;
using gidu.Domain.Core.Photos;
using gidu.Domain.Helpers;

namespace gidu.WebAPI.Controllers
{
    [Route("v1/users")]
    [ApiController]
    [Authorize(Policy = "Administrador")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserRegister _userRegister;
        private readonly UserChangePassword _userChangePassword;
        private readonly PhotoUser _photoUser;

        public UsersController(IUserRepository userRepository, UserRegister userRegister,
            UserChangePassword userChangePassword, PhotoUser photoUser)
        {
            _userRepository = userRepository;
            _userRegister = userRegister;
            _userChangePassword = userChangePassword;
            _photoUser = photoUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var usersDto = users.Select(u => Mapper.Map<UserDto>(u)).ToList();

            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userDto = Mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpGet("getBySearchField/{search}")]
        public async Task<IActionResult> GetBySearchFieldAsync(string search)
        {
            var users = await _userRepository.GetBySearchFieldAsync(search);
            var usersDto = users.Select(u => Mapper.Map<UserDto>(u)).ToList();

            return Ok(usersDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(UserRegisterDto userRegisterDto)
        {
            var userLog = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRegister.StoreAsync(null, userRegisterDto.Name, userRegisterDto.Email,
                userRegisterDto.Password, userRegisterDto.Permission, userLog);
            var userDto = Mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, UserRegisterDto userRegisterDto)
        {
            var userLog = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userRegisterDto.Password))
            {
                var user = await _userRegister.StoreAsync(id, userRegisterDto.Name, userRegisterDto.Email, null, userRegisterDto.Permission, userLog);
                var userDto = Mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            else
            {
                await _userChangePassword.UpdateAsync(id, userRegisterDto.Password, userLog);
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var userLog = User.FindFirst(ClaimTypes.Name)?.Value;
            await _userRegister.DeleteAsync(id, userLog);

            return Ok();
        }

        [HttpPost("{id}/uploadPhoto")]
        public async Task<IActionResult> UploadPhotoAsync(string id)
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            var file = Request.Form.Files[0];

            using (var photoMemoryStream = new MemoryStream())
            {
                file.CopyTo(photoMemoryStream);
                await _photoUser.UploadPhotoAsync(photoMemoryStream, id, usuario);
            }

            return Ok();
        }

        [HttpGet("getPhoto/{key}")]
        public async Task<IActionResult> GetPhoto(string key)
        {
            Stream photo = await _photoUser.GetPhotoAsync(key, GlobalParameters.FolderPhotoUsers);
            return File(photo, "image/jpeg");
        }
    }
}
