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

        Order order = new Order(123, 123);

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