using System;
using Xunit;
using Api.Dtos.Employee;
using Api.Services;
using Api.Repositories.Interfaces;
using Moq;
using Api.Dtos.Dependent;
using System.Collections.Generic;
using Api.Models;

namespace ApiTests.UnitTests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
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
