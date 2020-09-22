using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class NewItem
    {
        [Range(1, 99, ErrorMessage = "Incorrect Value! Values between {1} and {2} allowed.")]
        public int Level { get; set; }

        public ItemType Type { get; set; }

        [DataType(DataType.Date)]
        [DateValidation]
        public DateTime CreationDate { get; set; }
    }
}