using Application.Contexts;
using Application.Features.DictionaryKeywords.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Seeds
{
    public static class DefaultDictionaryKeywords
    {
        public static async Task SeedAsync(ApplicationContext context)
        {
            var initialKeywords = new List<string> { "bonjour", "encore", "télévision", "coaguler", "sœur", "journée", "journal" };
            var anyKeywordAdded = false;
            foreach (var keyword in initialKeywords)
            {
                var alreadyExist = await context.DictionaryKeywords.AnyAsync(k=>k.Keyword == keyword);
                if (!alreadyExist)
                {
                    await context.AddAsync(new DictionaryKeyword { Keyword = keyword });
                    anyKeywordAdded = true;
                }
            }

            if (anyKeywordAdded)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
