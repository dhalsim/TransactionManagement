using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransactionManagement.Enums;
using TransactionManagement.Interfaces;
using TransactionManagement.Models;

namespace TransactionManagement.Library
{
    public abstract class TransactionLeg : IExecutable, IRollable
    {
        protected TransactionLeg(string name)
        {
            Name = name;
            Exceptions = new TransactionExceptions(this);
        }

        public string Name { get; set; }

        [JsonIgnore]
        public Transaction ParentTransaction { get; set; }

        public TransactionExceptions Exceptions { get; set; }

        public ExecutionResult Result { get; set; }

        public ExecutionResult RollbackResult { get; set; }

        public abstract ExecutionResult Execute();

        public abstract ExecutionResult Rollback();

        public void AddHistory(TransactionHistoryEnum history)
        {
            if (ParentTransaction == null)
            {
                throw new Exception("Parent Transaction can not be null");
            }

            ParentTransaction.History.Add(new HistoryModel(ParentTransaction.Name, Name, history));
        }
    }
}