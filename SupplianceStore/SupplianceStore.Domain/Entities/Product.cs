using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplianceStore.Domain.Entities
{
   public class Product
    {
        [HiddenInput(DisplayValue = false)]

        public int ProductId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название товара")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание для товара")]
        public string Description { get; set; }
        [Display(Name = "Короткое описание")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Пожалуйста, укажите короткое описание")]
        public string Slug { get; set; }
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для товара")]
        public string Category { get; set; }
        [Display(Name = "Цена (грн)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
        [Display(Name= "Скидка(%)")]
        [Range(0, 100, ErrorMessage = "Пожалуйста, введите значение от 1 до 100")]
        public int Discount { get; set; }//In persengate
        [Display(Name="Колво шт")]
        [Range(0,int.MaxValue,ErrorMessage ="Значение должно быть положительным")]
        public int Emount { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Product()
        {
            Reviews = new List<Review>();
        }
    }
}
