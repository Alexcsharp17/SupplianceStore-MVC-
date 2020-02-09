using SupplianceStore.Domain.Abstract;
using SupplianceStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupplianceSrore.WebUI.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        
        private IReviewRepository repository;
       
        public ReviewController( IReviewRepository repo)
        {
            repository = repo;
            
        }
        [HttpGet]
        public ActionResult AddReview(int ProductId)
        {
            Review review = new Review();
            review.Date = DateTime.Now;
            review.ProductId = ProductId;
            return View(review);
        }

        [HttpPost]
        public ActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now.Date;
                repository.SaveReview(review);
                return RedirectToAction("List","Product");
            }
            else
            {
                return View(review);
            }
            
        }

    }
}