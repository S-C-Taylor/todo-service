using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Todo.Repository;

namespace Todo.Controllers
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
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return Ok(_todoRepository.GetAll());
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            return _todoRepository.GetByID(id);
        }

        // POST api/todo
        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var newItem = _todoRepository.Add(item);
            return Ok(newItem);
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TodoItem item)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            item.ItemId = id;
            _todoRepository.Update(item);
            return Ok();
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_todoRepository.GetByID(id) != null) {
                _todoRepository.Delete(id);
                return Ok();
            } else {
                return NotFound();
            }
        }
    }
}
