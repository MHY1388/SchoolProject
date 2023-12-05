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
        public ICollection<Day>? Days { get; set; }
    }
}
