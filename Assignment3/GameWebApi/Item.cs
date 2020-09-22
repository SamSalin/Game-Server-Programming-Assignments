using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi
{
    public enum ItemType
    {
        SWORD,
        POTION,
        SHIELD
    }
    public class Item
    {
        public Guid Id { get; set; }

        [Range(1, 99, ErrorMessage = "Incorrect Value! Values between {1} and {2} allowed.")]
        public int Level { get; set; }

        [EnumDataType(typeof(ItemType), ErrorMessage = "Invalid ItemType")]
        [Range(0, 2)]
        public ItemType Type { get; set; }

        //[DataType(DataType.Date)]
        [DateValidation]
        public DateTime CreationDate { get; set; }
    }
}