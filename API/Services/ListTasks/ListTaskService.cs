using API.DTOs.ListTasks;
using AutoMapper;
using Domain.Entities.ListTasks;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services.ListTasks
{
    public class ListTaskService: BaseService
    {
        public ListTaskService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IList<ListTask>> GetAll()
        {
            var listTasks = await UnitOfWork.Repository<ListTask>().GetAllAsync(x => x.IsDeleted == false);
            return listTasks.ToList();
        }

        public async Task<ListTask> GetOne(int listTaskId)
        {
            return await UnitOfWork.Repository<ListTask>().GetFirstOrDefaultAsync(x => x.Id == listTaskId);
        }

        public async Task Update(ListTaskUpsertRequest listTask, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var listTaskRepos = UnitOfWork.Repository<ListTask>();

                var listTaskInput = Mapper.Map<ListTask>(listTask);

                var listTaskDb = await listTaskRepos.FindAsync(listTaskInput.Id);
                if (listTaskDb == null)
                    throw new KeyNotFoundException();

                listTaskDb.Name = listTaskInput.Name;
                listTaskDb.ProjectId = listTaskInput.ProjectId;
                listTaskDb.UpdatedDate = DateTime.Now;
                listTaskDb.UpdatedById = user.Id;

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(ListTaskUpsertRequest listTask, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var listTaskRepos = UnitOfWork.Repository<ListTask>();

                var listTaskInput = Mapper.Map<ListTask>(listTask);

                listTaskInput.CreatedDate = DateTime.Now;
                listTaskInput.CreatedById = user.Id;

                await listTaskRepos.InsertAsync(listTaskInput);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int listTaskId, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var listTaskRepos = UnitOfWork.Repository<ListTask>();

                var listTaskDb = await listTaskRepos.FindAsync(listTaskId);
                if (listTaskDb == null)
                    throw new KeyNotFoundException();

                listTaskDb.IsDeleted = true;

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
