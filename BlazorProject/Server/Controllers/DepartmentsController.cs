using BlazorProject.Server.Repositories.DepartmentRepository;
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
    public class DepartmentsController : ControllerBase
    {
        private IDepartmentRepository _repository;

        public DepartmentsController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _repository.GetDepartments();
                if (departments.Any())
                {
                    return Ok(departments);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database. " + ex.Message);
            }        
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var department = await _repository.GetDepartment(id);
                if (department !=null)
                {
                    return Ok(department);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database. " + ex.Message);
            }
        }
    }
}
