using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.Models
{
    public class ClothingItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(100), NotNull]
        public string Name { get; set; } = string.Empty;

        [Indexed]
        public int CategoryID { get; set; }
        [Ignore]
        public Category Category 
        { 
            get 
            {
                return App.Database.GetCategory(CategoryID);
            } 
        }
        [Indexed]
        public int MaterialID { get; set; }
        [Ignore]
        public Material Material
        {
            get
            {
                return App.Database.GetMaterial(MaterialID);
            }
        }

        public string ImageURL { get; set; } = string.Empty;
        [Ignore]
        public ImageSource Source
        {
            get
            {
                return ImageSource.FromFile(ImageURL);
            }
        }

        public string RecommendationText { get; set; } = string.Empty;
        
        private List<CareSymbol> _careSymbols=[];
        [Ignore]
        public List<CareSymbol> CareSymbols
        {
            get => _careSymbols;
            set
            {
                _careSymbols = value;
            }
        }
        public void GenerateRecomenadtionText()
        {
            if (CareSymbols.Count == 0)
            {
                RecommendationText = string.Empty;
            }
            string wash =String.Empty;
            if(CareSymbols.Any(s => s.Code == "no_wash"))
            {
                wash = "Одяг не можна прати. Зверніться до хімчистки.";
            }
            else
            {
                if(CareSymbols.Any(s => s.Code == "hand_wash"))
                {
                    wash = "Одяг можна прати тільки вручну. Але не хвилюйтесь, адже багато пральних машин мають програми делікатного ручного прання, які так само бережуть ваш одяг.";
                }
                else
                {
                    if(CareSymbols.Any(s => s.Code == "mashine_wash_delicate"))
                    {
                        wash = "Одяг потребує делікатного прання.  Не використовуйте віджим, якщо це можливо.";
                    }
                    else
                    {
                        if (CareSymbols.Any(s => s.Code == "mashine_wash_perm_press"))
                        {
                            wash = "Одяг можна прати на короткому циклі прання.";
                        }                            
                    }
                   
                }
                var tempSymbols = CareSymbols.Where(s => s.Code.StartsWith("wash_")).ToList();
                var temperatures = tempSymbols
                    .Select(s => int.TryParse(new string(s.Code.Where(char.IsDigit).ToArray()), out var temp) ? temp : int.MaxValue)
                    .ToList();
                int minTemperature = temperatures.Any() ? temperatures.Min() : 30;
                wash += $" Прати за температури '{minTemperature}' або менше.";
            }

            if (CareSymbols.Any(s => s.Code == "no_wring"))
            {
                wash += " Одяг не можна віджимати";
            }
            else
            {
                if (CareSymbols.Any(s => s.Code == "wring"))
                {
                    wash += " Одяг можна віджимати";
                }
            }
            Console.WriteLine(wash);
            var bleach="\n";
            if(CareSymbols.Any(s => s.Code == "no_bleach"))
            {
                bleach += "Не можна використовувати відбілювач.";
            }
            else
            {
                if (CareSymbols.Any( s=> s.Code == "no_chlorine_bleach"))
                {
                    bleach += "Bи можете використовувати відбілювач для відбілювання одягу, але переконайтеся, що це не хлор.";
                }
                else
                {
                    if(CareSymbols.Any(s=> s.Code == "chlorine"))
                    {
                        bleach += "Bаш одяг можна відбілити хлорним відбілювачем. ";
                    }
                    else
                    {
                        if(CareSymbols.Any(s => s.Code == "bleach"))
                        {
                            bleach += "Будь - який тип відбілювача дозволений.";
                        }
                    }
                    
                }
            }
            Console.WriteLine (bleach);
            var iron = "\n";
            if (CareSymbols.Any(s => s.Code == "no_iron"))
            {
                iron += "Прасувати одяг небезпечно";
            }
            else
            {
                if (CareSymbols.Any(s => s.Code == "iron_low"))
                {
                    iron += "Надзвичайно ніжне прасування при температурі максимум 110°С.";
                }
                else
                {
                    if (CareSymbols.Any(s => s.Code == "iron_medium"))
                    {
                        iron += "Прасування при температурі максимум 150°С.";
                    }
                    else
                    {
                        if (CareSymbols.Any(s => s.Code == "iron_high"))
                        {
                            iron += "Прасування при температурі максимум  200°C.";
                        }
                        else
                        {
                            if (CareSymbols.Any(s => s.Code == "iron"))
                            {
                                iron += "Прасування підходить для вашого одягу.";
                            }
                        }
                    }
                }
            }

            if (CareSymbols.Any(s => s.Code == "no_steam"))
            {
                iron += " Одяг можна прасувати, доки ви не користуєтеся функцією пари.";
            }
            else 
            {
                if (CareSymbols.Any(s => s.Code == "steam"))
                {
                    iron += " Прасування з пропарюванням підходить для вашого одягу.";
                }
            }
            Console.WriteLine(iron);
            var clean = "\n";
            if (CareSymbols.Any(s => s.Code == "no_dry_clean"))
            {
                clean += "Хімчистка заборонена. ";
            }
            else
            {
                if (CareSymbols.Any(s => s.Code == "dry_clean"))
                {
                    clean += "Хімчистка дозволена. ";
                }
            }
            if (CareSymbols.Any(s => s.Code == "no_wet_clean"))
            {
                clean += "Волога професійна чистка заборонена. ";
            }
            else
            {
                if (CareSymbols.Any(s => s.Code == "wet_clean"))
                {
                    clean += "Волога професійна чистка дозволена. ";
                }
            }
            var dry = "\n";
            if (CareSymbols.Any(s => s.Code == "no_dry"))
            {
                dry += "Не сушити. ";
            }
            else
            {
                if (CareSymbols.Any(s => s.Code == "drip_dry_shade"))
                {
                    dry += "Сушити без віджимання у тіні. ";
                }
                else
                {
                    if (CareSymbols.Any(s => s.Code == "drip_dry"))
                    {
                        dry += "Сушити без віджимання. ";
                    }
                    else
                    {
                        if (CareSymbols.Any(s => s.Code == "flat_dry"))
                        {
                            dry += "Сушити на горизонтальній поверхні. ";
                        }
                        else
                        {
                            if (CareSymbols.Any(s => s.Code == "line_dry_shade"))
                            {
                                dry += "Сушити на мотузці у вертикальному положенні у тіні. ";
                            }
                            else
                            {
                                if (CareSymbols.Any(s => s.Code == "line_dry"))
                                {
                                    dry += "Сушити на мотузці у вертикальному положенні. ";
                                }
                                else
                                {
                                    if (CareSymbols.Any(s => s.Code == "shade_dry"))
                                    {
                                        dry += "Сушити в тіні. ";
                                    }
                                    else
                                    {
                                        if (CareSymbols.Any(s => s.Code == "natural_dry"))
                                        {
                                            dry += "Сушити на повітрі. ";
                                        }
                                        else
                                        {
                                            if (CareSymbols.Any(s => s.Code == "no_tumble_dry"))
                                            {
                                                dry += "Не можна сушити в сушильній машині. ";
                                            }
                                            else
                                            {
                                                if (CareSymbols.Any(s => s.Code == "tumble_dry_no_heat"))
                                                {
                                                    dry += "Сушіння в сушильній машині - без нагрівання. ";
                                                }
                                                else
                                                {
                                                    if (CareSymbols.Any(s => s.Code == "tumble_dry_low"))
                                                    {
                                                        dry += "Сушіння в сушильній машині - Максимальна температура – до 60°C. ";
                                                    }
                                                    else
                                                    {
                                                        if (CareSymbols.Any(s => s.Code == "tumble_dry_medium"))
                                                        {
                                                            dry += "Сушіння в сушильній машині - Максимальна температура – до 80°C. ";
                                                        }
                                                        else
                                                        {
                                                            if (CareSymbols.Any(s => s.Code == "tumble_dry_high"))
                                                            {
                                                                dry += "Сушіння в сушильній машині - Висока температура. ";
                                                            }
                                                            else
                                                            {
                                                                if (CareSymbols.Any(s => s.Code == "tumble_dry"))
                                                                {
                                                                    dry += "Можна сушити в сушильній машині. ";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(clean);
            var recommendations = String.Empty;
            recommendations += wash+bleach+iron+clean+dry;
            RecommendationText = recommendations;
        }

    }

    public class ClothingItemCareSymbol
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Indexed]
        public int CareSymbolID { get; set; }

        [Indexed]
        public int ClothingItemID {  get; set; }
    }
}
