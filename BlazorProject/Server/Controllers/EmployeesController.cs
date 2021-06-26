using BlazorProject.Server.Repositories.EmployeeRepository;
using BlazorProject.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var result = await _repository.GetEmployees();
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database. " + ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _repository.GetEmployee(id);
                if (result !=null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database. " + ex.Message);
            }
        }


        //No need to check model state. If model state is not valid "404 Bad request result is created."
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
              
                var employeeByEmail = await _repository.GetEmployeeByEmail(model.Email);

                //Creating custom model error
                if (employeeByEmail != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return BadRequest(ModelState);
                }
                var employee=await _repository.AddEmployee(model);

                return CreatedAtAction(nameof(GetEmployee), new {Id=employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating resource. " + ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await GetEmployee(id);
                if (employee == null)
                {
                   
                    return NotFound("Resource not found.");
                }
               
                await _repository.DeleteEmployee(id);

                return Ok("Resource deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource. " + ex.Message);
            }
        }
        //No need to check model state. If model state is not valid "404 Bad request result is created."
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee model)
        {
            try
            {
                //Creating custom model error
                if (id != model.EmployeeId)
                {
                    ModelState.AddModelError("EmployeeId", "Id mismatch.");
                    return BadRequest(ModelState);
                }

                var empToUpdate = await _repository.GetEmployee(id);
                if (empToUpdate ==null)
                {
                    return NotFound("Resource to update not found.");
                }
                var result=await _repository.UpdateEmployee(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating resource. " + ex.Message);
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {
                var result=await _repository.Search(name, gender);

                if (!result.Any())
                {          
                    return NotFound();
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data. " + ex.Message);
            }
        }

    }
}
