public class PaySystem1 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem1(IHash hasher)
    {
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
    }

    public string GetPayingLink(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        return $"pay.system1.ru/order?amount=12000RUB&hash={_hasher.Hash(order.Id.ToString())}";
    }
}