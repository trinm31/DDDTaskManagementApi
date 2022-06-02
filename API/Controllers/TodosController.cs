using API.Authorization;
using API.DTOs.Todos;
using API.Services.Todos;
using Domain.Entities.Todos;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TodoService _todoService;

        public TodosController(TodoService todoService, IHttpContextAccessor httpContextAccessor)
        {
            _todoService = todoService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region CRUD

        [HttpGet]
        public async Task<IList<Todo>> GetAll()
        {
            return await _todoService.GetAll();
        }

        [HttpPut]
        public async Task Update(TodoUpSertRequest todoUpSertDto)
        {
            var user = GetCurrentUser();
            await _todoService.Update(todoUpSertDto, user);
        }

        [HttpGet("{id:int}")]
        public async Task<Todo> GetOne([FromRoute] int id)
        {
            return await _todoService.GetOne(id);
        }

        [HttpPost]
        public async Task Add(TodoUpSertRequest todoUpSertDto)
        {
            var user = GetCurrentUser();
            await _todoService.Add(todoUpSertDto, user);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id)
        {
            var user = GetCurrentUser();
            await _todoService.Delete(id, user);
        }

        [HttpGet("[action]/{id:int}")]
        public async Task IsDone([FromRoute] int id)
        {
            await _todoService.IsDone(id);
        }

        #endregion

        private User GetCurrentUser()
        {
            return (User)_httpContextAccessor.HttpContext.Items["User"];
        }
    }
}
