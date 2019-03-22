using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Models;
using Todo.API.Repository;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // GET api/todo
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_todoRepository.GetAll());
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = _todoRepository.GetByID(id);
            
            if (item != null) {
                return Ok(item);
            } else {
                return NotFound();
            }
        }

        // POST api/todo
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            item.CreatedBy = HttpContext.User.Identity.Name;
            
            var newItem = _todoRepository.Add(item);
            return Ok(newItem);
        }

        // PUT api/todo/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TodoItem item)
        {
            if (_todoRepository.GetByID(id) == null) {
                return NotFound();
            }

            if (_todoRepository.GetByID(id).CreatedBy != HttpContext.User.Identity.Name) {
                return Unauthorized();
            }

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            item.ItemId = id;
            _todoRepository.Update(item);
            return Ok();
        }

        // DELETE api/todo/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_todoRepository.GetByID(id) != null) {
                _todoRepository.Delete(id);
                return Ok();
            } else if (_todoRepository.GetByID(id).CreatedBy != HttpContext.User.Identity.Name) {
                return Unauthorized();
            } else {
                return NotFound();
            }
        }
    }
}
