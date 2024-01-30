using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
		
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
			
            return View(objCompanyList);
        }
	
		public IActionResult Upsert(int? id)
		{
			
			if (id==null || id == 0)
			{
				//Create
				return View(new Company());
			}
			else
			{
				//Update
				Company companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
				return View(companyFromDb);

			}
		}

		[HttpPost]
		public IActionResult Upsert(Company companyObj)
		{
			if (ModelState.IsValid)
			{

			
				if (companyObj.Id == 0)
				{
					_unitOfWork.Company.Add(companyObj);
                    TempData["success"] = "Company created successfully";
                }
				else
				{
					_unitOfWork.Company.Update(companyObj);
                    TempData["success"] = "Company edited successfully";
                }
				
				_unitOfWork.Save();
				
				return RedirectToAction("Index");
			}
			else
			{
				return View(companyObj);
			}
		}

		
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
			return Json(new { data = objCompanyList });
		}

 
        public IActionResult Delete(int? id)
        {
			var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted== null)
            {
				return Json(new { success = false, message = "Error while deleting" });
            }

			_unitOfWork.Company.Remove(companyToBeDeleted);
			_unitOfWork.Save();

           
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}

