namespace eCommerce.Core.Abstractions.Constants;
public class FileSettings
{
    public const int MaxInMB = 5;
    public const int MaxInBytes = MaxInMB * 1024 * 1024;
    public const string _imagePath = "uploads/images";

    public static readonly string[] AllowedExtension = [".jpg", ".jpeg", ".png"];
}
