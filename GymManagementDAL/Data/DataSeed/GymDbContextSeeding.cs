using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            var HasPlans = dbContext.Plans.Any();
            var HasCategories = dbContext.Categories.Any();
            if (HasPlans && HasCategories)
            {
                return false;
            }
            try
            {
                if (!HasPlans)
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        dbContext.AddRange(Plans);

                    }
                }

                if (!HasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                    {
                        dbContext.AddRange(categories);
                    }
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }

        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName) {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",fileName);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();
            var Data = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(Data))
            {
                return new List<T> { };
            }
            var result  = JsonSerializer.Deserialize<List<T>>(Data,new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            if (result is null)
                return new List<T>();
            return result;
        } 

    }
}
