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
            var CompletionDays = GetCompletionDays(ServiceRequestPriority.Medium);
            return View(new RequestViewModel
            {
                RequestDate = DateTime.Now,
                RequestNumber = GenerateRequestNumber(),
                ExpectedCompletionDate = DateTime.Now.AddDays(7)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {

                DateTime parsedRequestDate;
                if (!DateTime.TryParseExact(model.RequestDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedRequestDate))
                {
                    ModelState.AddModelError("RequestDate", "Invalid date. Please enter the date in the format MM/dd/yyyy.");
                    PopulateDropdowns();
                    return View(model);
                }
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
                model.ExpectedCompletionDate = model.RequestDate.AddDays(model.CompletionDays);

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
                    ExpectedCompletionDate = model.ExpectedCompletionDate,
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

        // Изчисляване на очаквана дата за приключване
        private DateTime CalculateCompletionDate(ServiceRequestPriority priority)
        {
            return priority switch
            {
                ServiceRequestPriority.High => DateTime.Now.AddDays(1),
                ServiceRequestPriority.Medium => DateTime.Now.AddDays(3),
                _ => DateTime.Now.AddDays(5)
            };
        }

        private int GetCompletionDays(ServiceRequestPriority priority)
        {
            return priority switch
            {
                ServiceRequestPriority.High => 1,
                ServiceRequestPriority.Medium => 3,
                ServiceRequestPriority.Low => 5,
                _ => 5
            };
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
