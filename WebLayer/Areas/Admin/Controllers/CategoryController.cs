﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;

namespace WebLayer.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string title = null, int page=1)
        {
            if (title.IsNullOrEmpty()){
                var da = _unitOfWork.Categories.GetPaggination(page,6);
                return View(da);
            }

            var data = _unitOfWork.Categories.GetPaggination(page, 6, title, title);
            return View(data);
        }

        public IActionResult Add()
        {
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public IActionResult Add(CreateCategoryDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = _unitOfWork.Categories.CreateCategory(model);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("","عملیات با شکست مواجه شد");
                return View(model);
            }
            _unitOfWork.SaveChanges();
            return RedirectAndShowAlert(new OperationResult(){Message = "عملیات با موفقیت انجام شد", Status = OperationResultStatus.Success},RedirectToAction("Index", "Category"));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result  = _unitOfWork.Categories.DeleteCategory(id);
            _unitOfWork.SaveChanges();
            return RedirectAndShowAlert(result, RedirectToAction("Index"));
        }

        public IActionResult Update(int id)
        {
            var model = _unitOfWork.Categories.GetCategory(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(CategoryDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = _unitOfWork.Categories.UpdateCategory(model);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("","عملیات با شکست مواجه شد");
                return View(model);
            }
            _unitOfWork.SaveChanges();
            return RedirectAndShowAlert(new OperationResult()
                { Message = "با مفقیت بروزرسانی شد", Status = OperationResultStatus.Success }, RedirectToAction("Index"));
        }
    }
}
