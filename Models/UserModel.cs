using System.ComponentModel.DataAnnotations;

namespace CPSC321_Assignment7_DamianMarciniak.Models
{
    public class UserModel
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public int YearsOfExperience { get; set; }
        public double Salary { get; set; }
        public string? DeletionComment { get; set; }
    }
}
