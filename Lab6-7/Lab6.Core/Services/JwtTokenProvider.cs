using Lab6.Core.Auth;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using JsonWebToken = Lab6.Core.Auth.JsonWebToken;

namespace Lab6.Core.Services;

public sealed class JwtTokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtTokenGenerator _tokenGenerator;

    public JwtTokenProvider(
        JwtOptions jwtOptions,
        JwtTokenGenerator tokenGenerator)
    {
        _jwtOptions = jwtOptions;
        _tokenGenerator = tokenGenerator;
    }

    public JsonWebToken CreateToken(
        string userGid,
        string userName
        )
    {
        var now = DateTime.UtcNow;

        var jwtClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userGid),
            new Claim(JwtRegisteredClaimNames.Sub, userGid),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
        };

        if (!string.IsNullOrWhiteSpace(userName))
            jwtClaims.Add(new Claim(ClaimTypes.Name, userName));

        var expires = now.AddMinutes(_jwtOptions.ExpiryMinutes);

        var jwt = _tokenGenerator.GenerateToken(
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            expires,
            jwtClaims);

        return new JsonWebToken(
            jwt,
            expires.ToTimestamp(),
            userGid,
            jwtClaims);
    }
}