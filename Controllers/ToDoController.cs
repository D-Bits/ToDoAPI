using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create new item, if collection is empty, Therefore, you cannot delete all items
                _context.TodoItems.Add(new TodoItem { Name = "Item1"});
                _context.SaveChanges();
            }
        }

        // Get all items
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        // Get todo item by id
        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        // Create TodoItem
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        // Update Todo Item
        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);
            if(todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        //Delete todo item
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if(todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
