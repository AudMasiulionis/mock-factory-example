namespace MockFactory.Example.Services;

public class OrderService
{
    private readonly IItemRepository _itemRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderService(
        IItemRepository itemRepository,
        IOrderRepository orderRepository)
    {
        _itemRepository = itemRepository;
        _orderRepository = orderRepository;
    }

    public async Task CreateAsync(int itemId, int itemQuantity)
    {
        var item = await _itemRepository.GetAsync(itemId);
        if (item.Stock < itemQuantity)
            throw new Exception($"Item id=[{itemId}] has not enough stock for order.");

        Order order = new()
        {
            ItemId = item.Id,
            Quantity = itemQuantity
        };

        await _orderRepository.CreateAsync(order);
    }
}