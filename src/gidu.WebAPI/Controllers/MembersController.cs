using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gidu.Domain.Core.Logs;
using gidu.Domain.Core.Members;
using gidu.Domain.Core.Photos;
using gidu.Domain.Helpers;

namespace gidu.WebAPI.Controllers
{
    [Route("v1/members")]
    [ApiController]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly MemberRegister _memberRegister;
        private readonly PhotoMember _photoMember;

        public MembersController(IMemberRepository memberRepository, MemberRegister memberRegister, PhotoMember photoMember)
        {
            _memberRepository = memberRepository;
            _memberRegister = memberRegister;
            _photoMember = photoMember;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var members = await _memberRepository.GetLastMembersChangeAsync();
            var membersDto = members.Select(u => Mapper.Map<MemberDto>(u)).ToList();

            return Ok(membersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            var memberDto = Mapper.Map<MemberDto>(member);

            return Ok(memberDto);
        }

        [HttpGet("getBySearchField/{search}")]
        public async Task<IActionResult> GetBySearchFieldAsync(string search)
        {
            var members = await _memberRepository.GetBySearchFieldAsync(search);
            var membersDto = members.Select(u => Mapper.Map<MemberDto>(u)).ToList();

            return Ok(membersDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(MemberDto memberDto)
        {
            memberDto.User = User.FindFirst(ClaimTypes.Name)?.Value; ;
            memberDto.LogOperation = LogOperation.INSERT;

            // await _memberRegister.StoreAsync(memberDto);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, MemberDto memberDto)
        {
            memberDto.Id = id;
            memberDto.User = User.FindFirst(ClaimTypes.Name)?.Value; ;
            memberDto.LogOperation = LogOperation.UPDATE;

            // await _memberRegister.StoreAsync(memberDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = User.FindFirst(ClaimTypes.Name)?.Value;
            await _memberRegister.DeleteAsync(id, user);

            return Ok();
        }

        [HttpPost("{id}/uploadPhoto")]
        public async Task<IActionResult> UploadPhotoAsync(string id)
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            var file = Request.Form.Files[0];
            var photoMemoryStream = new MemoryStream();

            file.CopyTo(photoMemoryStream);

            await _photoMember.UploadPhotoAsync(photoMemoryStream, id, usuario);

            return Ok();
        }

        [HttpGet("getPhoto/{key}")]
        public async Task<IActionResult> GetPhoto(string key)
        {
            Stream photo = await _photoMember.GetPhotoAsync(key, GlobalParameters.FolderPhotoMembers);

            return File(photo, "image/jpeg");
        }
    }
}
