public class PaySystem2 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem2(IHash hasher)
    {
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
    }

    public string GetPayingLink(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        return $"order.system2.ru/pay?hash={_hasher.Hash(order.Id.ToString())}{order.Amount}";
    }
}