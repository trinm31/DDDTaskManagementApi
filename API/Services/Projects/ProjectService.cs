using API.DTOs.Projects;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Entities.Users;
using Domain.Interfaces;
using Infrastructure.Exceptions;

namespace API.Services.Projects
{
    public class ProjectService: BaseService
    {
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IList<Project>> GetAll()
        {
            var projectList = await UnitOfWork.Repository<Project>().GetAllAsync(x => x.IsDeleted == false);
            return projectList.ToList();
        }

        public async Task<Project> GetOne(int projectId)
        {
            return await UnitOfWork.Repository<Project>().GetFirstOrDefaultAsync(x => x.Id == projectId && x.IsDeleted == false);
        }

        public async Task Update(ProjectUpSertRequest project, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var projectRepos = UnitOfWork.Repository<Project>();

                // validate name
                var projNameCheck = await projectRepos.GetAllAsync(x => x.Name == project.Name);
                if (projNameCheck.Any())
                    throw new AppException("Project '" + project.Name + "' is already taken");

                var projectInput = Mapper.Map<Project>(project);

                var projectDb = await projectRepos.FindAsync(projectInput.Id);
                if (projectDb == null)
                    throw new KeyNotFoundException();

                projectDb.Name = projectInput.Name;
                projectDb.Description = projectInput.Description;
                projectDb.UpdatedDate = DateTime.Now;
                projectDb.UpdatedById = user.Id;
                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(ProjectUpSertRequest project, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var projectRepos = UnitOfWork.Repository<Project>();
                // validate
                var projCheck = await projectRepos.GetAllAsync(x => x.Name == project.Name);
                if (projCheck.Any())
                    throw new AppException("Project '" + project.Name + "' is already taken");

                var projectInput = Mapper.Map<Project>(project);

                projectInput.CreatedDate = DateTime.Now;
                projectInput.CreatedById = user.Id;

                await projectRepos.InsertAsync(projectInput);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int projectId, User user)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var projectRepos = UnitOfWork.Repository<Project>();

                var projectDb = await projectRepos.FindAsync(projectId);
                if (projectDb == null)
                    throw new KeyNotFoundException();

                projectDb.IsDeleted = true;

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
