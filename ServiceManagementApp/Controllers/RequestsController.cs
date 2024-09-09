using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.ViewModels;
using ServiceManagementApp.Data.Models.RequestModels;
using ServiceManagementApp.Data.Enums;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagementApp.Data.Models.Core;
using System.Globalization;

namespace ServiceManagementApp.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RequestsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new RequestViewModel
            {
                RequestDate = DateTime.Now,
                ExpectedCompletionDate = DateTime.Now,
                RequestNumber = GenerateRequestNumber(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {


                // Логика за търсене на клиента по име, телефон или имейл
                var client = await _context.Clients
                    .Include(c => c.Email)
                    .Include(c => c.Phone)
                    .FirstOrDefaultAsync(c => c.FullName == model.ClientName
                        || c.Phone.PhoneNumber == model.ClientPhone
                        || c.Email.EmailAddress == model.ClientEmail);
            
                if (client == null)
                {
                    // Ако няма такъв клиент, създаваме нов
                    client = new Client
                    {
                        FullName = model.ClientName,
                        Phone = new Phone { PhoneNumber = model.ClientPhone },
                        Email = new Email { EmailAddress = model.ClientEmail }
                    };
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                }

                // Проверка дали клиентът е свързан с фирма
                if (model.ClientCompanyId.HasValue)
                {
                    var clientCompany = _context.ClientCompanies
                        .FirstOrDefault(cc => cc.ClientId == client.Id && cc.CompanyId == model.ClientCompanyId);

                    // Ако връзката не съществува, я създаваме
                    if (clientCompany == null)
                    {
                        clientCompany = new ClientCompany
                        {
                            ClientId = client.Id,
                            CompanyId = model.ClientCompanyId.Value
                        };
                        _context.ClientCompanies.Add(clientCompany);
                        await _context.SaveChangesAsync();
                    }
                }
                int days = GetCompletionDays(model.Priority);
                var fake = model.ExpectedCompletionDate = DateTime.Now.AddDays(days);

                // Логика за създаване на нова заявка
                var serviceRequest = new ServiceRequest
                {
                    ClientId = client.Id,
                    ClientCompanyId = model.ClientCompanyId, // Връзка с фирма, ако има
                    RequestNumber = model.RequestNumber,
                    RequestDate = model.RequestDate,
                    Status = model.Status,
                    Priority = model.Priority,
                    ProblemDescription = model.ProblemDescription,
                    ExpectedCompletionDate = fake,
                    Device = model.Device,
                    Accessories = model.Accessories
                };

                // Връзка със служителя, който е логнат
                //var user = await _userManager.GetUserAsync(User);
                //var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmailAddress.EmailAddress == user.Email);
                //if (employee != null)
                //{
                //    serviceRequest.EmployeeId = employee.Id;
                //}

                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            PopulateDropdowns();
            return View(model);
        }

        // Логика за генериране на номер на заявка
        private string GenerateRequestNumber()
        {
            var lastRequest = _context.ServiceRequests.OrderByDescending(r => r.Id).FirstOrDefault();
            if (lastRequest == null)
            {
                return "R000001";
            }
            else
            {
                int nextNumber = int.Parse(lastRequest.RequestNumber.Substring(1)) + 1;
                return "R" + nextNumber.ToString("D6");
            }
        }



        private int GetCompletionDays(ServiceRequestPriority priority)
        {
            if (priority == ServiceRequestPriority.High)
            {
                return 1;
            }
            else if (priority == ServiceRequestPriority.Medium)
            {
                return 3;
            }
            else if (priority == ServiceRequestPriority.Low)
            {
                return 5;
            }
            else
            {
                return 5; // Default to 5 if no matching priority
            }
        }
        private void PopulateDropdowns()
        {
            ViewBag.RequestTypes = Enum.GetValues(typeof(ServiceRequestType))
                                      .Cast<ServiceRequestType>()
                                      .Select(e => new SelectListItem
                                      {
                                          Value = ((int)e).ToString(),
                                          Text = e == ServiceRequestType.OnSite ? "В сервиз" : "На адрес"
                                      }).ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(ServiceRequestStatus))
                                   .Cast<ServiceRequestStatus>()
                                   .Select(e => new SelectListItem
                                   {
                                       Value = ((int)e).ToString(),
                                       Text = e switch
                                       {
                                           ServiceRequestStatus.New => "Нова",
                                           ServiceRequestStatus.InProgress => "В прогрес",
                                           ServiceRequestStatus.OnHold => "Задържана",
                                           ServiceRequestStatus.Completed => "Завършена",
                                           ServiceRequestStatus.Cancelled => "Отменена",
                                           _ => e.ToString()
                                       }
                                   }).ToList();

            ViewBag.Priorities = Enum.GetValues(typeof(ServiceRequestPriority))
                                     .Cast<ServiceRequestPriority>()
                                     .Select(e => new SelectListItem
                                     {
                                         Value = ((int)e).ToString(),
                                         Text = e switch
                                         {
                                             ServiceRequestPriority.Low => "Нисък",
                                             ServiceRequestPriority.Medium => "Среден",
                                             ServiceRequestPriority.High => "Висок",
                                             _ => e.ToString()
                                         }
                                     }).ToList();
        }
    }
}
