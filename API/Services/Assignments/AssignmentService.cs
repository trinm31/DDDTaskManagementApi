using API.DTOs.Assignments;
using AutoMapper;
using Domain.Entities.Assigments;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services.Assignments
{
    public class AssignmentService: BaseService
    {
        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task Add(AssignmentUpSertRequest assignDto, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var assignRepos = UnitOfWork.Repository<Assignment>();

                var assignInput = Mapper.Map<Assignment>(assignDto);

                assignInput.CreatedDate = DateTime.Now;
                assignInput.CreatedById = user.Id;

                await assignRepos.InsertAsync(assignInput);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int assignmentId, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var assignRepos = UnitOfWork.Repository<Assignment>();

                var assignDb = await assignRepos.FindAsync(assignmentId);
                if (assignDb == null)
                    throw new KeyNotFoundException();

                assignDb.IsDeleted = true;

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
