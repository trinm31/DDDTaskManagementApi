using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authorization;
using Infrastructure.Exceptions;

namespace API.Services.Users
{
    public class UserService:BaseService
    {
        private readonly IJwtUtils _jwtUtils;

        public UserService(
            IUnitOfWork unitOfWork, 
            IJwtUtils jwtUtils,
            IMapper mapper) : base(unitOfWork, mapper)
        {
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await UnitOfWork.Repository<User>().GetFirstOrDefaultAsync(x => x.Username == model.Username);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = Mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await UnitOfWork.Repository<User>().GetAllAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await UnitOfWork.Repository<User>().FindAsync(id);
        }

        public async Task Register(RegisterRequest model)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                // validate
                var userList = await UnitOfWork.Repository<User>().GetAllAsync(x => x.Username == model.Username);
                if (userList.Any())
                    throw new AppException("Username '" + model.Username + "' is already taken");

                // map model to new user object
                var user = Mapper.Map<User>(model);

                // hash password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // save user
                var userRepos = UnitOfWork.Repository<User>();
                await userRepos.InsertAsync(user);

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Update(int id, UpdateRequest model)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var user = await UnitOfWork.Repository<User>().FindAsync(id);

                // validate
                var userList = await UnitOfWork.Repository<User>().GetAllAsync(x => x.Username == model.Username);
                if (userList.Any())
                    throw new AppException("Username '" + model.Username + "' is already taken");

                // hash password if it was entered
                if (!string.IsNullOrEmpty(model.Password))
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // copy model to user and save
                Mapper.Map(model, user);

                await UnitOfWork.SaveChangesAsync();

                await UnitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var user = await UnitOfWork.Repository<User>().FindAsync(id);
                if (user == null) throw new KeyNotFoundException("User not found");
                await UnitOfWork.Repository<User>().DeleteAsync(user);

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
