using SQLite;
using System.ComponentModel.DataAnnotations;

namespace LaundryScan.Models
{
    public class Material
    {


        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

    }
}
