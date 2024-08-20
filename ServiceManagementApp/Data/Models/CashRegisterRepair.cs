namespace ServiceManagementApp.Data.Models
{
    public class CashRegisterRepair
    {
        public int Id { get; set; }
        public DateTime StartRepairDate { get; set; }
        public DateTime EndRepairDate { get; set; }

        public string Description { get; set; } = null!;
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; } = null!;
    }
}
