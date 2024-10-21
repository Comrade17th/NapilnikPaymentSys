using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

        IHash md5Hasher = new MD5Hasher();
        IHash sha1Hasher = new SHA1Hasher();

        string currencyRubles = "RUB";
        Order order = new Order(123, 123, currencyRubles);

        List<IPaymentSystem> paymentSystems = new List<IPaymentSystem>()
        {
            new PaySystem1(md5Hasher),
            new PaySystem2(sha1Hasher),
            new PaySystem3(sha1Hasher)
        };

        foreach (IPaymentSystem system in paymentSystems)
        {
            Console.WriteLine(system.GetPayingLink(order));
        }
    }
}

public class PaySystem1 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem1(IHash hasher)
    {
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
    }

    public string GetPayingLink(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
            
        string hash = _hasher.GetHashOf(order.Id.ToString());
        
        return $"pay.system1.ru/order?amount={order.Amount}{order.Currency}&hash={hash}";
    }
}

public class PaySystem2 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem2(IHash hasher) => 
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));

    public string GetPayingLink(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        string hash = _hasher.GetHashOf(order.Id.ToString());
        
        return $"order.system2.ru/pay?hash={hash}{order.Amount}";
    }
}

public class PaySystem3 : IPaymentSystem
{
    private IHash _hasher;

    public PaySystem3(IHash hasher) => 
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));

    public string GetPayingLink(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        string hash = _hasher.GetHashOf(order.Amount.ToString());

        return $"/system3.com/pay?amount={order.Amount}&curency={order.Currency}&hash={hash}{order.Id}secretkey";
    }
}

public class SHA1Hasher : IHash
{
    public string GetHashOf(string input)
    {
        using (MD5 md5 = MD5.Create())
            return Convert.ToHexString(md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
    }
}

public class MD5Hasher : IHash
{
    public string GetHashOf(string input) =>
        Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(input)));
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;
    public readonly string Currency;

    public Order(int id, int amount, string currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        
        Id = id;
        Amount = amount;
        Currency = currency;
    }
}

public interface IHash
{
    string GetHashOf(string input);
}

public interface IPaymentSystem
{
    public string GetPayingLink(Order order);
}