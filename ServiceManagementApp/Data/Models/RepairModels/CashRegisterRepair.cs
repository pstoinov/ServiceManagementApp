using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.RepairModels
{
    public class CashRegisterRepair
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartRepairDate { get; set; }
        [Required]
        public DateTime EndRepairDate { get; set; }
        [Required]
        public DateTime TakenByClient { get; set; }
        [Required]
        public string DescriptionOfProblem { get; set; } = string.Empty;
        [Required]
        public string DescriptionOfRepair { get; set; } = string.Empty;
        [ForeignKey(nameof(CashRegisterId))]
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; } = null!;
        public bool IsDisposed { get; set; } = false;
        public DisposalReason? DisposalReason { get; set; }
        [MaxLength(10)]
        public string LastReportBeforeRepair { get; set; } = string.Empty;
        [MaxLength(10)]
        public int NumberOfReceiptsDuringRepair { get; set; }
        [MaxLength(10)]
        public string LastReportAfterRepair { get; set; } = string.Empty;
        [ForeignKey(nameof(EmployeeId))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public ICollection<RepairPart>? RepairPart { get; set; } = new List<RepairPart>();
        public bool IsFiscalMemoryRemoved { get; set; } = false;  // Индикация за демонтаж на фискалната памет
        public string? NewFiscalMemoryNumber { get; set; }  // Номер на новата фискална памет, ако е подменена
        public FiscalMemoryRemovalReason? FiscalMemoryRemovalReason { get; set; }  // Причина за демонтаж на фискалната памет
    }
}

//TODO Да се добави това в контролера за ремонтите
//public void ProcessRepair(CashRegisterRepair repair)
//{
//    // Проверка дали ремонтът води до бракуване на касовия апарат
//    if (repair.IsDisposed)
//    {
//        var cashRegister = GetCashRegisterById(repair.CashRegisterId);
//        if (cashRegister != null)
//        {
//            cashRegister.IsDisposed = true;
//            UpdateCashRegister(cashRegister); // Запазване на промените в базата данни
//        }
//    }
//}
