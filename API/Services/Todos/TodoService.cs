using API.DTOs.Todos;
using AutoMapper;
using Domain.Entities.Todos;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services.Todos
{
    public class TodoService: BaseService
    {
        public TodoService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IList<Todo>> GetAll()
        {
            var todoList = await UnitOfWork.Repository<Todo>().GetAllAsync(x => x.IsDeleted == false);
            return todoList.ToList();
        }

        public async Task<Todo> GetOne(int todoId)
        {
            return await UnitOfWork.Repository<Todo>().GetFirstOrDefaultAsync(x => x.Id == todoId);
        }

        public async Task Update(TodoUpSertRequest todoDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoItem = Mapper.Map<Todo>(todoDto);

                var todoRepos = UnitOfWork.Repository<Todo>();
                var todoDb = await todoRepos.FindAsync(todoItem.Id);
                if (todoDb == null)
                    throw new KeyNotFoundException();

                todoDb.Content = todoItem.Content;
                todoDb.TaskId = todoItem.TaskId;
                todoDb.UpdatedDate = DateTime.Now;
                todoDb.UpdatedById = user.Id;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(TodoUpSertRequest todoDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoItem = Mapper.Map<Todo>(todoDto);

                var todoRepos = UnitOfWork.Repository<Todo>();

                todoItem.CreatedDate = DateTime.Now;
                todoItem.CreatedById = user.Id;

                await todoRepos.InsertAsync(todoItem);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int todoId, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoRepos = UnitOfWork.Repository<Todo>();
                var todoItem = await todoRepos.FindAsync(todoId);
                if (todoItem == null)
                    throw new KeyNotFoundException();

                todoItem.IsDeleted = true;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task IsDone(int todoId)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoRepos = UnitOfWork.Repository<Todo>();
                var todoItem = await todoRepos.FindAsync(todoId);
                if (todoItem == null)
                    throw new KeyNotFoundException();

                todoItem.IsDone = true;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
