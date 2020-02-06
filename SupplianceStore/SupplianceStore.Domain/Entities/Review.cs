using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplianceStore.Domain.Entities
{
     public class Review
    {
        
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите ваш отзыв")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, поставьте отценку")]
        [Range(0, 5, ErrorMessage = "Значение должно быть от 1 до 5")]
        public int Stars { get; set; }
       
        [HiddenInput(DisplayValue = false)]

        public DateTime Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? ProductId { get; set; }
        
    }
}
