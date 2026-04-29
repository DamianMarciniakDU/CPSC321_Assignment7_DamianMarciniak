using CPSC321_Assignment7_DamianMarciniak.Data;
using CPSC321_Assignment7_DamianMarciniak.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPSC321_Assignment7_DamianMarciniak.Controllers
{
    public class UserController : Controller
    {

        public static int nextId = 1;

        public double totalBudget = 1_000_000;
        public double ITBudget = 500_000;
        public double BusinessBudget = 500_000;
        private readonly CompanyDbContext companyDbContext;

        public UserController(CompanyDbContext companyDbContext)
        {
            this.companyDbContext = companyDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListOfUsers()
        {
            var users = companyDbContext.Users.Where(u => u.DeletionComment == null).ToList();
            return View(users);
        }

        public IActionResult ListOfITUsers()
        {
            var itUsers = companyDbContext.Users.Where(u => u.Department == "IT" && u.DeletionComment == null).ToList();
            return View(itUsers);
        }

        public IActionResult ListOfBusinessUsers()
        {
            var businessUsers = companyDbContext.Users.Where(u => u.Department == "Business" && u.DeletionComment == null).ToList();
            return View(businessUsers);
        }

        public IActionResult ListOfDeletedUsers()
        {
            var deletedUsers = companyDbContext.Users.Where(u => u.DeletionComment != null).ToList();
            return View(deletedUsers);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel user)
        {
            if (companyDbContext.Users.Count() >= 10)
            {
                ViewBag.ErrorMessage = "Cannot add more than 10 users.";
                return View("CreateUser");
            }
            double currentTotalSalaries = companyDbContext.Users.Sum(u => u.Salary);
            double currentITDepartmentSalaries = companyDbContext.Users.Where(u => u.Department == "IT").Sum(u => u.Salary);
            double currentBusinessDepartmentSalaries = companyDbContext.Users.Where(u => u.Department == "Business").Sum(u => u.Salary);

            if (currentTotalSalaries + user.Salary > totalBudget)
            {
                ViewBag.ErrorMessage = "Adding this user would exceed the total budget.";
                return View("CreateUser");
            }

            if (user.Department == "IT" && currentITDepartmentSalaries + user.Salary > ITBudget)
            {
                ViewBag.ErrorMessage = "Adding this user would exceed the IT department budget.";
                return View("CreateUser");
            }

            if (user.Department == "Business" && currentBusinessDepartmentSalaries + user.Salary > BusinessBudget)
            {
                ViewBag.ErrorMessage = "Adding this user would exceed the Business department budget.";
                return View("CreateUser");
            }

            companyDbContext.Users.Add(user);
            await companyDbContext.SaveChangesAsync();
            return RedirectToAction("ListOfUsers");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModel updatedUser)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.YearsOfExperience = updatedUser.YearsOfExperience;
            user.Salary = updatedUser.Salary;
            await companyDbContext.SaveChangesAsync();
            return RedirectToAction("ListOfUsers");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserModel userModel)
        {
   
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == userModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.DeletionComment = userModel.DeletionComment;
            await companyDbContext.SaveChangesAsync();
            return RedirectToAction("ListOfUsers");
        }

        public IActionResult Details(int id)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult EditUserSalary(int id)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserSalary(UserModel updatedUser)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            double currentTotalSalaries = companyDbContext.Users.Where(u => u.Id != updatedUser.Id).Sum(u => u.Salary);
            double currentITDepartmentSalaries = companyDbContext.Users.Where(u => u.Department == "IT" && u.Id != updatedUser.Id).Sum(u => u.Salary);
            double currentBusinessDepartmentSalaries = companyDbContext.Users.Where(u => u.Department == "Business" && u.Id != updatedUser.Id).Sum(u => u.Salary);
            if (currentTotalSalaries + updatedUser.Salary > totalBudget)
            {
                ViewBag.ErrorMessage = "Updating this user's salary would exceed the total budget.";
                return View("EditUserSalary", user);
            }
            if (user.Department == "IT" && currentITDepartmentSalaries + updatedUser.Salary > ITBudget)
            {
                ViewBag.ErrorMessage = "Updating this user's salary would exceed the IT department budget.";
                return View("EditUserSalary", user);
            }
            if (user.Department == "Business" && currentBusinessDepartmentSalaries + updatedUser.Salary > BusinessBudget)
            {
                ViewBag.ErrorMessage = "Updating this user's salary would exceed the Business department budget.";
                return View("EditUserSalary", user);
            }
            user.Salary = updatedUser.Salary;
            await companyDbContext.SaveChangesAsync();
            return RedirectToAction("ListOfUsers");
        }

        [HttpGet]
        public IActionResult EditUserYearsOfExperience(int id)
        {
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserYearsOfExperience(UserModel updatedUser)
        { 
            var user = companyDbContext.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.YearsOfExperience = updatedUser.YearsOfExperience;
            await companyDbContext.SaveChangesAsync();
            return RedirectToAction("ListOfUsers");

        }
    }
}