using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications.EmpolyeeSpec;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{
   
    public class EmployeesController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeesController(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
            var employees = await _employeeRepo.GetAllWithSpecAsync(spec);
            return Ok(employees);
        }

        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeById(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await _employeeRepo.GetAllWithSpecAsync(spec);
            if (employee is null)
                return NotFound(new ApiResponse(404));
            return Ok(employee);
        }
    }
}
