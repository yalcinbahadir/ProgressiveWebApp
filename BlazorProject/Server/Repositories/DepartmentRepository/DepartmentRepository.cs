using BlazorProject.Server.Data;
using BlazorProject.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department> GetDepartment(int id)
        {
            return await _context.Department.FirstOrDefaultAsync(d=>d.DepartmentId==id);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Department.ToListAsync();
        }
    }
}
