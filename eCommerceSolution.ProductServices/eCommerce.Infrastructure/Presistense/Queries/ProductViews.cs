namespace eCommerce.Infrastructure.Presistense.Queries;
public class ProductViews
{
    public const string GetAllProductWithImageViewName = "public.get_products_with_images";
    public static string GetAllProductWithImageView 
    {
        get
        {
            var query = @"
                DROP VIEW IF EXISTS public.get_products_with_images;
                CREATE OR REPLACE VIEW public.get_products_with_images AS 
                    SELECT 
                        P.""Id"", 
                        P.""Name"", 
                        P.""Description"",
                        P.""Price"",
                        P.""CreatedAt"",
                        P.""CreatedBy"",
                        P.""IsActive"",
                        P.""UpdatedAt"",
                        P.""CategoryId"",
                        PI.""Id"" as ImageId,
                        PI.""Name"" AS ImageName,
                        PI.""CreatedAt"" AS ImageCreatedAt
                    FROM 
                        public.""Products"" AS P 
                    LEFT JOIN 
                        public.""ProductImages"" AS PI 
                    ON 
                        P.""Id"" = PI.""ProductId"";";

            return query;
        }
    }


}
public class ProductFunctions
{
    public const string GetProductWithImagesFunctionName = "public.get_products_with_images";
    public static string GetProductsWithImagesFunction
        => @"
            DROP FUNCTION IF EXISTS public.get_products_with_images; 
            CREATE OR REPLACE FUNCTION public.get_products_with_images(
                seller_id TEXT DEFAULT NULL
            )
            RETURNS TABLE (
                ""Id"" UUID,
                ""Name"" TEXT,
                ""Description"" TEXT,
                ""Price"" DECIMAL,
                ""CreatedAt"" TIMESTAMP WITH TIME ZONE,
                ""CreatedBy"" TEXT,
                ""CategoryId"" UUID,
                ""IsActive"" BOOLEAN,
                ""UpdatedAt"" TIMESTAMP WITH TIME ZONE,
                ""ImageId"" UUID,
                ""ImageName"" TEXT,
                ""ImageCreatedAt"" TIMESTAMP WITH TIME ZONE
            ) AS $$
            BEGIN
                RETURN QUERY
                SELECT 
                    P.""Id"", 
                    P.""Name"", 
                    P.""Description"",
                    P.""Price"",
                    P.""CreatedAt"",
                    P.""CreatedBy"",
                    P.""CategoryId"",
                    P.""IsActive"",
                    P.""UpdatedAt"",
                    PI.""Id"" AS ImageId,
                    PI.""Name"" AS ImageName,
                    PI.""CreatedAt"" AS ImageCreatedAt
                FROM 
                    public.""Products"" AS P 
                LEFT JOIN 
                    public.""ProductImages"" AS PI 
                ON 
                    P.""Id"" = PI.""ProductId""
                WHERE 
                    seller_id IS NULL OR P.""CreatedBy"" = seller_id;
            END;
            $$ LANGUAGE plpgsql;";

    public const string GetProductByIdFunctionName = "public.get_products_by_id";
    public static string GetProductByIdFunction
        => @"
            DROP FUNCTION IF EXISTS public.get_products_by_id;
            CREATE OR REPLACE FUNCTION public.get_products_by_id(
                product_id UUID
            )
            RETURNS TABLE (
                ""Id"" UUID,
                ""Name"" TEXT,
                ""Description"" TEXT,
                ""Price"" DECIMAL,
                ""CreatedAt"" TIMESTAMP WITH TIME ZONE,
                ""CreatedBy"" TEXT,
                ""CategoryId"" UUID,
                ""IsActive"" BOOLEAN,
                ""UpdatedAt"" TIMESTAMP WITH TIME ZONE,
                ""ImageId"" UUID,
                ""ImageName"" TEXT,
                ""ImageCreatedAt"" TIMESTAMP WITH TIME ZONE
            ) AS $$
            BEGIN
                RETURN QUERY
                SELECT 
                    P.""Id"", 
                    P.""Name"", 
                    P.""Description"",
                    P.""Price"",
                    P.""CreatedAt"",
                    P.""CreatedBy"",
                    P.""CategoryId"",
                    P.""IsActive"",
                    P.""UpdatedAt"",
                    PI.""Id"" AS ImageId,
                    PI.""Name"" AS ImageName,
                    PI.""CreatedAt"" AS ImageCreatedAt
                FROM 
                    public.""Products"" AS P 
                LEFT JOIN 
                    public.""ProductImages"" AS PI 
                ON 
                    P.""Id"" = PI.""ProductId""
                WHERE 
                    P.""Id"" = product_id;
            END;
            $$ LANGUAGE plpgsql;";
}