namespace ServiceManagementApp.ViewModels
{
    public class PartViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        // Ако имаме допълнителна логика или полета (напр. форматирани стойности, специфични за View-то), можем да ги добавим тук.
    }
}
