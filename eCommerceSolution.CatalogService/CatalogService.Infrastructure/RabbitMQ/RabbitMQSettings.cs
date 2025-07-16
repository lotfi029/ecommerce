using System.ComponentModel.DataAnnotations;

namespace eCommerceCatalogService.Infrastructure.RabbitMQ;
public class RabbitMQSettings
{
    public static string SectionsName => nameof(RabbitMQSettings);
    [Required]
    [MinLength(3)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MinLength(3)]
    public string Password { get; set; } = string.Empty;
    [Required]
    [MinLength(3)]
    public string HostName { get; set; } = string.Empty;
    [Required]
    public int Port { get; set; }
}
