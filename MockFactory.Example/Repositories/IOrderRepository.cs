namespace MockFactory.Example.Repositories;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
}