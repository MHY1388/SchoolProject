using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Teacher
{
    public class CreateTeacherDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Doc { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [DataType(DataType.PhoneNumber), Required]
        public string PhoneNumber { get; set; }
        public bool PublicPhoneNumber { get; set; } = true;
        [Required]
        public IFormFile Image { get; set; }
    }
    public class TeacherDto : BaseDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Doc { get; set; }
        [Required]
        public string FilePath { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [DataType(DataType.PhoneNumber), Required]
        public string PhoneNumber { get; set; }
        public bool PublicPhoneNumber { get; set; } = true;
    }
}
