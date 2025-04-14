using Microsoft.Extensions.Caching.Memory;
using MySecondMVC.Models;
using MySecondMVC.Enums;

namespace MySecondMVC.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private const string CacheKey = "PersonList";
        private readonly IMemoryCache _cache;

        public PersonRepository(IMemoryCache cache)
        {
            _cache = cache;

            // Initialize person list in cache if not already set
            if (!_cache.TryGetValue(CacheKey, out List<Person>? _))
            {
                var initialData = new List<Person>
                {
                    new Person { FirstName = "Minh", LastName = "Nguyen", Gender = Gender.Male, DateOfBirth = new DateOnly(2003, 6, 11), PhoneNumber = "0913234848", BirthPlace = "Vietnam", IsGraduated = true },
                    new Person { FirstName = "Van", LastName = "Vu", Gender = Gender.Female, DateOfBirth = new DateOnly(1999, 7, 15), PhoneNumber = "0948487413", BirthPlace = "Czech", IsGraduated = false },
                    new Person { FirstName = "Toan", LastName = "Le", Gender = Gender.Other, DateOfBirth = new DateOnly(1997, 4, 25), PhoneNumber = "0389943814", BirthPlace = "Poland", IsGraduated = false },
                    new Person { FirstName = "Ngoc", LastName = "Tran", Gender = Gender.Female, DateOfBirth = new DateOnly(2000, 11, 6), PhoneNumber = "0388449039", BirthPlace = "Thailand", IsGraduated = true },
                    new Person { FirstName = "Linh", LastName = "Do", Gender = Gender.Male, DateOfBirth = new DateOnly(2005, 3, 1), PhoneNumber = "0912294848", BirthPlace = "Myanmar", IsGraduated = true },
                };

                _cache.Set(CacheKey, initialData);
            }
        }

        public List<Person> GetAll() => _cache.Get<List<Person>>(CacheKey) ?? new List<Person>();

        public Person? GetById(Guid id) => GetAll().FirstOrDefault(p => p.Id == id);

        public void Add(Person person)
        {
            var people = GetAll();
            people.Add(person);
            _cache.Set(CacheKey, people);
        }

        public void Update(Person person)
        {
            var people = GetAll();
            var index = people.FindIndex(p => p.Id == person.Id);
            if (index != -1)
            {
                people[index] = person;
                _cache.Set(CacheKey, people);
            }
        }

        public void Delete(Guid id)
        {
            var people = GetAll();
            people = people.Where(p => p.Id != id).ToList();
            _cache.Set(CacheKey, people);
        }
    }
}