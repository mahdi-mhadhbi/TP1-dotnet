using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using EmployeeApp.Models.Repositories;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        // Declare a readonly field for IRepository<Employee>
        private readonly IRepository<Employee> employeeRepository;

        // Constructor for dependency injection
        public EmployeeController(IRepository<Employee> empRepository)
        {
            employeeRepository = empRepository;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employees = employeeRepository.GetAll();  // Récupère tous les employés
            if (employees == null)
            {
                return View(new List<Employee>());  // Passe une liste vide à la vue si la valeur est null
            }
            ViewData["EmployeesCount"] = employees.Count();
            ViewData["SalaryAverage"] = employeeRepository.SalaryAverage();
            ViewData["MaxSalary"] = employeeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = employeeRepository.HrEmployeesCount();
            return View(employees);  // Passe la liste des employés à la vue
        }


        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            var employee = employeeRepository.FindByID(id);  // Trouve l'employé par son ID
            if (employee == null)
            {
                return NotFound();  // Retourne une erreur 404 si l'employé n'est pas trouvé
            }
            return View(employee);
        }


        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)  // Vérifie que le modèle est valide
            {
                try
                {
                    employeeRepository.Add(employee);  // Ajoute un nouvel employé
                    return RedirectToAction(nameof(Index));  // Redirige vers l'action Index après la création
                }
                catch
                {
                    return View(employee);  // Retourne à la vue Create en cas d'erreur
                }
            }
            return View(employee);  // Retourne à la vue avec les erreurs de validation si le modèle est invalide
        }


        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.FindByID(id);  // Find employee by ID
            if (employee == null)
            {
                return NotFound();  // Return 404 if employee not found
            }
            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee newEmployee)
        {
            if (ModelState.IsValid)  // Vérifie que le modèle est valide
            {
                try
                {
                    employeeRepository.Update(id, newEmployee);  // Met à jour l'employé
                    return RedirectToAction(nameof(Index));  // Redirige vers l'action Index après la mise à jour
                }
                catch
                {
                    return View(newEmployee);  // Retourne à la vue en cas d'erreur
                }
            }
            return View(newEmployee);  // Retourne à la vue avec les erreurs de validation
        }


        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = employeeRepository.FindByID(id);  // Find employee by ID
            if (employee == null)
            {
                return NotFound();  // Return 404 if employee not found
            }
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var employee = employeeRepository.FindByID(id);
            if (employee == null)
            {
                return NotFound();  // Si l'employé n'existe pas, retourne une erreur 404
            }

            try
            {
                employeeRepository.Delete(id);  // Supprime l'employé par son ID
                return RedirectToAction(nameof(Index));  // Redirige vers l'action Index après la suppression
            }
            catch
            {
                return View(employee);  // Retourne à la vue en cas d'erreur
            }
        }
        public ActionResult Search(string term)
        {

            var result = employeeRepository.Search(term);
            return View("Index", result);
        }

    }
}
