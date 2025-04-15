using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoMaster.Domain.Interfaces;
using TodoMaster.Models;

namespace TodoMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(ITodoList todoList, ITodoListRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResponse<TodoItemModel>> GetAll()
        {
            try
            {
                var items = todoList.GetAllItems();
                var dtos = mapper.Map<List<TodoItemModel>>(items);
                return Ok(ApiResponse<TodoItemModel>.Success(dtos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TodoItemModel>.Fail([ex.Message]));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<TodoItemModel>> GetById(int id)
        {
            var item = todoList.GetItemById(id);
            if (item == null)
                return NotFound(ApiResponse<TodoItemModel>.Fail(["Item not found."]));

            var dto = mapper.Map<TodoItemModel>(item);
            return Ok(ApiResponse<TodoItemModel>.Success([dto]));
        }

        [HttpPost]
        public ActionResult<ApiResponse<TodoItemModel>> Create([FromBody] CreateTodoRequest req)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(req.Title))
                errors.Add("Title is required.");

            if (string.IsNullOrWhiteSpace(req.Category) || !repository.GetAllCategories().Contains(req.Category))
                errors.Add("Invalid category.");

            if (errors.Count != 0)
                return BadRequest(ApiResponse<TodoItemModel>.Fail(errors));

            try
            {
                var id = repository.GetNextId();
                todoList.AddItem(id, req.Title, req.Description, req.Category);
                var item = todoList.GetItemById(id);
                var dto = mapper.Map<TodoItemModel>(item);

                return Ok(ApiResponse<TodoItemModel>.Success([dto]));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TodoItemModel>.Fail([ex.Message]));
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<TodoItemModel>> Update(int id, [FromBody] UpdateTodoRequest req)
        {
            try
            {
                todoList.UpdateItem(id, req.Description);
                var item = todoList.GetItemById(id);
                var dto = mapper.Map<TodoItemModel>(item);
                return Ok(ApiResponse<TodoItemModel>.Success([dto]));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TodoItemModel>.Fail([ex.Message]));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<TodoItemModel>> Delete(int id)
        {
            try
            {
                var item = todoList.GetItemById(id);
                if (item == null)
                    return NotFound(ApiResponse<TodoItemModel>.Fail(["Item not found."]));

                todoList.RemoveItem(id);
                return Ok(ApiResponse<TodoItemModel>.Success([]));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TodoItemModel>.Fail([ex.Message]));
            }
        }

        [HttpPost("{id}/progression")]
        public ActionResult<ApiResponse<TodoItemModel>> RegisterProgression(int id, [FromBody] RegisterProgressionRequest req)
        {
            try
            {
                todoList.RegisterProgression(id, req.Date, req.Percent);
                var item = todoList.GetItemById(id);
                var dto = mapper.Map<TodoItemModel>(item);
                return Ok(ApiResponse<TodoItemModel>.Success([dto]));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TodoItemModel>.Fail([ex.Message]));
            }
        }
    }
}
