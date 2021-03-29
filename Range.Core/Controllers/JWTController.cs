using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Range.Core.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class JWTController : ControllerBase
  {
    public string Issure { get; set; } = "range.chen@quectel.com";

    /// <summary>
    /// 获取jwtToken
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<string> GetJWTToken()
    {
      IEnumerable<Claim> claims = new List<Claim>()
      {
        new Claim(ClaimTypes.Role,"Admin"),
        new Claim(ClaimTypes.Role, "User"),
        new Claim(ClaimTypes.Name,"Chen")
    };
      //安全密钥
      SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Issure));
      //签名令牌
      SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      //Token
      SecurityToken securityToken = new JwtSecurityToken(
        issuer: Issure,
        audience: Issure,
        claims: claims,
        signingCredentials: signingCredentials,
        expires: DateTime.Now.AddHours(1)
      );
      //生成的token
      var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

      return jwtToken;
    }
  }
}
