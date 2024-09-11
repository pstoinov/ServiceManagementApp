using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagementApp.Data.Enums;

namespace ServiceManagementApp.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; } 
        // Клиентска информация
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Моля, въведете име на клиента")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете телефон на клиента")]
        [Phone(ErrorMessage = "Моля, въведете валиден телефонен номер")]
        public string ClientPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете имейл на клиента")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл")]
        public string ClientEmail { get; set; } = string.Empty;

        public int? ClientCompanyId { get; set; }
        public string? ClientCompanyName { get; set; }

        [Display(Name = "Фирма на клиента")]
        public int? SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem>? Companies { get; set; } 

        // Заявка
        [Required(ErrorMessage = "Моля, въведете описание на проблема")]
        [Display(Name = "Описание на проблема")]
        public string ProblemDescription { get; set; } = string.Empty;

        [Display(Name = "Приоритет")]
        public ServiceRequestPriority Priority { get; set; } = ServiceRequestPriority.Medium; 

        [Display(Name = "Тип на заявката")]
        public ServiceRequestType RequestType { get; set; }

        [Display(Name = "Статус на заявката")]
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.New; 

        [Display(Name = "Очаквана дата за приключване")]
        public DateTime ExpectedCompletionDate { get; set; }

        [Display(Name = "Дата на заявка")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public int CompletionDays { get; set; }

        [Display(Name = "Устройство")]
        public string? Device { get; set; }

        [Display(Name = "Аксесоари към устройството")]
        public string? Accessories { get; set; } 
  
        // Логнат служител
        public string? LoggedInEmployeeName { get; set; }
        public string? LoggedInEmployeePhone { get; set; }
        public string? LoggedInEmployeeEmail { get; set; }
        public string? LoggedInEmployeeService { get; set; }

        [Display(Name = "Номер на заявката")]
        public string RequestNumber { get; set; } = "R000001"; 
    }
}
