using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories.Interfaces;

namespace Api.Repositories
{
    public class DependentRepository : IDependentRepository
    {
        /*
         * This was another hard call that I really dislike
         * but I had to do to focus on the main challenge.
         * 
         * I shouldn't repeat the same code in the
         * EmployeeRepository. But it is the same as the 
         * other repository file: after creating the DB and
         * deciding wich DB-to-code approach, we can easily 
         * change this whole file with low impact on the rest
         * of the system (thanks to the chosen design patern).
         */

        public List<GetDependentDto> Dependents { get; } = new List<GetDependentDto>()
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };

        public DependentRepository() { }

        public async Task<List<GetDependentDto>> SelectAllDependentsAsync()
        {
            return await SimulateDependentDatabaseFetching();
        }

        public async Task<GetDependentDto> SelectDependentAsync(int id)
        {
            var dependents = await SimulateDependentDatabaseFetching();
            var dependent = dependents.Where(x =>  x.Id == id).FirstOrDefault();

            if (dependent == null) throw new Exception("The dependent was not found");

            return dependent;
        }

        private async Task<List<GetDependentDto>> SimulateDependentDatabaseFetching()
        {
            var dependents = Dependents;

            return dependents;
        }
    }
}
