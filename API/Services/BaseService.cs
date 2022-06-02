using AutoMapper;
using Domain.Interfaces;

namespace API.Services
{
    public class BaseService
    {
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        protected internal IUnitOfWork UnitOfWork { get; set; }
        protected internal IMapper Mapper { get; set; }
    }
}
