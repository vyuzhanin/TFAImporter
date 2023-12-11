using System.Linq;
using Cerberos.TfaImporter.DTO;

namespace Cerberos.TfaImporter.Parsers;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class OtpAuthParser
{
    public const string SecretParamName = "secret";
    public const string IssuerParamName = "issuer";
    public const string DigitsParamName = "digits";
    public const string AlgorithmParamName = "algorithm";
    public const string PeriodParamName = "period";
    public const string CounterParamName = "counter";

    public const string OtpauthSchemaName = "otpauth";
    public const string OtpauthMigrationSchemaName = "otpauth-migration";
    
    public static DecodedTokenDto Parse(string otpAuthUri)
    {
        // Parse the URI scheme and query parameters
        Uri uri = new Uri(otpAuthUri);
        string scheme = uri.Scheme;
        string query = uri.Query;

        // Check if the URI scheme is otpauth
        if (scheme != OtpauthSchemaName)
        {
            throw new ArgumentException("Invalid OTPAuth URI");
        }

        // Parse the OTPAuth type and label from the URI path
        string type = uri.Authority;
        string label = Uri.UnescapeDataString(uri.AbsolutePath.Substring(1));

        // Parse the query parameters
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        foreach (string parameter in query.TrimStart('?').Split('&'))
        {
            string[] keyValue = parameter.Split('=');
            if (keyValue.Length == 2)
            {
                string key = Uri.UnescapeDataString(keyValue[0]);
                string value = Uri.UnescapeDataString(keyValue[1]);
                parameters[key] = value;
            }
        }

        var secretParam = parameters.Where(obj => obj.Key.Equals(SecretParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).First();
        var issuerParam = parameters.Where(obj => obj.Key.Equals(IssuerParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).FirstOrDefault(); 
        int digitsParam = int.TryParse(parameters.Where(obj => obj.Key.Equals(DigitsParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).FirstOrDefault(), out digitsParam) ? digitsParam : 6;
        
        var algorithmSourceParam = parameters.Where(obj => obj.Key.Equals(AlgorithmParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).FirstOrDefault() ?? string.Empty;

        var algorithmTypeParam = algorithmSourceParam.ParseDisplayNameToEnum(AlgorithmType.Sha1);
        
        int periodParam = int.TryParse(parameters.Where(obj => obj.Key.Equals(PeriodParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).FirstOrDefault(), out periodParam) ? periodParam : 30;
        
        long counterParam = long.TryParse(parameters.Where(obj => obj.Key.Equals(CounterParamName, StringComparison.CurrentCultureIgnoreCase))
            .Select(obj => obj.Value).FirstOrDefault(), out counterParam) ? counterParam : 0;

        var tokenTypeParam = type.ParseDisplayNameToEnum(TokenType.Totp);

        var result = new DecodedTokenDto(tokenTypeParam,
            secretParam, label, issuerParam, digitsParam, algorithmTypeParam,
            TokenType.Hotp == tokenTypeParam
                ? counterParam
                : null, TokenType.Totp == tokenTypeParam ? periodParam : null);

        // Create and return the OTPAuth object
        return result;
    }
}