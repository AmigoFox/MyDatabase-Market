namespace API_DatabaseMarket.Services
{
    public interface IOrderService
    {
        Task<bool> DeleteOrderAsync(int id);
    }
}
