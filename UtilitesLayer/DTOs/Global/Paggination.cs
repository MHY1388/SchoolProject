using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Global
{
    public class Paggination<T> where T : class
    {
        public List<T> Objects { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int GetSize { get; set; }
    }
}
