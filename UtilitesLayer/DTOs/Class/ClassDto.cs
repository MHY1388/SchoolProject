using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Class
{
    public class ClassDto
    {
        public int Id { get; set; }
        public int Grid { get; set; }
        public string Name { get; set; }
        public ICollection<User>? Students { get; set; }
        public ICollection<DataLayer.Entities.Day>? Days { get; set; }
    }
    public class CreateClassDto
    {
        [Display(Name = "پایه"), Required(ErrorMessage = "{0} اجباری است")]
        public int Grid { get; set; }
        [Display(Name = "نام"), Required(ErrorMessage = "{0} اجباری است")]
        public string Name { get; set; }
    }
}
