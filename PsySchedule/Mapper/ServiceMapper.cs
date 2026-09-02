using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Mapper
{
    public class ServiceMapper : IMapper<ServiceResponse, Service>
    {
        public Service FromDto(ServiceResponse dto)
        {
            return new Service()
            {
                Name = dto.Name,
                Price = dto.Price
            };
        }

        public ServiceResponse FromEntity(Service entity)
        {
            return new ServiceResponse(entity.Id, entity.Name, entity.Price, entity.Version);
        }
    }
}
