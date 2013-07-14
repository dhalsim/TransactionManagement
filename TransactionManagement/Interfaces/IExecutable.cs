using TransactionManagement.Enums;

namespace TransactionManagement.Interfaces
{
    public interface IExecutable
    {
        ExecutionResult Execute();
    }
}