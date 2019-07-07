using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using UdemyApp.DB;
using Microsoft.AspNetCore.Authorization;

namespace UdemyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private UdemyDbContext _db;
        public ValuesController(UdemyDbContext db){
            _db = db;
        }


        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var values = _db.Values.ToList();
            return  Ok( values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
           // HttpRuntime.Cache.['Hello'];
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
