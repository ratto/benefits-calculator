using Api.Dtos.Dependent;

namespace Api.Services.Interfaces
{
    public interface IDependentService
    {
        Task<List<GetDependentDto>> GetAllDependentsAsync();
        Task<(GetDependentDto?, string)> GetDependentByIdAsync(int id);
    }
}
