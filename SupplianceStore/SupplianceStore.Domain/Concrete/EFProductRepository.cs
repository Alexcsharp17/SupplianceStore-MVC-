using SupplianceStore.Domain.Abstract;
using SupplianceStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplianceStore.Domain.Concrete
{
   public class EFProductRepository : IProductRepository , IReviewRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
        
        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
                context.Products.Add(product);
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                    dbEntry.Discount = product.Discount;
                }
            }
            context.SaveChanges();
        }
        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public IEnumerable<Review> Reviews { get { return context.Reviews; } }

        public Review DeleteReview(int reviewId)
        {
            Review dbEntry = context.Reviews.Find(reviewId);
            if (dbEntry != null)
            {
                context.Reviews.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void SaveReview(Review review)
        {
            if (review.Id == 0)
                context.Reviews.Add(review);
            else
            {
                Review dbEntry = context.Reviews.Find(review.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Id = review.Id;
                    dbEntry.ProductId = review.ProductId;
                    dbEntry.Stars = review.Stars;
                    dbEntry.Description = review.Description;
                    dbEntry.Date = review.Date;
                }
            }
            context.SaveChanges();
        }
    }
}
