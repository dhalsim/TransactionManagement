using TransactionManagement.Enums;

namespace TransactionManagement.Interfaces
{
    public interface IRollable
    {
        ExecutionResult Rollback();
    }
}