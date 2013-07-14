using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransactionManagement.Enums;
using TransactionManagement.Interfaces;
using TransactionManagement.Models;

namespace TransactionManagement.Library
{
    public class Transaction : IExecutable, IRollable
    {
        private readonly Lazy<List<TransactionLeg>> _transactionLegs =
            new Lazy<List<TransactionLeg>>(() => new List<TransactionLeg>());

        private readonly TransactionManager _transactionManager;

        public Transaction(string name)
        {
            _transactionManager = new TransactionManager(this);

            Name = name;
            History = new List<HistoryModel>();
        }

        public string Name { get; set; }

        public List<TransactionLeg> TransactionLegs { get { return _transactionLegs.Value; } }

        public ExecutionResult Result { get; set; }

        public ExecutionResult RollbackResult { get; set; }

        public List<HistoryModel> History { get; set; }

        public ExecutionResult Execute()
        {
            AddHistory(TransactionHistoryEnum.TRANSACTION_EXECUTION_STARTED);
            _transactionManager.ExecuteLegs(_transactionLegs.Value);
            AddHistory(TransactionHistoryEnum.TRANSACTION_EXECUTION_FINISHED);

            switch (Result)
            {
                case ExecutionResult.BROKEN:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_BROKEN);
                    break;
                case ExecutionResult.FAILED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_FAILED);
                    break;
                case ExecutionResult.SUCCEEDED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_SUCCEEDED);
                    break;
                case ExecutionResult.CONTINUED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_CONTINUED);
                    break;
                case ExecutionResult.NOTRUN:
                    throw new Exception("Transaction should run no matter whatever");
            }

            return Result;
        }

        public ExecutionResult Rollback()
        {
            AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_STARTED);
            _transactionManager.RollbackLegs(_transactionLegs.Value);
            AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_FINISHED);

            switch (RollbackResult)
            {
                case ExecutionResult.BROKEN:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_BROKEN);
                    break;
                case ExecutionResult.FAILED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_FAILED);
                    break;
                case ExecutionResult.SUCCEEDED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_SUCCEEDED);
                    break;
                case ExecutionResult.CONTINUED:
                    AddHistory(TransactionHistoryEnum.TRANSACTION_ROLLBACK_CONTINUED);
                    break;
                case ExecutionResult.NOTRUN:
                    throw new Exception("Transaction should run no matter whatever");
            }

            return RollbackResult;
        }

        public void Add(TransactionLeg transactionLeg)
        {
            _transactionLegs.Value.Add(transactionLeg);
            transactionLeg.ParentTransaction = this;
        }

        public void AddHistory(TransactionHistoryEnum history)
        {
            History.Add(new HistoryModel(Name, null, history));
        }

        public TransactionExceptions GetExceptions()
        {
            TransactionExceptions allExceptions = new TransactionExceptions(this);
            foreach (TransactionLeg transactionLeg in _transactionLegs.Value)
            {
                allExceptions.AddRange(transactionLeg.Exceptions);
            }

            return allExceptions;
        } 

        public string ToJson()
        {
            return TransactionHelper.ToJson(this);
        }
    }
}