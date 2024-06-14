using System;
using Xunit;
using Api.Dtos.Employee;
using Api.Services;
using Api.Repositories.Interfaces;
using Moq;
using Api.Dtos.Dependent;
using System.Collections.Generic;
using Api.Models;

/*
 * About Unit Testing:
 * I really like to use Dependency Injection in any 
 * javascript/typescript or C# project because I'm used
 * to high-mocking London style for testing purpose. It 
 * is really easy to assure the functionality of any part
 * of the system when it comes to Unit Testing.
 * 
 * If I decided to work with other patern, like Factory,
 * I would choose to work with Chicago style for unit
 * testing, with less mocks. To tell the truth I have some
 * dificulties in this type of style as I'm not used to
 * work with it.
 * 
 * That doesn't mean that I would never work with
 * low-mocking testing styles. I have no fear on learning
 * new techniques or technologies, and that applies to 
 * testing standards. I can work with Chicago style testing
 * if I have to. But I chose to be more comfortable in this
 * coding challenge.
 */

namespace ApiTests.UnitTests.Services
{
    public class EmployeeServiceTests
    {
        /*
         * If you follow the steps in my repository, you will see that
         * I coded this project using TDD. I made these tests first 
         * (according to the requirements on calculations), made it 
         * pass (green light), made it fail (red light) and then I coded
         * with making the tests pass in mind.
         */

        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        // I really like to use the System Under Test term so I can focus on what unit I whant to test.
        private readonly EmployeeService _sut;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _sut = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Fact]
        public void CalculateEmployeePaycheck_FromEmployeeWithBasicDeduction()
        {
            var employee = new GetEmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Salary = 64000.00m,
                DateOfBirth = new DateTime(1984, 12, 30)
            };

            decimal expectedPaycheck = 2000.00m;

            decimal actualPaycheck = _sut.CalculateEmployeePaycheck(employee);

            Assert.Equal(expectedPaycheck, actualPaycheck);
        }

        [Fact]
        public void CalculateEmployeePaycheck_FromEmployeeWithOneDependent()
        {
            var employee = new GetEmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Salary = 71200.00m,
                DateOfBirth = new DateTime(1984, 12, 30),
                Dependents = new List<GetDependentDto>
                { 
                    new ()
                    {
                        Id = 1,
                        FirstName = "Jane",
                        LastName = "Doe",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    }
                }
            };
            
            decimal expectedPaycheck = 2000.00m;

            decimal actualPaycheck = _sut.CalculateEmployeePaycheck(employee);

            Assert.Equal(expectedPaycheck, actualPaycheck);
        }

        [Fact]
        public void CalculateEmployeePaycheck_FromEmployeeWithOneOldDependent()
        {
            var employee = new GetEmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Salary = 73600.00m,
                DateOfBirth = new DateTime(1984, 12, 30),
                Dependents = new List<GetDependentDto>
                {
                    new ()
                    {
                        Id = 1,
                        FirstName = "Jane",
                        LastName = "Doe",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1973, 3, 3)
                    }
                }
            };

            decimal expectedPaycheck = 2000.00m;

            decimal actualPaycheck = _sut.CalculateEmployeePaycheck(employee);

            Assert.Equal(expectedPaycheck, actualPaycheck);
        }

        [Fact]
        public void CalculateEmployeePaycheck_FromEmployeeWithSalaryOverEightyK()
        {
            var employee = new GetEmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Salary = 80000.00m,
                DateOfBirth = new DateTime(1984, 12, 30)
            };

            decimal expectedPaycheck = 1015.38m;

            decimal actualPaycheck = _sut.CalculateEmployeePaycheck(employee);

            Assert.Equal(expectedPaycheck, actualPaycheck);
        }
    }
}
