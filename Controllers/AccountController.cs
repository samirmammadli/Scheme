﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Scheme.Entities;
using System;
using Scheme.Services;
using Scheme.Models;
using System.Net.Mail;
using Scheme.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Scheme.Services.TokenService;
using Scheme.InputForms.Account;
using System.Linq;
using System.Collections.Generic;

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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LogInForm model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest(ModelState);

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return BadRequest(ControllerErrorCode.AccountOrPasswordWrong);

            var salt = user.Salt;

            var passHash = user.PassHash;

            var cryptoProvider = new CryptographyProcessor();

            if (!cryptoProvider.AreEqual(model.Password, passHash, salt))
                return BadRequest(ControllerErrorCode.AccountOrPasswordWrong);

            if (!user.IsConfirmed)
                return BadRequest(ControllerErrorCode.NotConfirmed);

            var token = await _token.GetTokenAsync(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationForm model)
        {
            if (!ModelState.IsValid || model == null) return BadRequest();

            if (await _db.Users.AnyAsync(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                return BadRequest(ControllerErrorCode.EmailAlreadyExists);

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

            await _db.Users.AddAsync(newUser);

            await _db.SaveChangesAsync();

            await SendMailAndGenerateCode(newUser);

            return Ok("Success!");
        }

        [HttpPost]
        [Authorize]
        [Route("token/renew/")]
        public async Task<IActionResult>RenewToken()
        {
            var email = User.Identity.Name;

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return Unauthorized();

            var token = await _token.GetTokenAsync(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("registration/confirm/")]
        public async Task<IActionResult> RegistrationCodeCheck([FromBody] RegCodeCheckForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(form.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return NotFound(ControllerErrorCode.AccountNotFound);

            if (user.IsConfirmed)
                return BadRequest(ControllerErrorCode.NotConfirmed);

            var record = await _db.VerificationCodes.FirstOrDefaultAsync(x => x.Code == form.Code && x.User.Id == user.Id);

            if (record == null )
                return BadRequest(ControllerErrorCode.WrongRegCode);

            if (record.Expires <= DateTime.UtcNow)
                return BadRequest(ControllerErrorCode.ExpiredCode);

            var token = await _token.GetTokenAsync(user);

            _db.VerificationCodes.Remove(record);

            user.IsConfirmed = true;

            _db.Update(user);

            await _db.SaveChangesAsync();

            return Ok(token);
        }

        [HttpPost]
        [Route("password/reset/")]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePassByCodeForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return BadRequest(ControllerErrorCode.AccountNotFound);

            var code = await _db.ForgotCodes.FirstOrDefaultAsync(x => x.Code == form.Code);

            if (code == null)
                return BadRequest(ControllerErrorCode.WrongRegCode);

            if (code.ExpireDate < DateTime.UtcNow)
                return BadRequest(ControllerErrorCode.ExpiredCode);

            var cryptoProvider = new CryptographyProcessor();

            var salt = cryptoProvider.CreateSalt(AuthOptions.SaltSize);

            var passHash = cryptoProvider.GenerateHash(form.Password, salt);

            user.PassHash = passHash;

            user.Salt = salt;

            await _db.SaveChangesAsync();

            return Ok();

        }

        [HttpPost]
        [Route("password/forgot/")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailInputForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return BadRequest(ControllerErrorCode.AccountNotFound);

            var code = await _db.ForgotCodes.FirstOrDefaultAsync(x => x.User == user);

            if (code != null)
                _db.ForgotCodes.Remove(code);

            _generator.GenerateCode(1);

            var newCode = new ForgotPassCode
            {
                User = user,
                Code = _generator.Code,
                ExpireDate = _generator.ExpireDate
            };

            await _db.ForgotCodes.AddAsync(newCode);

            await _db.SaveChangesAsync();

            var msg = ComposeMessage(user.Email, "Forgot Password", $@"Your reset code: <b>{_generator.Code}</b>");

            await _sender.SendAsync(msg);

            return Ok("Success");
        }

        [HttpPost]
        [Route("registration/resend_code/")]
        public async Task<IActionResult> ResendCode([FromBody] EmailInputForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return NotFound();

            if (user.IsConfirmed)
                return BadRequest(ControllerErrorCode.AlreadyConfirmed);

            await SendMailAndGenerateCode(user);

            return Ok();
        }

        [NonAction]
        private async Task SendMailAndGenerateCode(User user)
        {
            _generator.GenerateCode(2);

            var codes = await _db.VerificationCodes.Where(x => x.Id == user.Id).ToListAsync();

            if (codes != null)
                _db.VerificationCodes.RemoveRange(codes);

            _db.VerificationCodes.Add(new VerificationCode()
            {
                Code = _generator.Code,
                Expires = _generator.ExpireDate,
                User = user
            });

            await _db.SaveChangesAsync();

            var msg = ComposeMessage(user.Email, "Registration Confirmation", $@"Your verification code: <b>{_generator.Code}</b>");

            await _sender.SendAsync(msg);
        }

        [NonAction]
        private MailMessage ComposeMessage(string address, string subject, string message)
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress("samir.itstep@gmail.com", "Samir Mammadli"),
                Subject = subject,
                Body = message
            };

            mail.To.Add(new MailAddress(address));

            return mail;
        }

    }
}