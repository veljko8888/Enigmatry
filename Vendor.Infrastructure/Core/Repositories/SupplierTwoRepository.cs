using Shared.Models.Contracts;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vendor.Infrastructure.Core.Repositories
{
    public class SupplierTwoRepository : IInventory
    {
        private Dictionary<int, ArticleDto> _supplierTwoArticles = new Dictionary<int, ArticleDto>()
        {
            { 31, new ArticleDto { ID = 31, ArticlePrice = 110, Name_of_article = "Article 31" } },
            { 32, new ArticleDto { ID = 32, ArticlePrice = 120, Name_of_article = "Article 32" } },
            { 33, new ArticleDto { ID = 33, ArticlePrice = 130, Name_of_article = "Article 33" } },
            { 34, new ArticleDto { ID = 34, ArticlePrice = 140, Name_of_article = "Article 34" } },
            { 35, new ArticleDto { ID = 35, ArticlePrice = 150, Name_of_article = "Article 35" } },
            { 36, new ArticleDto { ID = 36, ArticlePrice = 200, Name_of_article = "Article 36" } },
            { 37, new ArticleDto { ID = 37, ArticlePrice = 220, Name_of_article = "Article 37" } },
            { 38, new ArticleDto { ID = 38, ArticlePrice = 240, Name_of_article = "Article 38" } },
            { 39, new ArticleDto { ID = 39, ArticlePrice = 260, Name_of_article = "Article 39" } },
            { 40, new ArticleDto { ID = 40, ArticlePrice = 280, Name_of_article = "Article 40" } },
        };

        public ArticleDto GetArticle(int id)
        {
            try
            {
                ArticleDto article = null;
                if (_supplierTwoArticles.ContainsKey(id))
                {
                    _supplierTwoArticles.TryGetValue(id, out article);
                }

                return article;
            }
            catch (ArgumentNullException)
            {
                //logger add
                return null;
            }
        }
    }
}
