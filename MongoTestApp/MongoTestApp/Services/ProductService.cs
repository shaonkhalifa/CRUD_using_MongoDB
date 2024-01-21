using MongoDB.Driver;
using MongoTestApp.Entity;
using MongoTestApp.Interface;

namespace MongoTestApp.Services;

public class ProductService
{
    private readonly IRepository<Product> _repository;

    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetProducts()
    {
        var builder = Builders<Product>.Projection;
        var pro = builder.Exclude(a => a.Id);

        var data = await _repository.GetAllAsync(pro);
        return data.ToList();
    }

    public async Task<Product> GetProductsById(string id)
    {
        var data = await _repository.GetByIdAsync(id);
        return data;
    }

    public async Task AddProduct(Product product)
    {

        await _repository.InsertAsync(product);

    }


    public async Task UpdateProduct(string id, Product product)
    {

        Product data = await _repository.GetByIdAsync(id);

        var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
        var update = Builders<Product>.Update.Combine(
            product.ProductName != null ? Builders<Product>.Update.Set(a => a.ProductName, product.ProductName) : Builders<Product>.Update.Set(a => a.ProductName, data.ProductName),
            product.Price != 0 ? Builders<Product>.Update.Set(a => a.Price, product.Price) : Builders<Product>.Update.Set(a => a.Price, data.Price),
            product.Category != null ? Builders<Product>.Update.Set(a => a.Category, product.Category) : Builders<Product>.Update.Set(a => a.Category, data.Category),
            product.Manufacturer != null ? Builders<Product>.Update.Set(a => a.Manufacturer, product.Manufacturer) : Builders<Product>.Update.Set(a => a.Manufacturer, data.Manufacturer)
        );

        await _repository.UpdateFieldAsync(filter, update);
    }


}


