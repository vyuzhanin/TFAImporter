using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cerberos.TfaImporter.DTO;
using Cerberos.TfaImporter.DTO.Proto;
using SimpleBase;

namespace Cerberos.TfaImporter.Models;

public class ProtobufService
{
    private readonly BarcodeService _barcodeService = new BarcodeService();
    private static string ToQueryString(IDictionary<string, string> parameters) =>
        string.Join("&", parameters.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));
    
    private IEnumerable<DecodedTokenDto> ExtractTokens(MigrationPayload mp)
    {
        return mp.OtpParameters.Select(otpParam => 
            new DecodedTokenDto(
                (TokenType)(int)otpParam.Type,
                Base32.Rfc4648.Encode(otpParam.Secret.ToByteArray(), padding: false),
                otpParam.Name,
                otpParam.Issuer, 
                (otpParam.Digits == MigrationPayload.Types.DigitCount.Eight) ? 8  : 6,
                (AlgorithmType)(int)otpParam.Algorithm, 
                otpParam.Type == MigrationPayload.Types.OtpType.Hotp ? otpParam.Counter : null,
                otpParam.Type == MigrationPayload.Types.OtpType.Totp ? (otpParam.Period > 0 ? otpParam.Period : null) : null));
    }
    public string GenerateOauthPathUrl(DecodedTokenDto decodedToken)
    {
        var queryParams = new Dictionary<string, string>();
        queryParams.Add("secret",decodedToken.Secret);
        queryParams.Add("issuer",decodedToken.Issuer);
        queryParams.Add("digits", decodedToken.Digits.ToString());
        queryParams.Add("algorithm", decodedToken.Alg.GetEnumDisplayName());
        
        if(decodedToken.Type == TokenType.Hotp)
            queryParams.Add("counter", decodedToken.Counter.ToString());
        else if(decodedToken.Type == TokenType.Totp && decodedToken.Period.HasValue)
            queryParams.Add("period", decodedToken.Period.ToString());
        
        var result = $"otpauth://{(decodedToken.Type == TokenType.Totp ? "totp" : "hotp")}/{HttpUtility.UrlEncode(decodedToken.Name)}?{ToQueryString(queryParams)}";
        return result;

    }

    public async Task<Tuple<string, IEnumerable<DecodedTokenDto>>> ReceiveTokensAsync(Stream barcodeStream)
    {
        var barcodeResult = await _barcodeService.DecodeBarcodeAsync(barcodeStream);

        var encodedUrl = barcodeResult?? string.Empty;

        if (!string.IsNullOrWhiteSpace(encodedUrl) && encodedUrl.StartsWith("otpauth-migration://offline"))
        {
            var query = HttpUtility.UrlDecode(encodedUrl);
            var parsed = HttpUtility.ParseQueryString(query);

            if (parsed?.AllKeys?.Any(obj => obj.EndsWith("data")) == true)
            {
                var data = parsed[0];
            
                var base64EncodedBytes = Convert.FromBase64String(data);

                var mp = MigrationPayload.Parser.ParseFrom(base64EncodedBytes);

                var tokens = ExtractTokens(mp);

                return Tuple.Create<string, IEnumerable<DecodedTokenDto>>(encodedUrl, tokens);
            }
            throw new InvalidOperationException("Decoded");
        }
        throw new InvalidOperationException("Decoded value is empty or has an invalid format.");

    }
}