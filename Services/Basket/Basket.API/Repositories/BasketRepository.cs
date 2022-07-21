using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }

    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCashe;
        public BasketRepository(IDistributedCache redisCashe)
        {
            _redisCashe = redisCashe ?? throw new ArgumentNullException(nameof(redisCashe));
        }

        public Task DeleteBasket(string userName)
        {
            return _redisCashe.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var json=await _redisCashe.GetStringAsync(userName);
            if(string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(json);
        }

        public Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var json=JsonConvert.SerializeObject(basket);
            _redisCashe.SetStringAsync(basket.UserName, json);  
            return GetBasket(basket.UserName);
        }
    }
}
