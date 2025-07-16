using eCommerceCatalogService.Core.Errors;
using eCommerceCatalogService.Core.IRepositories;

namespace eCommerceCatalogService.Core.Features.Catalog.Commands.Delete;

public record DeleteCatalogProductCommand(Guid Id) : ICommand;

public class DeleteCatalogProductCommandHandler(
    ICatalogRepository _catalogProductRepository) : ICommandHandler<DeleteCatalogProductCommand>
{
    public async Task<Result> HandleAsync(DeleteCatalogProductCommand command, CancellationToken ct = default)
    {
        if (!await _catalogProductRepository.ExistsAsync(command.Id, ct))
            return CatalogProductErrors.NotFound;

        await _catalogProductRepository.DeleteAsync(command.Id, ct);
        return Result.Success();
    }
}