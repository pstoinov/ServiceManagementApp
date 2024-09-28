using ServiceManagementApp.Data.Models.ServiceModels;
using ServiceManagementApp.Data;
using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetEmployeesByServiceId(int serviceId)
    {
        return await _context.Employees
            .Where(e => e.ServiceId == serviceId)
            .ToListAsync();
    }
}
