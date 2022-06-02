using API.DTOs.TaskItems;
using AutoMapper;
using Domain.Entities.Tasks;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services.TaskItems
{
    public class TaskItemService: BaseService
    {
        public TaskItemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IList<TaskItem>> GetAll()
        {
            var TaskItems = await UnitOfWork.Repository<TaskItem>().GetAllAsync(x => x.IsDeleted == false);
            return TaskItems.ToList();
        }

        public async Task<TaskItem> GetOne(int taskId)
        {
            return await UnitOfWork.Repository<TaskItem>().GetFirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task Update(TaskItemUpSertRequest taskItemDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var taskItem = Mapper.Map<TaskItem>(taskItemDto);

                var taskRepos = UnitOfWork.Repository<TaskItem>();
                var task = await taskRepos.FindAsync(taskItem.Id);
                if (task == null)
                    throw new KeyNotFoundException();

                task.Name = taskItem.Name;
                task.Content = taskItem.Content;
                task.ListTaskId = taskItem.ListTaskId;
                task.UpdatedDate = DateTime.Now;
                task.UpdatedById = user.Id;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(TaskItemUpSertRequest taskItemDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var taskItem = Mapper.Map<TaskItem>(taskItemDto);

                var taskRepos = UnitOfWork.Repository<TaskItem>();

                taskItem.CreatedDate = DateTime.Now;
                taskItem.CreatedById = user.Id;

                await taskRepos.InsertAsync(taskItem);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int taskId, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var taskRepos = UnitOfWork.Repository<TaskItem>();
                var task = await taskRepos.FindAsync(taskId);
                if (task == null)
                    throw new KeyNotFoundException();

                task.IsDeleted = true;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task IsDone(int taskId)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var taskRepos = UnitOfWork.Repository<TaskItem>();
                var task = await taskRepos.FindAsync(taskId);
                if (task == null)
                    throw new KeyNotFoundException();

                task.IsDone = true;

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
