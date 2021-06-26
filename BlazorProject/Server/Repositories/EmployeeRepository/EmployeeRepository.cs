using BlazorProject.Server.Data;
using BlazorProject.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (employee.Department !=null)
            {
                _context.Entry(employee.Department).State = EntityState.Unchanged;
            }
            var result= await _context.Employee.AddAsync(employee);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteEmployee(int id)
        {
            var result = _context.Employee.FirstOrDefault(e=>e.EmployeeId==id);
            if (result !=null)
            {
                _context.Employee.Remove(result);
                await _context.SaveChangesAsync();
            }
           
        }

        public async Task<Employee> GetEmployee(int id)
        {
           return await _context.Employee
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _context.Employee
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employee.Include(e => e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            var query =  _context.Employee.Include(e=>e.Department).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.ToUpper().Contains(name.ToUpper()) 
                                      || e.LastName.ToUpper().Contains(name.ToUpper()));
            }

            if (gender !=null)
            {
                query = query.Where(e => e.Gender == gender);
            }
            return await query.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var empToUpdate = await GetEmployee(employee.EmployeeId);
            if (empToUpdate !=null)
            {
                empToUpdate.FirstName = employee.FirstName;
                empToUpdate.LastName = employee.LastName;
                empToUpdate.DateOfBrith = employee.DateOfBrith;             
                empToUpdate.Gender = employee.Gender;
                empToUpdate.Email = employee.Email;
                if (employee.DepartmentId !=0)
                {
                    empToUpdate.DepartmentId = employee.DepartmentId;
                } else if (employee.Department !=null)
                {
                    empToUpdate.DepartmentId = employee.Department.DepartmentId;
                }
                empToUpdate.PhotoPath = employee.PhotoPath;
                // var result = _context.Employee.Update(employee).Entity;
                await _context.SaveChangesAsync();
                return empToUpdate;
            }


            return null;
        }
    }
}
