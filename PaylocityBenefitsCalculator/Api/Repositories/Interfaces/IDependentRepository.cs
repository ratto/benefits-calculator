using Api.Dtos.Dependent;

namespace Api.Repositories.Interfaces
{
    public interface IDependentRepository
    {
        Task<List<GetDependentDto>> SelectAllDependentsAsync();
        Task<GetDependentDto> SelectDependentAsync(int id);
    }
}
