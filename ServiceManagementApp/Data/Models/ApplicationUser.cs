using Microsoft.AspNetCore.Identity;
using ServiceManagementApp.Data.Models.Service;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey(nameof(EmployeeId))] //TODO да се обясни в дипломната рабоа, че това не е задължително защото Entitiy Framework сам открива връзките когато дадено навигационно свойство е от рода на EmployeeId, той сам открива че това е външен ключ, но го дабавяме тук за по добра четимост на кода !
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        [ForeignKey(nameof(ClientId))]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
    }
}
