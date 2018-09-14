using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Scheme.Entities;
using System;
using Scheme.Services;
using System.Linq;
using Scheme.Models;
using System.Net.Mail;
using Scheme.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Scheme.Services.TokenService;

namespace Scheme.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private ProjectContext _db;
        private CodeGenerator _generator;
        private IEmailSender _sender;
        private ITokenAuthProvider _token;
        public AccountController(ProjectContext context, IEmailSender sender, ITokenAuthProvider provider, CodeGenerator generator)
        {
            _db = context;
            _sender = sender;
            _token = provider;
            _generator = generator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInModel model)
        {
            if (!ModelState.IsValid || model == null) return BadRequest();
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
            if (user == null) return Forbid("User not found!");
            var salt = user.Salt;
            var passHash = user.PassHash;
            var cryptoProvider = new CryptographyProcessor();
            if (!cryptoProvider.AreEqual(model.Password, passHash, salt)) return Forbid("Wrong username or password!");

            if (!user.IsConfirmed) return Unauthorized();
            //TODO: Delete Role
            var token = await _token.GetToken(user, new Role { Name = "_admin", Project = new Project { Id = 5, Name = "Some" }, User = user });
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid || model == null) return BadRequest();
            if (await _db.Users.AnyAsync(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                return BadRequest("This Email is alrady exist!");

            var cryptoProvider = new CryptographyProcessor();

            var salt = cryptoProvider.CreateSalt(AuthOptions.SaltSize);
            var passHash = cryptoProvider.GenerateHash(model.Password, salt);

            var newUser = new User()
            {
                Email = model.Email.ToLower(),
                Salt = salt,
                PassHash = passHash,
                Name = model.Name,
                SurnName = model.Surname
            };


            await SendMailAndGenerateCode(newUser);
            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

            return Ok("Success!");
        }

        [HttpPost("renew")]
        [Authorize]
        public async Task<IActionResult>RenewToken()
        {
            var email = User.Identity.Name;
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return Unauthorized();
            var token = _token.GetToken(user);
            return Ok(token);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> RegistrationCodeCheck([FromBody] string mail, int code)
        {
            if (!ModelState.IsValid) return BadRequest("Wrong Data!");
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(mail, StringComparison.OrdinalIgnoreCase));
            if (user == null) return BadRequest("Wrong email or code!");
            var record = await _db.VerificationCodes.FirstOrDefaultAsync(x => x.Code == code && x.User.Id == user.Id);
            if (record == null) return BadRequest("Wrong email or code!");

            var token = _token.GetToken(user);
            _db.VerificationCodes.Remove(record);
            await _db.SaveChangesAsync();
            return Ok(token);

        }

        [NonAction]
        private async Task SendMailAndGenerateCode(User user)
        {
            _generator.GenerateCode(2);
            var codes = await _db.VerificationCodes.Where(x => x.Id == user.Id).ToListAsync();
            if (codes != null) _db.VerificationCodes.RemoveRange(codes);
            _db.VerificationCodes.Add(new VerificationCode() { Code = _generator.Code, Expires = _generator.ExpireDate, User = user });
            var msg = ConfirmaionMessage(_generator.Code, user.Email);
            await _sender.SendAsync(msg);
        }

        [NonAction]
        private MailMessage ConfirmaionMessage(int code, string address)
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress("samir.itstep@gmail.com", "Samir Mammadli"),
                Subject = "Get your code",
                Body = $@"Your verification code: <b>{code}</b>"
            };
            mail.To.Add(new MailAddress(address));
            return mail;
        }

    }
}