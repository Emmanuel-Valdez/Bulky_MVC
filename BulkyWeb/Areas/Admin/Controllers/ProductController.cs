using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
			
            return View(objProductList);
        }
		//public IActionResult Create()
		//{


		//	ProductVM productVM = new()
		//	{
		//		CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
		//		{
		//			Text = u.Name,
		//			Value = u.Id.ToString(),
		//		}),
		//		Product = new Product()
		//	};

		//	return View(productVM);
		//      }
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				}),
				Product = new Product()
			};
			if(id==null || id == 0)
			{
				//Create
				return View(productVM);
			}
			else
			{
				//Update
				productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
				return View(productVM);

			}

			
		}
		//[HttpPost]
		//      public IActionResult Create(ProductVM productVM)
		//      {

		//          if (ModelState.IsValid)
		//          {
		//              _unitOfWork.Product.Add(productVM.Product);
		//              _unitOfWork.Save();
		//              TempData["success"] = "Product created successfully";
		//              return RedirectToAction("Index");
		//	}
		//	else
		//	{
		//		productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
		//		{
		//			Text = u.Name,
		//			Value = u.Id.ToString(),
		//		});
		//		return View(productVM);
		//	}

		//      }
		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, IFormFile? file)
		{

			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(productVM.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			}
			else
			{
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				});
				return View(productVM);
			}

		}
		//public IActionResult Edit(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	Product? product = _unitOfWork.Product.Get(u => u.Id == id);
		//	//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
		//	//Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
		//	if (product == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(product);

		//}
		//[HttpPost]
		//public IActionResult Edit(Product obj)
		//{

		//	if (ModelState.IsValid)
		//	{
		//		_unitOfWork.Product.Update(obj);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Product updated successfully";
		//		return RedirectToAction("Index");
		//	}
		//	return View();

		//}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? product = _unitOfWork.Product.Get(u => u.Id == id);
			//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
			//Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
			if (product == null)
			{
				return NotFound();
			}
			return View(product);

		}
		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Product deleted successfully";
			return RedirectToAction("Index");


		}

	}
}