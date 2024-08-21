using Azure.Identity;
using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models;

namespace ServiceManagementApp.Data.Models
{
    public class CashRegisterRepair
    {
        public int Id { get; set; }
        public DateTime StartRepairDate { get; set; }
        public DateTime EndRepairDate { get; set; }
        /*TODO да се добави Описание на Дефекта
         * сервизна фирма
         * отстранена ли е пломбата
         * дата и час на приемане от клиента
        */
        public string DescriptionOfRepair { get; set; } = null!;
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; } = null!;
        public bool IsDisposed { get; set; } = false;
        public string LastReportBeforeRepair { get; set; } = null!;
        public int NumberOfReceiptsDuringRepair { get; set; }
        public string LastReportAfterRepair { get; set; } = null!;
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
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
