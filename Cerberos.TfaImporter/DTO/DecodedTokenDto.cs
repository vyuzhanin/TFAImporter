namespace Cerberos.TfaImporter.DTO;

/// <summary>
/// Provides decoded OTP token
/// </summary>
/// <param name="Secret">Secret Key</param>
/// <param name="Name">Token Name</param>
/// <param name="Type">OTP Type</param>
public record DecodedTokenDto(TokenType Type, string Secret, string Name, string Issuer, int Digits, AlgorithmType Alg,
    long? Counter = null, int? Period = null);