namespace Cerberos.TfaImporter.DTO;

/// <summary>
/// Decoded Token Type
/// </summary>
public enum TokenType 
{
    /// <summary>
    /// Unspecified
    /// </summary>
    Unspecified = 0,
    
    /// <summary>
    /// HOTP
    /// </summary>
    Hotp = 1,
    
    /// <summary>
    /// TOTP
    /// </summary>
    Totp = 2,
}