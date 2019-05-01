using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeData.Models;
using Microsoft.AspNetCore.Http;
using EmployeeData.DAL;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EmployeeData.Controllers
{
    public class HomeController : Controller
    {
        public List<string[]> display = new List<string[]> { };
        public List<string> stringList = new List<string> { };
        public List<Employee> list = new List<Employee>();
        public readonly mySQLdbContext _context;

        public string[] SavedData;

        public HomeController(mySQLdbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        
            return View();
        }

        //Create Method [POST]
        [HttpPost]
        public ActionResult Create(IFormCollection formCollection)
        {
            string id = Request.Form["id"];
            string name = Request.Form["name"];
            string designation = Request.Form["designation"];
            string contact = Request.Form["contact"];

            try
            {
                Employee employee = new Employee();
                employee.empid = id;
                employee.empname = name;
                employee.empcontact = contact;
                employee.empdesignation = designation;
                _context.Add(employee);
                _context.SaveChanges();

            }catch(Exception e)
            {
                SavedData = getDataFromLocalDB();
                String data = id + ',' + name + ',' + designation + ',' + contact;
                saveToLocalDB(data);
            }

            return View();
           
       }

        // Read Method [GET]
        [HttpGet]
        public ActionResult Read()
        {
            try
            {
                list = _context.Employees.ToList();
                var empID = _context.Employees.Include(e=> e.empid);
                var empName = _context.Employees.Include(e => e.empname);
                var empDes = _context.Employees.Include(e => e.empdesignation);
                var empCon = _context.Employees.Include(e => e.empcontact);
        }
            catch (Exception ex)
            {
                SavedData = getDataFromLocalDB();
                foreach (var line in SavedData)
                {
                    string[] localData = new string[4];
                    localData = line.Split(",");
                    display.Add(localData);
                }
                ViewData["Display"] = display;
            }

            return View();
        }

        // Delete Method [POST]
        [HttpPost]
        public ActionResult Delete(IFormCollection formCollection)
        {

            string id = Request.Form["id"];
            
            try
            {

                Employee employee = _context.Employees.Where(e => e.empid == id).FirstOrDefault();
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                FindEntryByID(id);
            }

            return View();

        }

        // Update Method
        [HttpPost]
        public ActionResult Update(IFormCollection formCollection)
        {
            string id = Request.Form["id"];
            string name = Request.Form["name"];
            string designation = Request.Form["designation"];
            string contact = Request.Form["contact"];

            try
            {
                var employee = _context.Employees.SingleOrDefault(e => e.empid == id);

                if (employee != null)
                {
                    employee.empid = id;
                    employee.empname = name;
                    employee.empdesignation = designation;
                    employee.empcontact = contact;
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                FindEntryByID(id);
                String data = id + ',' + name + ',' + designation + ',' + contact;
                saveToLocalDB(data);
            }
         

            return View();

        }

        private string[] getDataFromLocalDB()
        {
            return System.IO.File.ReadAllLines("emp.dat");
        }

        private void saveToLocalDB(String Data)
        {
            System.IO.File.AppendAllText("emp.dat", Data + Environment.NewLine);
        }

        private void FindEntryByID(string id)
        {
            SavedData = getDataFromLocalDB();
            foreach (var line in SavedData)
            {
                stringList.Add(line.Split(",")[0]);
            }

            int counter = 0;
            foreach (string j in stringList)
            {
                if (j == id)
                {
                    break;
                }
                counter++;
            }
            stringList = SavedData.ToList();
            stringList.RemoveAt(counter);
            System.IO.File.WriteAllLines("emp.dat", stringList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
