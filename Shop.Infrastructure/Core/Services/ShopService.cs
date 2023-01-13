using Microsoft.Extensions.Logging;
using Shared.Models.Contracts;
using Shared.Models.ResponseBuilder;
using Shared.Models.Shop;
using Shop.Application.Core.Interfaces;
using Shop.Application.Models;
using Shop.Infrastructure.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Core.Services
{
    public class ShopService : IShopService
    {
        private readonly ILogger<ShopService> _log;
        private readonly IEnumerable<IInventory> _inventoryServices;
        private readonly IInventoryStore _cacheStoreService;
        private readonly IDealerCommunicator _dealerCommunicator;
        private readonly IDb _db;

        public ShopService(
            IEnumerable<IInventory> inventoryServices,
            IInventoryStore cacheStoreService,
            IDealerCommunicator dealerCommunicator,
            IDb db,
            ILogger<ShopService> log)
        {
            _inventoryServices = inventoryServices;
            _cacheStoreService = cacheStoreService;
            _dealerCommunicator = dealerCommunicator;
            _db = db;
            _log = log;
        }

        public async Task<Result<ArticleDto>> GetArticle(int id, int maxExpectedPrice, bool isDealerOne = true)
        {
            Result<ArticleDto> result = new Result<ArticleDto>();
            ArticleDto tmpArticle = null;

            _log.LogInformation("Pulling cache service from dependenvy injection");
            var cacheService = _inventoryServices.FirstOrDefault(x => x.GetType() == typeof(CachedInventoryRepository));

            if (cacheService != null)
            {
                tmpArticle = cacheService.GetArticle(id);
                if (tmpArticle == null || tmpArticle.ArticlePrice > maxExpectedPrice)
                {
                    var warehouseService = _inventoryServices.FirstOrDefault(x => x.GetType() == typeof(WarehouseInventoryRepository));
                    if (warehouseService != null)
                    {
                        tmpArticle = warehouseService.GetArticle(id);
                        if (tmpArticle == null || tmpArticle.ArticlePrice > maxExpectedPrice)
                        {
                            tmpArticle = await _dealerCommunicator.GetArticle(id, HttpCommunicatorEnum.DealerOne);
                            if (tmpArticle == null || tmpArticle.ArticlePrice > maxExpectedPrice)
                            {
                                tmpArticle = await _dealerCommunicator.GetArticle(id, HttpCommunicatorEnum.DealerTwo);
                            }
                        }
                    }
                }
                if (tmpArticle != null)
                {
                    _log.LogInformation("Article found and retrieved.");
                    _cacheStoreService.SetArticle(tmpArticle);
                    return ResponseManager<ArticleDto>.GenerateServiceSuccessResult(ResultStatus.Success, tmpArticle, result);
                }
            }

            return ResponseManager<ArticleDto>.GenerateErrorServiceResult(
                                                ResultStatus.NotFound,
                                                "There is no article with provided id and maximum expected price.",
                                                result);
        }

        public async Task<Result<bool>> BuyArticle(ArticleDto article, int buyerId, bool isDealerOne = true)
        {
            Result<bool> result = new Result<bool>();
            if (article == null)
            {
                return ResponseManager<bool>.GenerateErrorServiceResult(
                                                ResultStatus.InvalidParameters,
                                                "Could not order article.",
                                                result);
            }
            var warehouseService = _inventoryServices.FirstOrDefault(x => x.GetType() == typeof(WarehouseInventoryRepository));
            if (warehouseService != null)
            {
                var tmpArticle = warehouseService.GetArticle(article.ID);
                if (tmpArticle != null)
                {
                    HelperManager.StoreBoughtArticle(_db, article, buyerId);
                    return ResponseManager<bool>.GenerateServiceSuccessResult(ResultStatus.Success, true, result);
                }
                else
                {
                    var bought = await _dealerCommunicator.BuyArticle(article, HttpCommunicatorEnum.DealerOne);
                    if (bought)
                    {
                        HelperManager.StoreBoughtArticle(_db, article, buyerId);
                        return ResponseManager<bool>.GenerateServiceSuccessResult(ResultStatus.Success, true, result);
                    }
                    else
                    {
                        bought = await _dealerCommunicator.BuyArticle(article, HttpCommunicatorEnum.DealerTwo);
                        if (bought)
                        {
                            HelperManager.StoreBoughtArticle(_db, article, buyerId);
                            return ResponseManager<bool>.GenerateServiceSuccessResult(ResultStatus.Success, true, result);
                        }
                    }
                }
            }

            return ResponseManager<bool>.GenerateErrorServiceResult(
                                                ResultStatus.InvalidParameters,
                                                "Failed to buy article.",
                                                result);
        }
    }
}
