using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

using WebApi.Models;


namespace WebApi.Controllers
{
    public class AutodeskUserController : ApiController
    {
        private DBModel db = new DBModel();
        AutodeskUser user = new AutodeskUser();

        // GET: api/AutodeskUser/5
        [ResponseType(typeof(AutodeskUser))]
        public IHttpActionResult GetAutodeskUser()
        {
            string username="";
            string password="";
            var request = Request;
            var headers = request.Headers;
            if (headers.Contains("username"))
            {
                username = headers.GetValues("username").First();
            }

            if (headers.Contains("task") && headers.GetValues("task").First()== "verifyUsername")
            {
                AutodeskUser result = db.AutodeskUsers.FirstOrDefault(x => x.username == username);
                if (result == null)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
            else
            {

                if (headers.Contains("password"))
                {
                    password = headers.GetValues("password").First();
                }

                AutodeskUser result = db.AutodeskUsers.FirstOrDefault(x => x.username == username && x.userPassword == password);
                if (result == null)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
        }
        

        // POST: api/AutodeskUser
        [ResponseType(typeof(AutodeskUser))]
        public IHttpActionResult PostAutodeskUser(AutodeskUser autodeskUser)
        {

            if (!AutodeskUserExists(autodeskUser.username))
            {
                db.AutodeskUsers.Add(autodeskUser);
            }
            else
            {
                return Ok(user);
            }
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AutodeskUserExists(autodeskUser.username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = autodeskUser.username }, autodeskUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AutodeskUserExists(string id)
        {
            return db.AutodeskUsers.Count(e => e.username == id) > 0;
        }

    }

    public class user
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}