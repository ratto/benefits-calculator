﻿using Api.Dtos.Employee;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
    }
}