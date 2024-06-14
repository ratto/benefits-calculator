using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories.Interfaces;

/*
 * About the repositories:
 * This was a hard call. Should I use the same list that
 * was hard coded on the EmployeesController or should I 
 * build a database to make it more production ready?
 * 
 * My SQL and Entity Framework skills are not the best 
 * and I was in doubt on how make the DB available for
 * this system when you evaluate my work. So I decided 
 * to care for the requirements and the challenge as well.
 * 
 * But it doesn't mean that the repositories are in no use.
 * They can easily be refactored after creating the DB 
 * both in code first or DB first approach. In either case
 * it will have minimum impact (or even no impact at all)
 * on the whole system bercause of the separation of
 * concerns done with the dependency injection/inversion
 * that I made.
 */

namespace Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public List<GetEmployeeDto> Employees { get; } = new List<GetEmployeeDto>()
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
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
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

        public EmployeeRepository() { }

        public async Task<List<GetEmployeeDto>> SelectAllEmployeesAsync()
        {
            return await SimulateEmployeeDatabaseFetching();
        }

        public async Task<GetEmployeeDto> SelectEmployeeAsync(int id)
        {
            var employees = await SimulateEmployeeDatabaseFetching();
            var employee = employees.Where(x =>  x.Id == id).FirstOrDefault();

            if (employee == null) throw new Exception("The employee was not found");

            return employee;
        }

        private async Task<List<GetEmployeeDto>> SimulateEmployeeDatabaseFetching()
        {
            var employees = Employees;

            return employees;
        }
    }
}
