using Api.Controllers;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;

namespace Api.Test;

public class ItemControllerTest
{
    [Theory]
    [InlineData("test", 0)]
    [InlineData("", 1)]
    public void PostPutModel_Validation(string title, int expectedResult)
    {
        var model = new ItemPostPutModel
        {
            Title = title
        };

        Assert.Equal(expectedResult, Helper.ValidateModel(model).Count);
    }

    [Fact]
    public void List_Success()
    {
        var _db = Helper.GetDbContext();
        _db.Items.Add(new ItemDbModel()
        {
            Title = "Guitar"
        });
        _db.SaveChanges();

        var controller = new ItemController(_db);
        var response = controller.List();

        Assert.True(response.Value!.Count > 0);
    }

    [Fact]
    public void Get_Success()
    {
        var _db = Helper.GetDbContext();
        var model = new ItemDbModel()
        {
            Title = "Guitar"
        };
        _db.Items.Add(model);
        _db.SaveChanges();

        var controller = new ItemController(_db);
        var response = controller.Get(model.Id);

        Assert.NotNull(response.Value);
    }

    [Fact]
    public void Get_NotFound()
    {
        var _db = Helper.GetDbContext();

        var controller = new ItemController(_db);
        var response = controller.Get(Guid.NewGuid());

        Assert.Equal(StatusCodes.Status404NotFound, ((NotFoundResult)response.Result!).StatusCode);
    }

    [Fact]
    public void Post_Success()
    {
        var _db = Helper.GetDbContext();

        var controller = new ItemController(_db);
        var response = controller.Post(new ItemPostPutModel()
        {
            Title = "Test"
        });

        Assert.NotNull(response.Value);
    }

    [Fact]
    public void Put_Success()
    {
        var _db = Helper.GetDbContext();
        var model = new ItemDbModel()
        {
            Title = "Guitar"
        };
        _db.Items.Add(model);
        _db.SaveChanges();

        var controller = new ItemController(_db);
        var response = controller.Put(model.Id, new ItemPostPutModel()
        {
            Title = "Test"
        });

        Assert.NotNull(response.Value);
    }

    [Fact]
    public void Put_NotFound()
    {
        var _db = Helper.GetDbContext();

        var controller = new ItemController(_db);
        var response = controller.Put(Guid.NewGuid(), new ItemPostPutModel()
        {
            Title = "Test"
        });

        Assert.Equal(StatusCodes.Status404NotFound, ((NotFoundResult)response.Result!).StatusCode);
    }

    [Fact]
    public void Delete_Success()
    {
        var _db = Helper.GetDbContext();
        var model = new ItemDbModel()
        {
            Title = "Guitar"
        };
        _db.Items.Add(model);
        _db.SaveChanges();

        var controller = new ItemController(_db);
        var response = controller.Delete(model.Id);

        Assert.Equal(StatusCodes.Status200OK, ((OkResult)response.Result!).StatusCode);
    }

    [Fact]
    public void Delete_NotFound()
    {
        var _db = Helper.GetDbContext();

        var controller = new ItemController(_db);
        var response = controller.Delete(Guid.NewGuid());

        Assert.Equal(StatusCodes.Status404NotFound, ((NotFoundResult)response.Result!).StatusCode);
    }
}

