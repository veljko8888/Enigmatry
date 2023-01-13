using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Contracts
{
    public static class HelperManager
    {
        public static void StoreBoughtArticle(IDb db, ArticleDto article, int buyerId)
        {
            article.IsSold = true;
            article.SoldDate = DateTime.Now;
            article.BuyerUserId = buyerId;

            db.Save(article);
        }
    }
}
