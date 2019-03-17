using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BalticAmadeusTask.Data;
using BalticAmadeusTask.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BalticAmadeusTask.Models;
using BalticAmadeusTask.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Design;

namespace BalticAmadeusTask.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly BalticAmadeusTaskContext _context;
        private readonly IPasswordPolicyService _passwordPolicyService;
        private readonly IHashingService _hashingService;
        private readonly IMapper _mapper;

        public UserApiController(BalticAmadeusTaskContext context, IMapper mapper, IPasswordPolicyService passwordPolicyService, IHashingService hashingService)
        {
            _context = context;
            _mapper = mapper;
            _passwordPolicyService = passwordPolicyService;
            _hashingService = hashingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserModel([FromQuery] UserFiltering userFiltering = null)
        {
            if (userFiltering == null)
            {
                var res = (await _context.UserModel
                    .ToListAsync())
                    .Select(x => _mapper.Map<RegisteredUserD, RegisteredUser>(x)).ToList();
                return Ok(res);
            }
            else
            {
                var res = await _context.UserModel
                    .Where(_ => (string.IsNullOrEmpty(userFiltering.Email) || _.Email.Contains(userFiltering.Email)) &&
                                (string.IsNullOrEmpty(userFiltering.Name) || _.Name.Contains(userFiltering.Name)))
                    .Take<RegisteredUserD>(userFiltering.MaxElements).Select(x=> _mapper.Map<RegisteredUserD, RegisteredUser>(x)).ToListAsync();
                return Ok(res);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserModel(Guid id)
        {
            var registeredUser = await _context.UserModel.FindAsync(id);
            if (registeredUser == null)
            {
                return NotFound();
            }

            var registeredUserD = _mapper.Map<RegisteredUserD, RegisteredUser> (registeredUser);

            return Ok(registeredUserD);
        }

        [HttpPost]
        public async Task<IActionResult> PostUserModel(RegisteredUser registeredUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUserWithEmail = await _context.UserModel.AnyAsync(_ => _.Email == registeredUser.Email);

            if (existingUserWithEmail)
            {
                ModelState.AddModelError("Email", "Email already exists.");
            }

            if (!ModelState.IsValid)
            {
                var error = ModelStateToValidationError.Convert(ModelState);
                return BadRequest(error);
            }

            registeredUser.Password = _hashingService.GenerateHashString(registeredUser.Password);

            RegisteredUserD registeredUserD = _mapper.Map<RegisteredUser, RegisteredUserD>(registeredUser);

            _context.UserModel.Add(registeredUserD);
            await _context.SaveChangesAsync();
            registeredUser = _mapper.Map<RegisteredUserD, RegisteredUser>(registeredUserD);
            return CreatedAtAction("GetUserModel", new { id = registeredUser.Id }, registeredUser);
        }
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel([FromRoute] Guid id, [FromBody] RegisteredUser registeredUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registeredUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(registeredUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();

            return Ok(userModel);
        }

        private bool UserModelExists(Guid id)
        {
            return _context.UserModel.Any(e => e.Id == id);
        }
        */
    }
}