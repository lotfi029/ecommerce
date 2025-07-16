using eCommerce.Core.DTO.Categories;

namespace eCommerce.Core.Features.Categories.Commands.Add;
public record AddCategoryCommand(string UserId, CategoryRequest Request) : ICommand<Guid>;

public class AddCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<AddCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddCategoryCommand command, CancellationToken ct)
    {
        var category = command.Request.Adapt<Category>();
        category.CreatedBy = command.UserId;

        try
        {
            var response = await categoryRepository.AddAsync(category, ct);
            return Result.Success(response);
        } catch
        {
            return Error.BadRequest("Category.AddCategory", "adding operation is failed.");
        }
    }
}