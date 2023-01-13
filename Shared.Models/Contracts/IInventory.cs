using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Contracts
{
    public interface IInventory
    {
        public ArticleDto GetArticle(int id);
    }
}
