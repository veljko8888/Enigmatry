using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Core.Interfaces
{
    public interface IInventoryStore
    {
        public void SetArticle(ArticleDto article);
    }
}
