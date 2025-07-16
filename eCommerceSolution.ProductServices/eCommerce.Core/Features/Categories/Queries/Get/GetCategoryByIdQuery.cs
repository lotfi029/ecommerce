using eCommerce.Core.DTO.Categories;

namespace eCommerce.Core.Features.Categories.Queries.Get;
public record GetCategoryByIdQuery(string UserId, Guid Id) : IQuery<CategoryResponse>;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) : IQueryHandler<GetCategoryByIdQuery ,CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        try
        {
            var category = await categoryRepository.GetByIdAsync(query.Id, ct);

            if (category is null)
                return CategoryErrors.CategoryNotFound;

            var response = category.Adapt<CategoryResponse>();
            return response;
        }
        catch
        {
            return CategoryErrors.CategoryNotFound;
        }
    }
}
