using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.Helpers;
using Blog.Models;
using PagedList;
using PagedList.Mvc;

namespace Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";
            EmailModel model = new EmailModel();
            return View(model);
        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.Posts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                                      p.BlogPostBody.Contains(searchStr) ||
                                      p.Abstract.Contains(searchStr) ||
                                      p.Comments.Any(c => c.Body.Contains(searchStr) ||
                                                     c.Author.FirstName.Contains(searchStr) ||
                                                     c.Author.LastName.Contains(searchStr) ||
                                                     c.Author.DisplayName.Contains(searchStr) ||
                                                     c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.Posts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    //var emailTo = ConfigurationManager.AppSettings["emailto"];
                    //var from = $"{model.FromEmail}<{emailTo}>";
                    //var email = new MailMessage(from, emailTo)
                    //{
                    //    Subject = model.Subject,
                    //    Body = $"{model.Body}<br/>Sent from le blog de christopher",
                    //    IsBodyHtml = true
                    //};

                    //var svc = new PersonalEmail();
                    //await svc.SendAsync(email);

                    //return View(new EmailModel());
                    await EmailHelper.ComposeEmailAsync(model);
                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

    }
}