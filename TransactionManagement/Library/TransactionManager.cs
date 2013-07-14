using System;
using System.Collections.Generic;
using TransactionManagement.Enums;
using TransactionManagement.Interfaces;

namespace TransactionManagement.Library
{
    internal class TransactionManager
    {
        private readonly Transaction _transaction;
        private ExecutionResult _executionResult;
        private ExecutionResult _rollbackResult;

        private ExecutionResult ExecutionResult
        {
            get { return _executionResult; }
            set
            {
                switch (_executionResult)
                {
                    case ExecutionResult.CONTINUED:
                        if (value == ExecutionResult.FAILED || value == ExecutionResult.BROKEN)
                        {
                            _executionResult = value;
                        }
                        break;
                    case ExecutionResult.FAILED:
                        break;
                    default:
                        _executionResult = value;
                        break;
                }

                _transaction.Result = _executionResult;
            }
        }

        private ExecutionResult RollbackResult
        {
            get { return _rollbackResult; }
            set
            {
                switch (_rollbackResult)
                {
                    case ExecutionResult.CONTINUED:
                        if (value == ExecutionResult.FAILED)
                        {
                            _rollbackResult = value;
                        }
                        break;
                    case ExecutionResult.FAILED:
                    case ExecutionResult.BROKEN:
                        break;
                    default:
                        _rollbackResult = value;
                        break;
                }

                _transaction.RollbackResult = _rollbackResult;
            }
        }

        internal TransactionManager(Transaction transaction)
        {
            _transaction = transaction;
        }

        internal void ExecuteLegs(List<TransactionLeg> legs)
        {
            if (legs.Count == 0)
            {
                throw new Exception("Can not execute transaction with no transaction legs");
            }

            foreach (TransactionLeg transactionLeg in legs)
            {
                try
                {
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_EXECUTION_STARTED);
                    ExecutionResult executedResult = transactionLeg.Result = transactionLeg.Execute();
                    ExecutionResult = executedResult;
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_EXECUTION_FINISHED);

                    // add to leg's history
                    switch (executedResult)
                    {
                        case ExecutionResult.SUCCEEDED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_SUCCEEDED);
                            break;
                        case ExecutionResult.FAILED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_FAILED);
                            ExecutionResult = ExecutionResult.FAILED;
                            RollbackLegs(legs);
                            return;
                        case ExecutionResult.CONTINUED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_CONTINUED);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    transactionLeg.Exceptions.AddException(exception, ExceptionType.ON_EXECUTE);
                    if (transactionLeg is IContinuable)
                    {
                        ExecutionResult = ExecutionResult.CONTINUED;
                        transactionLeg.Result = ExecutionResult.CONTINUED;
                        transactionLeg.AddHistory(TransactionHistoryEnum.LEG_CONTINUED);
                        continue;
                    }

                    //rollback all
                    ExecutionResult = ExecutionResult.BROKEN;
                    transactionLeg.Result = ExecutionResult.BROKEN;
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_BROKEN);

                    _transaction.AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_STARTED);
                    RollbackLegs(legs);
                    _transaction.AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_FINISHED);
                    return;
                }
            }
        }

        internal void RollbackLegs(List<TransactionLeg> legs)
        {
            if (legs.Count == 0)
            {
                throw new Exception("Can not rollback transaction with no transaction legs");
            }

            for (int index = legs.Count - 1; index >= 0; index--)
            {
                TransactionLeg transactionLeg = legs[index];
                if (transactionLeg.Result == ExecutionResult.NOTRUN)
                {
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_STARTED);
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_PASSED);
                    continue;
                }
                if (transactionLeg.Result == ExecutionResult.CONTINUED)
                {
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_STARTED);
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_CONTINUED);

                    RollbackResult = ExecutionResult.CONTINUED;
                    continue;
                }

                try
                {
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_STARTED);
                    ExecutionResult executedResult = transactionLeg.RollbackResult = transactionLeg.Rollback();
                    RollbackResult = executedResult;
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_FINISHED);

                    // add to leg's history
                    switch (executedResult)
                    {
                        case ExecutionResult.SUCCEEDED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_SUCCEEDED);
                            break;
                        case ExecutionResult.CONTINUED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_CONTINUED);
                            break;
                        case ExecutionResult.FAILED:
                            transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_FAILED);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    transactionLeg.AddHistory(TransactionHistoryEnum.LEG_ROLLBACK_BROKEN);
                    transactionLeg.Exceptions.AddException(exception, ExceptionType.ON_ROLLBACK);
                    if (transactionLeg is IContinuable)
                    {
                        RollbackResult = transactionLeg.RollbackResult = ExecutionResult.CONTINUED;
                    }
                    else
                    {
                        RollbackResult = transactionLeg.RollbackResult = ExecutionResult.BROKEN;
                    }
                }
            }
        }
    }
}