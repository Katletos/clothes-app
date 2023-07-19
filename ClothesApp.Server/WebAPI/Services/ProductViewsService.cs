namespace WebAPI.Services;

public class ProductViewsService
{
    private readonly Dictionary<long, HashSet<long>> _viewing;

    public ProductViewsService()
    {
        _viewing = new Dictionary<long, HashSet<long>>();
    }

    private bool Contains(long productId)
    {
        return _viewing.ContainsKey(productId);
    }

    public long GetCount(long productId)
    {
        return _viewing[productId].Count;
    }

    public void IncrementView(long productId, long userId)
    {
        var contains = Contains(productId);

        if (contains)
        {
            _viewing[productId].Add(userId);
        }
        else
        {
            _viewing.Add(productId, new HashSet<long>() { userId });
        }
    }

    public void DecrementView(long productId, long userId)
    {
        var contains = Contains(productId);

        if (!contains)
        {
            return;
        }

        _viewing[productId].Remove(userId);

        if (_viewing[productId].Count == 0)
        {
            _viewing.Remove(productId);
        }
    }
}