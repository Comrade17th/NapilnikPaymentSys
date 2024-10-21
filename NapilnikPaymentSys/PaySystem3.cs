public class PaySystem3 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem3(IHash hasher)
    {
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
    }

    public string GetPayingLink(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        return $"/system3.com/pay?amount=12000&curency=RUB&hash={_hasher.Hash(order.Amount.ToString())}{order.Id}secretkey";
    }
}