using System.ComponentModel.DataAnnotations;

namespace Cerberos.TfaImporter.DTO;

/// <summary>
/// Decoded Token Type
/// </summary>
public enum TokenType 
{
    /// <summary>
    /// Unspecified
    /// </summary>
    [Display(Name="none")]
    Unspecified = 0,
    
    /// <summary>
    /// HOTP
    /// </summary>
    [Display(Name="hotp")]
    Hotp = 1,
    
    /// <summary>
    /// TOTP
    /// </summary>
    [Display(Name="totp")]
    Totp = 2,
}