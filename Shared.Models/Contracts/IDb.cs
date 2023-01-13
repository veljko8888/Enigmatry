using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Contracts
{
    public interface IDb
    {
        public ArticleDto GetById(int id);

        public void Save(ArticleDto article);
    }
}
