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

        var p = Builders<Product>.Projection
        .Exclude(a => a.Id);

        var s = Builders<Product>.Sort
            .Ascending(a => a.ManufacturingDate);

        var data = await _repository.GetAllAsync(null, s);
        return data.ToList();
    }
    public async Task<Pager<Product>> GetProductWithPaging(string keyword, int page, int pagesize)
    {

        var f = Builders<Product>.Filter;
        var filterDefination = keyword != null ? f.Eq(x => x.Category, keyword) : f.Empty;

        var sortingDefination = Builders<Product>.Sort
            .Ascending(a => a.ManufacturingDate);

        var data = await _repository.GetdatawithPaging<Product>(filterDefination, sortingDefination, page, pagesize);
        return data;
    }


    public async Task<List<Product>> DateFilter(string startDate, string endDate)
    {

        var f = Builders<Product>.Filter;
        var filterDefinition = f.Gte(x => x.ManufacturDate, startDate) & f.Lte(x => x.ManufacturDate, endDate);

        var data = await _repository.DateFilter(filterDefinition);
        return data.ToList();
    }


    public async Task<Product> GetProductsById(string id)
    {
        var data = await _repository.GetByIdAsync(id);
        return data;
    }

    public async Task AddProduct(Product product)
    {

        product.ManufacturingDate = DateTime.Now;
        product.ManufacturDate = product.ManufacturingDate.ToString("O");
        product.TimeZone = product.ManufacturingDate.ToString("G");

        //DateTime Test = DateTime.Parse(product.ManufacturDate);

        await _repository.InsertAsync(product);

    }


    public async Task UpdateProduct(string id, Product product)
    {

        Product data = await _repository.GetByIdAsync(id);

        var f = Builders<Product>.Filter;
        var filter = f.Eq(x => x.Id, product.Id) & f.Eq(x => x.ProductName, product.ProductName);


        var update = Builders<Product>.Update
        .Set(x => x.ProductName, product.ProductName)
        .Set(x => x.Manufacturer, product.Manufacturer)
        .Set(x => x.Category, product.Category)
        .Set(x => x.Price, product.Price)
        .Set(x => x.ManufacturingDate, product.ManufacturingDate)
        .SetOnInsert(x => x.Id, product.Id);


        await _repository.UpdateFieldAsync(filter, update, true);
    }


}


