using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Models;

namespace Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ItemController : Controller
{
    private readonly DataContext _db;

    public ItemController(DataContext db)
    {
        _db = db;
    }

    [HttpGet("List")]
    public ActionResult<List<ItemModel>> List()
    {
        var models = _db.Items.ToList();
        return models.ConvertAll(x => CreateModelObject(x));
    }

    [HttpGet("{id}")]
    public ActionResult<ItemModel> Get(Guid id)
    {
        var dbModel = _db.Items.FirstOrDefault(x => x.Id == id);
        if (dbModel == null) return NotFound();
        return CreateModelObject(dbModel);
    }

    [HttpPost]
    public ActionResult<ItemModel> Post(ItemPostPutModel model)
    {
        var dbModel = new ItemDbModel()
        {
            Title = model.Title,
        };

        _db.Items.Add(dbModel);
        _db.SaveChanges();

        return CreateModelObject(dbModel);
    }

    [HttpPost("{id}")]
    public ActionResult<ItemModel> Put(Guid id, ItemPostPutModel model)
    {
        var dbModel = _db.Items.FirstOrDefault(x => x.Id == id);
        if (dbModel == null) return NotFound();

        dbModel.Title = model.Title;
        _db.SaveChanges();

        return CreateModelObject(dbModel);
    }

    [HttpDelete("{id}")]
    public ActionResult<ItemModel> Delete(Guid id)
    {
        var dbModel = _db.Items.FirstOrDefault(x => x.Id == id);
        if (dbModel == null) return NotFound();

        _db.Remove(dbModel);
        _db.SaveChanges();

        return Ok();
    }

    private static ItemModel CreateModelObject(ItemDbModel model)
    {
        return new ItemModel
        {
            Id = model.Id,
            Title = model.Title,
        };
    }
}

