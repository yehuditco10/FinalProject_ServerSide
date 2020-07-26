using System.Threading.Tasks;

namespace Transaction.Services
{
   public interface ITransactionService
    {
       Task<bool> DoTransactionAsync(Models.Transaction transaction); 
    }
}
