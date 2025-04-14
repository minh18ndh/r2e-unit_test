using MySecondMVC.Enums;

namespace MySecondMVC.Models
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender Gender { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public required string BirthPlace { get; set; }
        public required bool IsGraduated { get; set; }

        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public string FullName => $"{LastName} {FirstName}";
    }
}