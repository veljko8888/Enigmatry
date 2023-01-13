using Shared.Models.Contracts;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vendor.Infrastructure.Core.Repositories
{
    public class SupplierOneRepository : IInventory
    {
        private Dictionary<int, ArticleDto> _supplierOneArticles = new Dictionary<int, ArticleDto>()
        {
            { 21, new ArticleDto { ID = 21, ArticlePrice = 110, Name_of_article = "Article 21" } },
            { 22, new ArticleDto { ID = 22, ArticlePrice = 120, Name_of_article = "Article 22" } },
            { 23, new ArticleDto { ID = 23, ArticlePrice = 130, Name_of_article = "Article 23" } },
            { 24, new ArticleDto { ID = 24, ArticlePrice = 140, Name_of_article = "Article 24" } },
            { 25, new ArticleDto { ID = 25, ArticlePrice = 150, Name_of_article = "Article 25" } },
            { 26, new ArticleDto { ID = 26, ArticlePrice = 200, Name_of_article = "Article 26" } },
            { 27, new ArticleDto { ID = 27, ArticlePrice = 220, Name_of_article = "Article 27" } },
            { 28, new ArticleDto { ID = 28, ArticlePrice = 240, Name_of_article = "Article 28" } },
            { 29, new ArticleDto { ID = 29, ArticlePrice = 260, Name_of_article = "Article 29" } },
            { 30, new ArticleDto { ID = 30, ArticlePrice = 280, Name_of_article = "Article 30" } },
        };

        public ArticleDto GetArticle(int id)
        {
            try
            {
                ArticleDto article = null;
                if (_supplierOneArticles.ContainsKey(id))
                {
                    _supplierOneArticles.TryGetValue(id, out article);
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
