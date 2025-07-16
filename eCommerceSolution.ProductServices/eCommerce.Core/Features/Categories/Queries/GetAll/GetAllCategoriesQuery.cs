using eCommerce.Core.DTO.Categories;

namespace eCommerce.Core.Features.Categories.Queries.GetAll;
public record GetAllCategoriesQuery(string UserId) : IQuery<IEnumerable<CategoryResponse>>;

public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository) : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
{
    public async Task<Result<IEnumerable<CategoryResponse>>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
    {
        var categories = await categoryRepository.GetAllAsync(ct);

        var response = categories.Adapt<IEnumerable<CategoryResponse>>();

        return response.ToList();
    }
}
