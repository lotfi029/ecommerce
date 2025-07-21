using System.ComponentModel.DataAnnotations;

namespace InventoryService.Infrastructure.Authentication;
public class JwtSettings
{
    public const string SectionName = nameof(JwtSettings);
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; set; }
}
