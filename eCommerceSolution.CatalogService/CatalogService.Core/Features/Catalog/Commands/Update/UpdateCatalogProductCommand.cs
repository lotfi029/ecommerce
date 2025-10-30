using CatalogService.Core.Errors;
using CatalogService.Core.ExternalContractDTOs;
using CatalogService.Core.IRepositories;
using Mapster;

namespace CatalogService.Core.Features.Catalog.Commands.Update;

public record UpdateCatalogProductCommand(Guid Id, ProductMessageDTO Request) : ICommand;

public class UpdateCatalogProductCommandHandler(ICatalogRepository _catalogProductRepository) : ICommandHandler<UpdateCatalogProductCommand>
{
    public async Task<Result> HandleAsync(UpdateCatalogProductCommand command, CancellationToken ct = default)
    {
        if (await _catalogProductRepository.GetByIdAsync(command.Id, ct) is not { } catalogProduct)
            return CatalogProductErrors.NotFound;

        catalogProduct = command.Request.Adapt(catalogProduct);

        await _catalogProductRepository.UpdateAsync(catalogProduct, ct);

        return Result.Success();
    }
}