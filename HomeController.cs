using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BLOGW.Models;
using Newtonsoft.Json;
namespace BLOGW.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var blogs = PostManager.Read();
            if (blogs == null)
            {
                ViewBag.Empty = true;
                return View();
            }
            else
            {
                // Just for sorting.  
                blogs = (from blog in blogs
                         orderby blog.CreateTime descending
                         select blog).ToList();

                ViewBag.Empty = false;
                return View("index", blogs);
            }
        }

        public ActionResult About()
        {
            if (Request.HttpMethod == "POST")
            {
                // Post request method  
                var Id = Request.Form["Id"].ToString();
                var Name = Request.Form["Name"].ToString();
                var Email = Request.Form["Email"].ToString();
                var password = Request.Form["password"].ToString();
                var Role = Request.Form["Role"].ToString();
                var Address = Request.Form["Address"].ToString();
                var Birthdate = Request.Form["Birthdate"].ToString();

                // Save content  
                var post = new BlogPostModel { id = Id, CreateTime = DateTime.Now, name = Name, email= Email, Password= password, role= Role, address= Address, birthdate= Birthdate };
                PostManager.Create(JsonConvert.SerializeObject(post));

                // Redirect  
                Response.Redirect("~/Home/Index");
            }
            return View("About");
        }

        public ActionResult Contact()
        {
            var blogs = PostManager.Read();
            if (blogs == null)
            {
                ViewBag.Empty = true;
                return View();
            }
            else
            {
                // Just for sorting.  
                blogs = (from blog in blogs
                         orderby blog.CreateTime descending
                         select blog).ToList();

                ViewBag.Empty = false;
                return View("Contact", blogs);
            }
        }
        [Route("Home/read/{id}")]
    
        // GET: Blog
        
        public ActionResult Read(int id)
        {
            // Read one single blog  
            var blogs = PostManager.Read();
            BlogPostModel post = null;

            if (blogs != null && blogs.Count > 0)
            {
                post = blogs.Find(x => x.ID == id);
            }

            if (post == null)
            {
                ViewBag.PostFound = false;
                return View();
            }
            else
            {
                ViewBag.PostFound = true;
                return View("ViewPage", post);
            }
        }
        public ActionResult Create()
        {
            if (Request.HttpMethod == "POST")
            {
                // Post request method  
                var title = Request.Form["title"].ToString();
                var tags = Request.Form["tags"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var content = Request.Form["content"].ToString();

                // Save content  
                var post = new BlogPostModel { Title = title, CreateTime = DateTime.Now, Content = content, Tags = tags.ToList() };
                PostManager.Create(JsonConvert.SerializeObject(post));

                // Redirect  
                Response.Redirect("~/Home/Contact");
            }
            return View("Createpost");
        }
        [Route("Home/edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (Request.HttpMethod == "POST")
            {
                // Post request method  
                var title = Request.Form["title"].ToString();
                var tags = Request.Form["tags"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var content = Request.Form["content"].ToString();

                // Save content  
                var post = new BlogPostModel { Title = title, CreateTime = DateTime.Now, Content = content, Tags = tags.ToList() };
                PostManager.Update(id, JsonConvert.SerializeObject(post));

                // Redirect  
                Response.Redirect("~/Home/Contact");
            }
            else
            {
                // Find the required post.  
                var post = PostManager.Read().Find(x => x.ID == id);

                if (post != null)
                {
                    // Set the values  
                    ViewBag.Found = true;
                    ViewBag.PostTitle = post.Title;
                    ViewBag.Tags = post.Tags;
                    ViewBag.Content = post.Content;
                }
                else
                {
                    ViewBag.Found = false;
                }
            }

            // Finally return the view.  
            return View("Updatepage");
        }

    }

}