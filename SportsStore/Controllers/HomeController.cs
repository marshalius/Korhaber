﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreRepository _repo;
        public int pageSize = 4;

        public HomeController(IStoreRepository repository)
        {

            _repo = repository;
        }
        public ViewResult Index(string category, int ProductPage = 1) =>
            View(new ProductListViewModel
            {
                Haberler = _repo.Haberler
                .Where(p => category == null || p.Kategori == category)
                .OrderBy(p => p.Id)
                .Skip((ProductPage - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = ProductPage,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    _repo.Haberler.Count() :
                    _repo.Haberler.Where(e =>
                    e.Kategori == category).Count()
                },
                CurrentCategory = category
            });


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
