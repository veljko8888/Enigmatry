using Microsoft.Extensions.Logging;
using Shared.Models.Contracts;
using Shared.Models.Shop;
using Shop.Application.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Shop.Infrastructure.Core.Repositories
{
    public class WarehouseInventoryRepository : IInventory
    {
        private readonly ILogger<WarehouseInventoryRepository> _log;

        public WarehouseInventoryRepository(
            ILogger<WarehouseInventoryRepository> log)
        {
            _log = log;
        }

        private Dictionary<int, ArticleDto> _warehouseArticles = new Dictionary<int, ArticleDto>()
        {
            { 11, new ArticleDto { ID = 11, ArticlePrice = 110, Name_of_article = "Article 11" } },
            { 12, new ArticleDto { ID = 12, ArticlePrice = 120, Name_of_article = "Article 12" } },
            { 13, new ArticleDto { ID = 13, ArticlePrice = 130, Name_of_article = "Article 13" } },
            { 14, new ArticleDto { ID = 14, ArticlePrice = 140, Name_of_article = "Article 14" } },
            { 15, new ArticleDto { ID = 15, ArticlePrice = 150, Name_of_article = "Article 15" } },
            { 16, new ArticleDto { ID = 16, ArticlePrice = 200, Name_of_article = "Article 16" } },
            { 17, new ArticleDto { ID = 17, ArticlePrice = 220, Name_of_article = "Article 17" } },
            { 18, new ArticleDto { ID = 18, ArticlePrice = 240, Name_of_article = "Article 18" } },
            { 19, new ArticleDto { ID = 19, ArticlePrice = 260, Name_of_article = "Article 19" } },
            { 20, new ArticleDto { ID = 20, ArticlePrice = 280, Name_of_article = "Article 20" } },
        };

        public ArticleDto GetArticle(int id)
        {
            try
            {
                _log.LogTrace("Get article from warehouse started.");
                ArticleDto article = null;
                if (_warehouseArticles.ContainsKey(id))
                {
                    _warehouseArticles.TryGetValue(id, out article);
                }

                _log.LogTrace($"Get article from warehouse finished, returning article with ID {id}.");
                return article;
            }
            catch (Exception ex)
            {
                _log.LogError($"Get article from warehouse failed with exception: {ex.Message}.");
                return null;
            }
        }
    }
}
