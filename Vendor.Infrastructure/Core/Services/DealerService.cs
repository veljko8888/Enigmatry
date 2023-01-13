using Microsoft.Extensions.Logging;
using Shared.Models.Contracts;
using Shared.Models.ResponseBuilder;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Infrastructure.Core.Repositories;

namespace Vendor.Infrastructure.Core.Services
{
    public class DealerService : IShopService
    {
        private readonly ILogger<DealerService> _log;
        private readonly IEnumerable<IInventory> _supplierRepositories;
        private readonly IDb _db;

        public DealerService(
            IEnumerable<IInventory> supplierRepositories,
            IDb db,
            ILogger<DealerService> log)
        {
            _supplierRepositories = supplierRepositories;
            _db = db;
            _log = log;
        }

        public async Task<Result<ArticleDto>> GetArticle(int id, int maxExpectedPrice = 0, bool isDealerOne = true)
        {
            return await Task.Run(() =>
            {
                Result<ArticleDto> result = new Result<ArticleDto>();
                ArticleDto tmpArticle = null;

                var supplier = isDealerOne 
                                ? _supplierRepositories.FirstOrDefault(x => x.GetType() == typeof(SupplierOneRepository))
                                : _supplierRepositories.FirstOrDefault(x => x.GetType() == typeof(SupplierTwoRepository));
                tmpArticle = supplier != null ? supplier.GetArticle(id) : null;
                if (tmpArticle != null)
                {
                    return ResponseManager<ArticleDto>.GenerateServiceSuccessResult(ResultStatus.Success, tmpArticle, result);
                }

                _log.LogInformation("Article does not exist.");
                return ResponseManager<ArticleDto>.GenerateErrorServiceResult(
                                                    ResultStatus.NotFound,
                                                    "There is no article with provided id.",
                                                    result);
            });
        }

        public async Task<Result<bool>> BuyArticle(ArticleDto article, int buyerId, bool isDealerOne = true)
        {
            return await Task.Run(() =>
            {
                Result<bool> result = new Result<bool>();
                if (article == null)
                {
                    return ResponseManager<bool>.GenerateErrorServiceResult(
                                                    ResultStatus.InvalidParameters,
                                                    "Could not order article.",
                                                    result);
                }
                else
                {
                    var supplier = isDealerOne
                                ? _supplierRepositories.FirstOrDefault(x => x.GetType() == typeof(SupplierOneRepository))
                                : _supplierRepositories.FirstOrDefault(x => x.GetType() == typeof(SupplierTwoRepository));
                    var tmpArticle = supplier != null ? supplier.GetArticle(article.ID) : null;
                    if (tmpArticle != null)
                    {
                        HelperManager.StoreBoughtArticle(_db, article, buyerId);
                        return ResponseManager<bool>.GenerateServiceSuccessResult(ResultStatus.Success, true, result);
                    }

                    return ResponseManager<bool>.GenerateErrorServiceResult(
                                                    ResultStatus.InvalidParameters,
                                                    "Failed to buy article.",
                                                    result);
                }
            });
        }
    }
}
