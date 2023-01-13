using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Shop
{
    public class ArticleDto
    {
        public int ID { get; set; }

        public string Name_of_article { get; set; }

        public int ArticlePrice { get; set; }
        public bool IsSold { get; set; }

        public DateTime SoldDate { get; set; }
        public int BuyerUserId { get; set; }
    }
}
