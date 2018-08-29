using Built.Mongo;
using BuiltCloud.BlogModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiltCloud.Portal
{
    [ViewComponent(Name = "RecentPosts")]
    public class RecentPostsComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecentPostsComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 5)
        {
            string MyView = "RecentPosts";
            var blogRepo = _unitOfWork.GetRepository<Blog>();
            return View(MyView, blogRepo.FindAll(t => t.CreatedOn, 0, count, true));
        }
    }
}