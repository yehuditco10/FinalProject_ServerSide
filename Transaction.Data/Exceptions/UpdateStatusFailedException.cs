using System;
using System.Collections.Generic;
using System.Text;

namespace Transaction.Data.Exceptions
{
  public  class UpdateStatusFailedException:Exception
    {
        public UpdateStatusFailedException()
        {
        }

        public UpdateStatusFailedException(string message)
            : base(message)
        {
        }

        public UpdateStatusFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
