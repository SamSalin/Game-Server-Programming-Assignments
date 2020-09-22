using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class ModifiedItem
    {
        public int Level { get; set; }
        public ItemType Type { get; set; }
        public DateTime CreationDate { get; set; }
    }
}