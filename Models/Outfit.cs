using SQLite;

namespace LaundryScan.Models
{
    public class Outfit
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [NotNull]
        public string Name { get; set; }
        private List<ClothingItem> _clothingItems = [];
        [Ignore]
        public List<ClothingItem> ClothingItems
        {
            get => _clothingItems;
            set
            {
                _clothingItems = value;
            }
        }
        public string? RecomendationText { get; set; } =string.Empty;

        public void AddClothingItem(ClothingItem clothingItem)
        {
            ClothingItems.Add(clothingItem);
        }
        public void GenerateRecomenadtionText()
        {
            if (ClothingItems.Count == 0)
            {
                RecomendationText = string.Empty;
                return;
            }
            List<CareSymbol> careSymbols = new List<CareSymbol>();
            foreach(ClothingItem clothingItem in ClothingItems)
            {
                careSymbols.AddRange(clothingItem.CareSymbols);
            }
            string wash = String.Empty;
            if (careSymbols.Any(s => s.Code == "no_wash"))
            {
                wash = "Одяг не можна прати. Зверніться до хімчистки.";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "hand_wash"))
                {
                    wash = "Одяг можна прати тільки вручну. Але не хвилюйтесь, адже багато пральних машин мають програми делікатного ручного прання, які так само бережуть ваш одяг.";
                }
                else
                {
                    if (careSymbols.Any(s => s.Code == "mashine_wash_delicate"))
                    {
                        wash = "Одяг потребує делікатного прання.  Не використовуйте віджим, якщо це можливо.";
                    }
                    else
                    {
                        if (careSymbols.Any(s => s.Code == "mashine_wash_perm_press"))
                        {
                            wash = "Одяг можна прати на короткому циклі прання.";
                        }
                    }

                }
                var tempSymbols = careSymbols.Where(s => s.Code.StartsWith("wash_")).ToList();
                var temperatures = tempSymbols
                    .Select(s => int.TryParse(new string(s.Code.Where(char.IsDigit).ToArray()), out var temp) ? temp : int.MaxValue)
                    .ToList();
                int minTemperature = temperatures.Any() ? temperatures.Min() : 30;
                wash += $" Прати за температури '{minTemperature}' або менше.";
            }
            if (careSymbols.Any(s => s.Code == "no_wring"))
            {
                wash += " Одяг не можна віджимати";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "wring"))
                {
                    wash += " Одяг можна віджимати";
                }
            }

            var bleach = "\n";
            if (careSymbols.Any(s => s.Code == "no_bleach"))
            {
                bleach += "Не можна використовувати відбілювач.";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "no_chlorine_bleach"))
                {
                    bleach += "Bи можете використовувати відбілювач для відбілювання одягу, але переконайтеся, що це не хлор.";
                }
                else
                {
                    if (careSymbols.Any(s => s.Code == "chlorine"))
                    {
                        bleach += "Bаш одяг можна відбілити хлорним відбілювачем. ";
                    }
                    else
                    {
                        if (careSymbols.Any(s => s.Code == "bleach"))
                        {
                            bleach += "Будь - який тип відбілювача дозволений.";
                        }
                    }

                }
            }
            var iron = "\n";
            if (careSymbols.Any(s => s.Code == "no_iron"))
            {
                iron += "Прасувати одяг небезпечно";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "iron_low"))
                {
                    iron += "Надзвичайно ніжне прасування при температурі максимум 110°С.";
                }
                else
                {
                    if (careSymbols.Any(s => s.Code == "iron_medium"))
                    {
                        iron += "Прасування при температурі максимум 150°С.";
                    }
                    else
                    {
                        if (careSymbols.Any(s => s.Code == "iron_high"))
                        {
                            iron += "Прасування при температурі максимум  200°C.";
                        }
                        else
                        {
                            if (careSymbols.Any(s => s.Code == "iron"))
                            {
                                iron += "Прасування підходить для вашого одягу.";
                            }
                        }
                    }
                }
            }

            if (careSymbols.Any(s => s.Code == "no_steam"))
            {
                iron += " Одяг можна прасувати, доки ви не користуєтеся функцією пари.";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "steam"))
                {
                    iron += " Прасування з пропарюванням підходить для вашого одягу.";
                }
            }
            var clean = "\n";
            if (careSymbols.Any(s => s.Code == "no_dry_clean"))
            {
                clean += "Хімчистка заборонена. ";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "dry_clean"))
                {
                    clean += "Хімчистка дозволена. ";
                }
            }
            if (careSymbols.Any(s => s.Code == "no_wet_clean"))
            {
                clean += "Волога професійна чистка заборонена. ";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "wet_clean"))
                {
                    clean += "Волога професійна чистка дозволена. ";
                }
            }
            var dry = "\n";
            if (careSymbols.Any(s => s.Code == "no_dry"))
            {
                dry += "Не сушити. ";
            }
            else
            {
                if (careSymbols.Any(s => s.Code == "drip_dry_shade"))
                {
                    dry += "Сушити без віджимання у тіні. ";
                }
                else
                {
                    if (careSymbols.Any(s => s.Code == "drip_dry"))
                    {
                        dry += "Сушити без віджимання. ";
                    }
                    else
                    {
                        if (careSymbols.Any(s => s.Code == "flat_dry"))
                        {
                            dry += "Сушити на горизонтальній поверхні. ";
                        }
                        else
                        {
                            if (careSymbols.Any(s => s.Code == "line_dry_shade"))
                            {
                                dry += "Сушити на мотузці у вертикальному положенні у тіні. ";
                            }
                            else
                            {
                                if (careSymbols.Any(s => s.Code == "line_dry"))
                                {
                                    dry += "Сушити на мотузці у вертикальному положенні. ";
                                }
                                else
                                {
                                    if (careSymbols.Any(s => s.Code == "shade_dry"))
                                    {
                                        dry += "Сушити в тіні. ";
                                    }
                                    else
                                    {
                                        if (careSymbols.Any(s => s.Code == "natural_dry"))
                                        {
                                            dry += "Сушити на повітрі. ";
                                        }
                                        else
                                        {
                                            if (careSymbols.Any(s => s.Code == "no_tumble_dry"))
                                            {
                                                dry += "Не можна сушити в сушильній машині. ";
                                            }
                                            else
                                            {
                                                if (careSymbols.Any(s => s.Code == "tumble_dry_no_heat"))
                                                {
                                                    dry += "Сушіння в сушильній машині - без нагрівання. ";
                                                }
                                                else
                                                {
                                                    if (careSymbols.Any(s => s.Code == "tumble_dry_low"))
                                                    {
                                                        dry += "Сушіння в сушильній машині - Максимальна температура – до 60°C. ";
                                                    }
                                                    else
                                                    {
                                                        if (careSymbols.Any(s => s.Code == "tumble_dry_medium"))
                                                        {
                                                            dry += "Сушіння в сушильній машині - Максимальна температура – до 80°C. ";
                                                        }
                                                        else
                                                        {
                                                            if (careSymbols.Any(s => s.Code == "tumble_dry_high"))
                                                            {
                                                                dry += "Сушіння в сушильній машині - Висока температура. ";
                                                            }
                                                            else
                                                            {
                                                                if (careSymbols.Any(s => s.Code == "tumble_dry"))
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
            var recommendations = String.Empty;
            recommendations += wash + bleach + iron + clean + dry;
            RecomendationText = recommendations;
        }
    }

    public class OutfitClothingItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Indexed]
        public int OutfitID { get; set; }

        [Indexed]
        public int ClothingItemID { get; set; }
    }
}
