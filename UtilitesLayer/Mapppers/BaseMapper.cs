using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs;

namespace UtilitesLayer.Mapppers
{
    public class BaseMapper
    {
        public static T2 BaseMap<T1,T2>(T1 t1, T2 t2) where T1:BaseEntity where T2 : BaseDto
        {
            t2.Id = t1.Id;
            t2.Updated = t1.Updated;
            t2.Created = t1.Created;
            return t2 ;
        }
    }
}
