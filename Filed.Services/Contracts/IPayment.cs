using System.Collections.Generic;

namespace Filed.Services.Contracts
{
    public interface IPayment<T> where T : class
    {
        public IEnumerable<T> Get();
        public int ProcessPayment(T t);
    }
}
