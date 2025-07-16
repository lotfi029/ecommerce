using System.ComponentModel.DataAnnotations;

namespace eCommerce.Infrastructure.RabbitMQ;
public class RabbitMQOptions
{
    public static string SectionsName => nameof(RabbitMQOptions);
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
