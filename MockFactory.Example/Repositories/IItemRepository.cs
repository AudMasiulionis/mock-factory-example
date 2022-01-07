using MockFactory.Example.Entities;

namespace MockFactory.Example.Repositories;

public interface IItemRepository
{
    Task<Item> GetAsync(int itemId);
}