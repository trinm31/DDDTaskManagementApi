using API.DTOs.TodoItems;
using AutoMapper;
using Domain.Entities.TodoItems;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services.TodoItems
{
    public class TodoItemService: BaseService
    {
        public TodoItemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IList<TodoItem>> GetAll()
        {
            var todoItemList = await UnitOfWork.Repository<TodoItem>().GetAllAsync(x => x.IsDeleted == false);
            return todoItemList.ToList();
        }

        public async Task<TodoItem> GetOne(int todoId)
        {
            return await UnitOfWork.Repository<TodoItem>().GetFirstOrDefaultAsync(x => x.Id == todoId);
        }

        public async Task Update(TodoItemUpSertRequest todoDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoItem = Mapper.Map<TodoItem>(todoDto);

                var todoRepos = UnitOfWork.Repository<TodoItem>();
                var todoItemDb = await todoRepos.FindAsync(todoItem.Id);
                if (todoItemDb == null)
                    throw new KeyNotFoundException();

                todoItemDb.Content = todoItem.Content;
                todoItemDb.TodoId = todoItem.TodoId;
                todoItemDb.UpdatedDate = DateTime.Now;
                todoItemDb.UpdatedById = user.Id;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(TodoItemUpSertRequest todoDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var todoItem = Mapper.Map<TodoItem>(todoDto);

                var todoItemRepos = UnitOfWork.Repository<TodoItem>();

                todoItem.CreatedDate = DateTime.Now;
                todoItem.CreatedById = user.Id;

                await todoItemRepos.InsertAsync(todoItem);

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

                var todoItemRepos = UnitOfWork.Repository<TodoItem>();
                var todoItem = await todoItemRepos.FindAsync(todoId);
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

                var todoItemRepos = UnitOfWork.Repository<TodoItem>();
                var todoItem = await todoItemRepos.FindAsync(todoId);
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
