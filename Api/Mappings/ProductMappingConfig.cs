using System;
using Api.Dtos.Products;
using Application.UseCase.Products;
using Domain.Entities.Products;
using Domain.ValueObjects.Products;
using Mapster;

namespace Api.Mappings;

public sealed class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.Sku, src => src.Sku != null ? src.Sku.Value : string.Empty)
            .Map(dest => dest.Price, src => src.Price.Amount);

        _ = config.NewConfig<CreateProductRequest, CreateProduct>()
            .MapWith(src => new CreateProduct(
                src.Name,
                src.Sku,
                new Money(src.Price, "COP"),
                src.Stock
            ));

        config.NewConfig<UpdateProductRequest, UpdateProduct>()
            .MapWith(src => new UpdateProduct(
                Guid.Empty,
                src.Name,
                src.Sku,
                src.Price,
                src.Stock
            ));
    }
}