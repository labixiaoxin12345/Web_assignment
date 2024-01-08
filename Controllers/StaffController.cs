﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;
namespace P02_WebApr2023_Assg1_Team6.Controllers
{
	public class StaffController : Controller
	{
		private StaffDAL staffContext = new StaffDAL();
		// GET: StaffController
		public ActionResult Index()
		{
			return View();
		}
		// GET: StaffController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}
		// GET: StaffController/Create
		public ActionResult Create()
		{
			return View();
		}
		// POST: StaffController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
		// GET: StaffController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}
		// POST: StaffController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
		// GET: StaffController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}
		// POST: StaffController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}