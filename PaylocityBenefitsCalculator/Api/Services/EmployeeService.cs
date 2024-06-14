using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

/*
 * About Services layer:
 * As used in many MVC and clean architecture 
 * systems model, this layer should be used for
 * validations and other functionalities to make 
 * the communication between controllers and
 * repositories easier.
 * 
 * When more business and users needs are
 * discovered (using Scrum terminology) we 
 * can easily implement them in here with low 
 * impact on other layers of this system.
 */

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.SelectAllEmployeesAsync();
        }

        public async Task<(GetEmployeeDto?, string)> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return (await _employeeRepository.SelectEmployeeAsync(id), "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (GetEmployeeDto?, string) PostDependentForEmployee(GetEmployeeDto employee, GetDependentDto dependent)
        {
            // TO DO: make this validation on restricting spouse and partner with the DB information. It is not a good thing to pass these two through arguments.
            if (employee.Dependents.Where(x => x.Relationship == Models.Relationship.Spouse || x.Relationship == Models.Relationship.DomesticPartner).Any() && (dependent.Relationship == Models.Relationship.Spouse || dependent.Relationship == Models.Relationship.DomesticPartner)) return (null, "Each employee can't have more than one spouse or domestic partner.");

            employee.Dependents.Add(dependent);

            return (employee, "");
        }

        /*
         * As I said on the controller, this would be
         * the first thing to be changed after creating
         * a route and model just for the 
         * salary/paychecks/deductions.
         * 
         * I also was in doubt about the deductions
         * values as I don't know if it would be better 
         * to store them in a DB or having them placed
         * in app config files.
         * 
         * It would be better to store them in the DB
         * if these values change a lot because it would
         * not have to be built and released to
         * production every time it changes. I would store
         * them in config files if they are not that
         * volatile to free some DB space.
         * 
         * In eitheer case I would not hard code it in
         * here. I just coded them here to focus on other 
         * more important requirements for this challenge.
         */

        public decimal CalculateEmployeePaycheck(GetEmployeeDto employee)
        {
            decimal deduction = 0.00m;

            decimal employeeDeduction = 1000.00m * 12;

            deduction += employeeDeduction;

            if (employee.Dependents.Count > 0)
            {
                DateTime currentDate = DateTime.Now;

                decimal dependentsDeduction = (employee.Dependents.Count * 600.00m) * 12;

                foreach(var dependent in employee.Dependents)
                {
                    var age = currentDate.Year - dependent.DateOfBirth.Year;

                    if (age >= 50)
                    {
                        dependentsDeduction += (200.00m * 12);
                    }
                }

                deduction += dependentsDeduction;
            }

            var paycheck = (employee.Salary - deduction) / 26;

            if (employee.Salary >= 80000.00m) paycheck -= (employee.Salary * 0.02m);

            return Decimal.Round(paycheck, 2);
        }
    }
}
