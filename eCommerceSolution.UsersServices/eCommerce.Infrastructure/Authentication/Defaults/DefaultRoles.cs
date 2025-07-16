namespace eCommerce.Infrastructure.Authentication.Defaults;
public class DefaultRoles
{
    public const string Admin = "admin";
    public static Guid AdminId = Guid.Parse("c428bf98-d0e5-434d-9e1b-a60a9bea8ce5");
    public const string User = "user";
    public static Guid UserRoleId = Guid.Parse("ec94e0aa-8b98-491f-8eeb-df3e67bc5cbe");
}