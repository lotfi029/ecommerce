using eCommerce.Core.DTO.Categories;
using System.Data;

namespace eCommerce.Core.Features.Categories.Commands.Update;
public record UpdateCategoryCommand(string UserId, Guid Id, CategoryRequest Request) : ICommand;

public class UpdateCategoryCommandRequest(ICategoryRepository categoryRepository) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        if (await categoryRepository.GetByIdAsync(command.Id, ct) is not { } category)
            return CategoryErrors.NotFound;
        if (category.CreatedBy != command.UserId)
            return CategoryErrors.InvalidAccess;

        category = command.Request.Adapt(category);

        try
        {
            await categoryRepository.UpdateAsync(category, ct);
            return Result.Success();
        }
        catch
        {
            return CategoryErrors.FailedOperation;
        }
    }
}
