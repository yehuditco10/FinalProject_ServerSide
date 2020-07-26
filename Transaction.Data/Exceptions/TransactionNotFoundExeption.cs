
using System;

namespace Transaction.Data.Exceptions
{
   public class TransactionNotFoundExeption:Exception
    {
        public TransactionNotFoundExeption()
        {
        }

        public TransactionNotFoundExeption(string message)
            : base(message)
        {
        }

        public TransactionNotFoundExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
