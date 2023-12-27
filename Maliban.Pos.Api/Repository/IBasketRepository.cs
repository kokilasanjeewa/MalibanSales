using System.Threading.Tasks;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IBasketRepository
  {
    Task<CustomerBasket> GetBasketAsync(string basketId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);
  }
}