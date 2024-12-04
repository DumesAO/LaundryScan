using SQLite;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LaundryScan.Models
{
    public class Category
    {
        

        [PrimaryKey, AutoIncrement]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        public string Name { get; set; }

    }
}
