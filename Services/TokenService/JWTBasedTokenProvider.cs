﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scheme.Services.TokenService
{
    public class JWTBasedTokenProvider : ITokenAuthProvider
    {
        private ProjectContext _db;

        public JWTBasedTokenProvider(ProjectContext db)
        {
            _db = db;
        }

        async public Task<ClaimsIdentity> GetIdentityAsync(User user, Role role = null)
        {
            var roles = await _db.Roles.AsNoTracking().Where(x => x.User.Id == user.Id).ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, 
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        async public Task<AccessTokenResult> GetTokenAsync(User user, Role role = null)
        {
            var identity = await GetIdentityAsync(user, role);

            if (identity == null) return null;

            var now = DateTime.UtcNow;

            var expire = now.AddDays(AuthOptions.LIFETIME);

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: expire,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                                                               SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new AccessTokenResult(
                token: encodedJwt,
                username: identity.Name,
                expireDate: expire);

            return response;
        }
    }
}
