using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Exceptions
{
   public class IrreparableException:Exception
    {
        public IrreparableException()
        {
        }

        public IrreparableException(string message)
            : base(message)
        {
        }

        public IrreparableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
