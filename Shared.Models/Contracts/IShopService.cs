using Shared.Models.ResponseBuilder;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Contracts
{
    public interface IShopService
    {
        public Task<Result<ArticleDto>> GetArticle(int id, int maxExpectedPrice = 0, bool isDealerOne = true);
        public Task<Result<bool>> BuyArticle(ArticleDto article, int buyerId, bool isDealerOne = true);
    }
}
