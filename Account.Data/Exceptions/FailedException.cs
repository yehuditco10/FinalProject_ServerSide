using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Exceptions
{
   public class FailedException:Exception
    {
        public FailedException()
        {
        }

        public FailedException(string message)
            : base(message)
        {
        }

        public FailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
