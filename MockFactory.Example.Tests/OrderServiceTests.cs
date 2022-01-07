namespace MockFactory.Example.Tests;

public class OrderServiceTests : IDisposable
{
    private readonly OrderService _sut;
    private readonly MockRepository _mockRepository;
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    public OrderServiceTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);
        _itemRepositoryMock = _mockRepository.Create<IItemRepository>();
        _orderRepositoryMock = _mockRepository.Create<IOrderRepository>();
        _sut = new OrderService(_itemRepositoryMock.Object, _orderRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateNewOrder()
    {
        const int itemId = 1;
        const int quantity = 2;
        const int existingItemStock = 3;

        _itemRepositoryMock.Setup(m => m.GetAsync(It.Is<int>(i => i == itemId)))
            .ReturnsAsync(() => new Item
            {
                Id = itemId,
                Stock = existingItemStock
            });

        _orderRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        await _sut.CreateAsync(itemId, quantity);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenItemIsOutOfStock()
    {
        const int itemId = 1;
        const int quantity = 2;
        const int existingItemStock = 1;

        _itemRepositoryMock.Setup(m => m.GetAsync(It.Is<int>(i => i == itemId)))
            .ReturnsAsync(() => new Item
            {
                Id = itemId,
                Stock = existingItemStock
            });

        Task Act() => _sut.CreateAsync(itemId, quantity);
        var exception = await Assert.ThrowsAsync<Exception>(Act);
        Assert.Equal("Item id=[1] has not enough stock for order.", exception.Message);
    }

    public void Dispose()
    {
        _mockRepository.VerifyAll();
    }
}