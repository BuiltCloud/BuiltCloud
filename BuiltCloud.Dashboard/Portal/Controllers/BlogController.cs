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
        private readonly IRepository<Blog> _repository;

        public BlogController(IRepository<Blog> repository)
        {
            _repository = repository;
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
            var list = _repository.Find(t =>
            (string.IsNullOrWhiteSpace(model.Tag) || t.Tags.Contains(model.Tag)) &&
            (string.IsNullOrWhiteSpace(model.Catalog) || t.Catalog.Equals(model.Catalog))
            , model.PageIndex, model.PageSize);
            return View(list);
        }

        public IActionResult Detail(string id)
        {
            ViewData["Message"] = "Your application description page.";
            var blog = _repository.Get(id);
            return View(blog);
        }

        public IActionResult List()
        {
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
            _repository.Insert(blog);
            return Json(blog);
        }

        public IActionResult Add(Blog blog)
        {
            if (!string.IsNullOrWhiteSpace(blog.Title))
                _repository.Insert(blog);
            return View(blog);
        }
    }
}