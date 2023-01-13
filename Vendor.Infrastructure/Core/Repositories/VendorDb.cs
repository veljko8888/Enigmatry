using Shared.Models.Contracts;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vendor.Infrastructure.Core.Repositories
{
    public class VendorDb : IDb
    {
        private List<ArticleDto> _articles = new List<ArticleDto>();

        public ArticleDto GetById(int id)
        {
            return _articles.Single(x => x.ID == id);
        }

        public void Save(ArticleDto article)
        {
            _articles.Add(article);
        }
    }
}
