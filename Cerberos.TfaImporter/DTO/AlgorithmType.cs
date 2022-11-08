using System.ComponentModel.DataAnnotations;

namespace Cerberos.TfaImporter.DTO;

public enum AlgorithmType
{
    [Display(Name="UNSPECIFIED")]
    Unspecified = 0,
    
    [Display(Name="SHA1")]
    Sha1 = 1,
    
    [Display(Name="SHA256")]
    Sha256 = 2,
    
    [Display(Name="SHA512")]
    Sha512 = 3,
    
    [Display(Name="MD5")]
    Md5 = 4
}