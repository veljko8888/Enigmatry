using Shared.Models.Shop;
using Shop.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Core.Interfaces
{
    public interface IDealerCommunicator
    {
        public Task<ArticleDto> GetArticle(int id, HttpCommunicatorEnum httpDealer);
        public Task<bool> BuyArticle(ArticleDto article, HttpCommunicatorEnum httpDealer);
    }
}
