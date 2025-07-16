namespace eCommerce.Core.Profiles;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.Images, src => src.Images);
    }
}
