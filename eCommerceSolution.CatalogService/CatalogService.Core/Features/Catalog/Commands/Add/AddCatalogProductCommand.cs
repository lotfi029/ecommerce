using eCommerceCatalogService.Core.ExternalContractDTOs;
using eCommerceCatalogService.Core.IRepositories;
using Mapster;

namespace eCommerceCatalogService.Core.Features.Catalog.Commands.Add;

public record AddCatalogProductCommand(ProductMessageDTO Request) : ICommand;

public class AddCatalogProductCommandHandler(ICatalogRepository _catalogProductRepository) : ICommandHandler<AddCatalogProductCommand>
{
    public async Task<Result> HandleAsync(AddCatalogProductCommand command, CancellationToken ct = default)
    {
        var catalogProduct = command.Request.Adapt<CatalogProduct>();

        await _catalogProductRepository.AddAsync(catalogProduct, ct);

        return Result.Success();
    }
}
