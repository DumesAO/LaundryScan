using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.Models
{
    public class CareSymbol
    {
        [PrimaryKey, AutoIncrement]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Description { get; set; }
        [NotNull]
        public string ImageURL { get; set; }
        [Ignore]
        public ImageSource Source 
        { 
            get 
            { 
                return ImageSource.FromFile(ImageURL);
            } 
        }
        [Indexed]
        public int GroupID { get; set; }

        [NotNull]
        public string Code { get; set; }

    }
}
