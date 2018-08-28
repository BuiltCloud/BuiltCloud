using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuiltCloud.Portal.Models;
using BuiltCloud.BlogModel;
using Built.Mongo;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BuiltCloud.Portal.Controllers
{
    public static class er
    {
        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> exprLeft, bool condition, Expression<Func<T, bool>> exprRight)
        {
            var invokedExpr = Expression.Invoke(exprRight, exprLeft.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(exprLeft.Body, invokedExpr), exprLeft.Parameters);
        }

        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> exprLeft, bool condition, Expression<Func<T, bool>> exprRight)
        {
            var invokedExpr = Expression.Invoke(exprRight, exprLeft.Parameters);

            return Expression.Lambda<Func<T, bool>>(Expression.And(exprLeft.Body, invokedExpr), exprLeft.Parameters);
        }
    }

    public class bModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; } = 10;

        public string Tag { get; set; }

        public string Catalog { get; set; }
    }

    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("blogs")]
        [Route("blogs/{PageIndex:int}")]
        [Route("blogs/{PageIndex:int}/{PageSize:int}")]
        [Route("blogs/tag/{Tag}")]
        [Route("blogs/tag/{Tag}/{PageIndex:int}")]
        [Route("blogs/tag/{Tag}/{PageIndex:int}/{PageSize:int}")]
        [Route("blogs/catalog/{Catalog}")]
        [Route("blogs/catalog/{Catalog}/{PageIndex:int}")]
        [Route("blogs/catalog/{Catalog}/{PageIndex:int}/{PageSize:int}")]
        public IActionResult Index(bModel model)
        {
            var _repository = _unitOfWork.GetRepository<Blog>();
            var list = _repository.Find(t =>
            (string.IsNullOrWhiteSpace(model.Tag) || t.Tags.Contains(model.Tag)) &&
            (string.IsNullOrWhiteSpace(model.Catalog) || t.Catalog.Equals(model.Catalog))
            , model.PageIndex, model.PageSize);
            return View(list);
        }

        public IActionResult Detail(string id)
        {
            var _repository = _unitOfWork.GetRepository<Blog>();
            var blog = _repository.Get(id);
            return View(blog);
        }

        public IActionResult List()
        {
            var _repository = _unitOfWork.GetRepository<Blog>();
            var list = _repository.Find(t => t.Tags.Contains("Docker"));
            return Json(list);
        }

        [HttpPost]
        public IActionResult Add2(Blog blog)
        {
            //Blog blog = new Blog
            //{
            //    Catalog = ".net core",
            //    Author = "Enter",
            //    Content = "内容" + DateTime.Now,
            //    Editer = "Enter",
            //    Features = new string[] { "置顶", "原创", "精品" },
            //    Tags = new string[] { "Kubernetes", "Docker", ".net core" },
            //    Title = ".net core on Kubernetes.",
            //    Url = "",
            //    Version = 0
            //};
            var _repository = _unitOfWork.GetRepository<Blog>();
            _repository.Insert(blog);
            return Json(blog);
        }

        public IActionResult Add(Blog blog)
        {
            if (!string.IsNullOrWhiteSpace(blog.Title) && !string.IsNullOrWhiteSpace(blog.Catalog))
            {
                var _repository = _unitOfWork.GetRepository<Blog>();
                _repository.Insert(blog);

                var catalogRepo = _unitOfWork.GetRepository<Catalog>();
                /*
                 FilterDefinition<BsonDocument> filter = "{ x: 1 }";
                // or
                FilterDefinition<BsonDocument> filter = new BsonDocument("x", 1);
                 */

                var result = catalogRepo.Collection.UpdateMany(new BsonDocument("Name", blog.Catalog), catalogRepo.Updater.Inc(t => t.Added, 1), new UpdateOptions
                {
                    IsUpsert = true
                });
                // db.test.update({y:999}, {$inc: { money: 10} })
                //db.test.update({y:100},{y:999},true)
                //db.test.update({y:1}, {$inc: { money: 10} },true)
            }

            return View(blog);
        }
    }
}