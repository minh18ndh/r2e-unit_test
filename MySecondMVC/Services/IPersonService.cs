using MySecondMVC.Models;

namespace MySecondMVC.Services
{
    public interface IPersonService
    {
        List<Person> GetAll();
        List<Person> GetMales();
        Person? GetOldest();
        List<string> GetFullNames();
        List<Person> FilterByBirthYear(int year, string filterType);
        Person? GetById(Guid id);
        void Add(Person person);
        void Update(Person person);
        void Delete(Guid id);
        List<Person> GetPaged(int page, int pageSize);
        int GetTotalCount();
    }
}