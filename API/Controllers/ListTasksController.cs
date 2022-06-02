using API.Authorization;
using API.DTOs.ListTasks;
using API.Services.ListTasks;
using Domain.Entities.ListTasks;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListTasksController : ControllerBase
    {
        private readonly ListTaskService _listTaskService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListTasksController(ListTaskService listTaskService, IHttpContextAccessor httpContextAccessor)
        {
            _listTaskService = listTaskService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region CRUD
        [HttpGet]
        public async Task<IList<ListTask>> GetAll()
        {
            return await _listTaskService.GetAll();
        }

        [HttpPut]
        public async Task Update(ListTaskUpsertRequest listTaskDto)
        {
            var user = GetCurrentUser();
            await _listTaskService.Update(listTaskDto, user);
        }

        [HttpGet("{id:int}")]
        public async Task<ListTask> GetOne([FromRoute] int id)
        {
            return await _listTaskService.GetOne(id);
        }

        [HttpPost]
        public async Task Add(ListTaskUpsertRequest listTaskDto)
        {
            var user = GetCurrentUser();
            await _listTaskService.Add(listTaskDto, user);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id)
        {
            var user = GetCurrentUser();
            await _listTaskService.Delete(id, user);
        }

        #endregion

        private User GetCurrentUser()
        {
            return (User)_httpContextAccessor.HttpContext.Items["User"];
        }
    }
}
