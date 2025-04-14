using MySecondMVC.Models;

namespace MySecondMVC.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Person? GetById(Guid id);
        void Add(Person person);
        void Update(Person person);
        void Delete(Guid id);
    }
}