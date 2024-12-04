using LaundryScan.Models;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace LaundryScan
{
    public class DbController
    {
        SQLiteConnection? Database;

        public DbController()
        {
        }
        [MemberNotNull(nameof(Database))]
        void Init()
        {
            if (Database is not null)
            { 
                return;
            }
            if (!File.Exists(Constants.DatabasePath))
            {
                using var dbInResourses= FileSystem.OpenAppPackageFileAsync("LaundryScanData.db3").GetAwaiter().GetResult();
                using var target = new FileStream(Constants.DatabasePath, FileMode.Create, FileAccess.Write, FileShare.None);
                dbInResourses.CopyTo(target);
            }
            Database = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

        }

        public List<Category> GetCategories()
        {
            Init();
            return Database.Table<Category>().ToList();
        }
        public Category GetCategory(int id)
        {
            Init();
            return Database.Table<Category>().Where(i => i.ID == id).FirstOrDefault();
        }
        public int SaveCategory(Category category)
        {
            Init();
            if (category.ID != 0)
            {
                return Database.Update(category);

            }
            else
            {
                return Database.Insert(category);
            }
                
            
        }
        public int DeleteCategory(Category category)
        {
            Init();
            return Database.Delete(category);
        }


        public List<Material> GetMaterials()
        {
            Init();
            return Database.Table<Material>().ToList();
        }
        public Material GetMaterial(int id)
        {
            Init();
            return Database.Table<Material>().Where(i => i.ID == id).FirstOrDefault();
        }
        public int SaveMaterial(Material material)
        {
            Init();
            if (material.ID != 0)
            {
                return Database.Update(material);

            }
            else
            {
                return Database.Insert(material);
            }


        }
        public int DeleteMaterial(Material material)
        {
            Init();
            return Database.Delete(material);
        }


        public List<CareSymbol> GetCareSymbols()
        {
            Init();
            return Database.Table<CareSymbol>().ToList();
        }
        public CareSymbol GetCareSymbol(int id)
        {
            Init();
            return Database.Table<CareSymbol>().Where(i => i.ID == id).FirstOrDefault();
        }

        public CareSymbol GetCareSymbolByCode(string code)
        {
            Init();
            return Database.Table<CareSymbol>().Where(i => i.Code == code).FirstOrDefault();
        }
        public int SaveCareSymbol(CareSymbol careSymbol)
        {
            Init();
            if (careSymbol.ID != 0)
            {
                return Database.Update(careSymbol);

            }
            else
            {
                return Database.Insert(careSymbol);
            }


        }
        public int DeleteCareSymbol(CareSymbol careSymbol)
        {
            Init();
            return Database.Delete(careSymbol);
        }


        public List<ClothingItemCareSymbol> GetClothingItemCareSymbols()
        {
            Init();
            return Database.Table<ClothingItemCareSymbol>().ToList();
        }
        public ClothingItemCareSymbol GetClothingItemCareSymbol(int id)
        {
            Init();
            return Database.Table<ClothingItemCareSymbol>().Where(i => i.ID == id).FirstOrDefault();
        }
        public int SaveClothingItemCareSymbol(ClothingItemCareSymbol clothingItemCareSymbol)
        {
            Init();
            if (clothingItemCareSymbol.ID != 0)
            {
                return Database.Update(clothingItemCareSymbol);

            }
            else
            {
                return Database.Insert(clothingItemCareSymbol);
            }


        }
        public int DeleteClothingItemCareSymbol(ClothingItemCareSymbol clothingItemCareSymbol)
        {
            Init();
            return Database.Delete(clothingItemCareSymbol);
        }


        public List<ClothingItem> GetClothingItems()
        {
            Init();
            var items= Database.Table<ClothingItem>().ToList();
            for(int i =0; i<items.Count();i++)
            {
                items[i] = GetCareSymbolsForItem(items[i]);
            }
            return items;
        }
        public ClothingItem GetClothingItem(int id)
        {
            Init();
            var item = Database.Table<ClothingItem>().Where(i => i.ID == id).FirstOrDefault();
            if (item != null)
            {
                item=GetCareSymbolsForItem(item);
            }
            return item;
        }
        public int SaveClothingItem(ClothingItem clothingItem)
        {
            Init();
            if (clothingItem.ID != 0)
            {
                Database.Table<ClothingItemCareSymbol>().Delete(i => i.ClothingItemID == clothingItem.ID);
                if (clothingItem.CareSymbols != null)
                {
                    foreach (var careSymbol in clothingItem.CareSymbols)
                    {
                        var careSymbolClothingItem = new ClothingItemCareSymbol
                        {
                            CareSymbolID = careSymbol.ID,
                            ClothingItemID = clothingItem.ID
                        };
                        Database.Insert(careSymbolClothingItem);
                    }
                }
                clothingItem.GenerateRecomenadtionText();
                return Database.Update(clothingItem);

            }
            else
            {
                clothingItem.GenerateRecomenadtionText();
                var rows=Database.Insert(clothingItem);
                clothingItem.ID = Database.Table<ClothingItem>().Last().ID;
                if (clothingItem.CareSymbols != null)
                {
                    foreach (var careSymbol in clothingItem.CareSymbols)
                    {
                        var careSymbolClothingItem = new ClothingItemCareSymbol
                        {
                            CareSymbolID = careSymbol.ID,
                            ClothingItemID = clothingItem.ID
                        };
                        Database.Insert(careSymbolClothingItem);
                    }
                }
                return rows;
            }


        }
        public int DeleteClothingItem(ClothingItem clothingItem)
        {
            Init();

            Database.Table<ClothingItemCareSymbol>().Delete(i => i.ClothingItemID == clothingItem.ID);
            return Database.Delete(clothingItem);
        }
        public ClothingItem GetCareSymbolsForItem(ClothingItem clothingItem)
        {
            Init();
            var con = this.GetClothingItemCareSymbols();
            var ids = con.Where(i=>i.ClothingItemID==clothingItem.ID).Select(i=>i.CareSymbolID).ToList();
            var symbols = Database.Table<CareSymbol>().Where(s=>ids.Contains(s.ID)).ToList();
            clothingItem.CareSymbols = new List<CareSymbol>();
            foreach(CareSymbol symbol in symbols)
                clothingItem.CareSymbols.Add(symbol);

            return clothingItem;
        }


        public List<CareSymbolGroup> GetCareSymbolsGroups()
        {
            Init();
            var groups = Database.Table<CareSymbolGroup>().ToList();
            for (int i = 0; i < groups.Count(); i++)
            {
                groups[i] = GetCareSymbolsForGroup(groups[i]);
            }
            return groups;
        }
        public CareSymbolGroup GetCareSymbolGroup(int id)
        {
            Init();
            var group = Database.Table<CareSymbolGroup>().Where(i => i.ID == id).FirstOrDefault();
            if (group != null)
            {
                group = GetCareSymbolsForGroup(group);
            }
            return group;
        }
        public CareSymbolGroup GetCareSymbolsForGroup(CareSymbolGroup careSymbolGroup)
        {
            Init();
            var con = this.GetCareSymbols();
            var symbols = con.Where(i => i.GroupID == careSymbolGroup.ID).ToList();
            careSymbolGroup.CareSymbols = new List<CareSymbol>();
            foreach (CareSymbol symbol in symbols)
                careSymbolGroup.CareSymbols.Add(symbol);

            return careSymbolGroup;
        }


        public List<OutfitClothingItem> GetOutfitClothingItems()
        {
            Init();
            return Database.Table<OutfitClothingItem>().ToList();
        }
        public OutfitClothingItem GetOutfitClothingItem(int id)
        {
            Init();
            return Database.Table<OutfitClothingItem>().Where(i => i.ID == id).FirstOrDefault();
        }
        public int SaveOutfitClothingItem(OutfitClothingItem outfitClothingItem)
        {
            Init();
            if (outfitClothingItem.ID != 0)
            {
                return Database.Update(outfitClothingItem);

            }
            else
            {
                return Database.Insert(outfitClothingItem);
            }


        }
        public int DeleteOutfitClothingItem(OutfitClothingItem outfitClothingItem)
        {
            Init();
            return Database.Delete(outfitClothingItem);
        }


        public List<Outfit> GetOutfits()
        {
            Init();
            var outfits = Database.Table<Outfit>().ToList();
            for (int i = 0; i < outfits.Count(); i++)
            {
                outfits[i] = GetClothingItemsForOutfit(outfits[i]);
            }
            return outfits;
        }
        public Outfit GetOutfit(int id)
        {
            Init();
            var outfit = Database.Table<Outfit>().Where(i => i.ID == id).FirstOrDefault();
            if (outfit != null)
            {
                outfit = GetClothingItemsForOutfit(outfit);
            }
            return outfit;
        }
        public int SaveOutfit(Outfit outfit)
        {
            Init();
            if (outfit.ID != 0)
            {
                Database.Table<OutfitClothingItem>().Delete(i => i.OutfitID == outfit.ID);
                if (outfit.ClothingItems != null)
                {
                    foreach (var clothingItem in outfit.ClothingItems)
                    {
                        var outfitClothingItem = new OutfitClothingItem
                        {
                            OutfitID = outfit.ID,
                            ClothingItemID = clothingItem.ID
                        };
                        Database.Insert(outfitClothingItem);
                    }
                }
                outfit.GenerateRecomenadtionText();
                return Database.Update(outfit);

            }
            else
            {
                outfit.GenerateRecomenadtionText();
                var rows = Database.Insert(outfit);
                outfit.ID = Database.Table<Outfit>().Last().ID;
                if (outfit.ClothingItems != null)
                {
                    foreach (var clothingItem in outfit.ClothingItems)
                    {
                        var outfitClothingItem = new OutfitClothingItem
                        {
                            OutfitID = outfit.ID,
                            ClothingItemID = clothingItem.ID
                        };
                        Database.Insert(outfitClothingItem);
                    }
                }
                return rows;
            }


        }
        public int DeleteOutfit(Outfit outfit)
        {
            Init();

            Database.Table<OutfitClothingItem>().Delete(i => i.OutfitID == outfit.ID);
            return Database.Delete(outfit);
        }
        public Outfit GetClothingItemsForOutfit(Outfit outfit)
        {
            Init();
            var con = this.GetOutfitClothingItems();
            var ids = con.Where(i => i.OutfitID == outfit.ID).Select(i => i.ClothingItemID).ToList();
            var items = Database.Table<ClothingItem>().Where(s => ids.Contains(s.ID)).ToList();
            outfit.ClothingItems = new List<ClothingItem>();
            foreach (ClothingItem item in items)
                outfit.ClothingItems.Add(item);

            return outfit;
        }
    }
}
