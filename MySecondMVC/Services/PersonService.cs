using MySecondMVC.Enums;
using MySecondMVC.Models;
using MySecondMVC.Repositories;

namespace MySecondMVC.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public List<Person> GetAll() => _personRepository.GetAll();

        public List<Person> GetMales() => _personRepository.GetAll().Where(p => p.Gender == Gender.Male).ToList();

        public Person? GetOldest() => _personRepository.GetAll().OrderByDescending(p => p.Age).FirstOrDefault();

        public List<string> GetFullNames() => _personRepository.GetAll().Select(p => p.FullName).ToList();

        public List<Person> FilterByBirthYear(int year, string filterType)
        {
            return filterType switch
            {
                "equal" => _personRepository.GetAll().Where(p => p.DateOfBirth.Year == year).ToList(),
                "before" => _personRepository.GetAll().Where(p => p.DateOfBirth.Year < year).ToList(),
                "after" => _personRepository.GetAll().Where(p => p.DateOfBirth.Year > year).ToList(),
                _ => new List<Person>()
            };
        }

        public Person? GetById(Guid id) => _personRepository.GetById(id);

        public void Add(Person person) => _personRepository.Add(person);

        public void Update(Person person) => _personRepository.Update(person);

        public void Delete(Guid id) => _personRepository.Delete(id);

        public List<Person> GetPaged(int page, int pageSize)
        {
            return _personRepository.GetAll()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int GetTotalCount()
        {
            return _personRepository.GetAll().Count;
        }
    }
}