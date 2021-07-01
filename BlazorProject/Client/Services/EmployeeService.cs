using BlazorProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorProject.Client.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IHttpClientFactory _clientFactory;
       
        public const string clientName = "BlazorClient";

        public EmployeeService(IHttpClientFactory clientFactory, HttpClient client)
        {
            _clientFactory = clientFactory;
          
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            HttpClient httpClient = _clientFactory.CreateClient(clientName);
                          
            var response=await httpClient.GetAsync("api/employees");
            IEnumerable<Employee> employees=new List<Employee>();
            if (response.IsSuccessStatusCode)
            {
                employees=await response.Content.ReadAsAsync<IEnumerable<Employee>>();
            }
            return employees;
        }


        public Task<Employee> AddEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEmployeeByEmail(string email)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
