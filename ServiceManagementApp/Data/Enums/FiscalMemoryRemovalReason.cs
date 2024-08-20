namespace ServiceManagementApp.Data.Enums
{
    public enum FiscalMemoryRemovalReason
    {
        FullMemory = 1,  // Препълване на фискалната памет
        OwnershipChange = 2,  // Смяна на собственика
        RegistrationTermination = 3,  // Прекратяване на регистрация по инициатива на лицето
        Disposal = 4,  // Бракуване
        MemoryFailure = 5,  // Повреда на фискалната памет, която не позволява разчитането й
        BlockError = 6,  // Грешка в блок на фискалната памет
        CommissioningError = 7,  // Грешка при въвеждане в експлоатация на фискалното устройство
        TestCompletion = 8  // След приключване на изпитване на ЕСФП в реални условия
    }
}
