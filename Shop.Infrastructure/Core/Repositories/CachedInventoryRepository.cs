using Shared.Models.Contracts;
using Shared.Models.Shop;
using Shop.Application.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Core.Repositories
{
    public class CachedInventoryRepository : IInventory, IInventoryStore
    {
        private Dictionary<int, ArticleDto> _cachedArticles = new Dictionary<int, ArticleDto>() 
        {
            { 1, new ArticleDto { ID = 1, ArticlePrice = 110, Name_of_article = "Article 1" } },
            { 2, new ArticleDto { ID = 2, ArticlePrice = 120, Name_of_article = "Article 2" } },
            { 3, new ArticleDto { ID = 3, ArticlePrice = 130, Name_of_article = "Article 3" } },
            { 4, new ArticleDto { ID = 4, ArticlePrice = 140, Name_of_article = "Article 4" } },
            { 5, new ArticleDto { ID = 5, ArticlePrice = 150, Name_of_article = "Article 5" } },
            { 6, new ArticleDto { ID = 6, ArticlePrice = 200, Name_of_article = "Article 6" } },
            { 7, new ArticleDto { ID = 7, ArticlePrice = 220, Name_of_article = "Article 7" } },
            { 8, new ArticleDto { ID = 8, ArticlePrice = 240, Name_of_article = "Article 8" } },
            { 9, new ArticleDto { ID = 9, ArticlePrice = 260, Name_of_article = "Article 9" } },
            { 10, new ArticleDto { ID = 10, ArticlePrice = 280, Name_of_article = "Article 10" } },
        };

        public ArticleDto GetArticle(int id)
        {
            try
            {
                ArticleDto article = null;
                if (_cachedArticles.ContainsKey(id))
                {
                    _cachedArticles.TryGetValue(id, out article);
                }
                
                return article;
            }
            catch (ArgumentNullException)
            {
                //logger add
                return null;
            }
        }

        public void SetArticle(ArticleDto article)
        {
            try
            {
                if (!_cachedArticles.ContainsKey(article.ID))
                {
                    _cachedArticles.Add(article.ID, article);
                }
            }
            catch (Exception ex)
            {
                //logger add
            }
        }
    }
}
