using Built.Mongo;
using BuiltCloud.BlogModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiltCloud.Portal
{
    [ViewComponent(Name = "BlogSidebar")]
    public class BlogSidebarComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogSidebarComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, int count)
        {
            switch (name)
            {
                case "RecentPosts":
                    var blogRepo = _unitOfWork.GetRepository<Blog>();
                    var result = blogRepo.FindAll(t => t.CreatedOn, 0, count, true);
                    return View(name, result);

                case "Catalogs":
                    return View(name, _unitOfWork.GetRepository<Catalog>().FindAll(0, count));

                case "Tags":
                    return View(name, _unitOfWork.GetRepository<BlogTag>().FindAll(0, count));
            }
            throw new System.Exception("name not def");
        }
    }
}