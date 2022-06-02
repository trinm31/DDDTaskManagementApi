using API.Authorization;
using API.DTOs.TaskItems;
using API.Services.TaskItems;
using Domain.Entities.Tasks;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskItemService _taskService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TaskItemsController(TaskItemService taskService, IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region CRUD

        [HttpGet]
        public async Task<IList<TaskItem>> GetAll()
        {
            return await _taskService.GetAll();
        }

        [HttpPut]
        public async Task Update(TaskItemUpSertRequest taskItem)
        {
            var user = GetCurrentUser();
            await _taskService.Update(taskItem, user);
        }

        [HttpGet("{id:int}")]
        public async Task<TaskItem> GetOne([FromRoute] int id)
        {
            return await _taskService.GetOne(id);
        }

        [HttpPost]
        public async Task Add(TaskItemUpSertRequest taskItem)
        {
            var user = GetCurrentUser();
            await _taskService.Add(taskItem, user);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id)
        {
            var user = GetCurrentUser();
            await _taskService.Delete(id, user);
        }

        [HttpGet("[action]/{id:int}")]
        public async Task IsDone([FromRoute] int id)
        {
            await _taskService.IsDone(id);
        }

        #endregion

        private User GetCurrentUser()
        {
            return (User)_httpContextAccessor.HttpContext.Items["User"];
        }
    }
}
