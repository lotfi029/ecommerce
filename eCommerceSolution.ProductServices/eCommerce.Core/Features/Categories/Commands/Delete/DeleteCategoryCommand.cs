namespace eCommerce.Core.Features.Categories.Commands.Delete;
public record DeleteCategoryCommand(string UserId, Guid Id) : ICommand;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken ct)
    {
        try
        {
            var result = await categoryRepository.DeleteAsync(command.Id, ct);
            return result == 0
                ? CategoryErrors.CategoryNotFound
                : Result.Success();
        }
        catch
        { 
            return CategoryErrors.FailedOperation;
        }
    }
}
