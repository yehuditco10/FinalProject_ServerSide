using System;

namespace Transaction.Data.Exceptions
{
    public class AddToDBFailedExeption:Exception
    {
        public AddToDBFailedExeption()
        {
        }

        public AddToDBFailedExeption(string message)
            : base(message)
        {
        }

        public AddToDBFailedExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
