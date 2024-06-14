using Api.Dtos.Dependent;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _dependetRepository;

        public DependentService(IDependentRepository dependentRepository)
        {
            _dependetRepository = dependentRepository;
        }

        public async Task<List<GetDependentDto>> GetAllDependentsAsync()
        {
            return await _dependetRepository.SelectAllDependentsAsync();
        }

        public async Task<(GetDependentDto?, string)> GetDependentByIdAsync(int id)
        {
            try
            {
                return (await _dependetRepository.SelectDependentAsync(id), "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
