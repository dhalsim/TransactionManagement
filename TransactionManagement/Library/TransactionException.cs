using System;
using System.Collections.Generic;
using TransactionManagement.Enums;
using TransactionManagement.Models;

namespace TransactionManagement.Library
{
    public class TransactionExceptions : List<ExceptionModel>
    {
        public TransactionLeg ParentLeg { get; set; }

        public Transaction ParentTransaction { get; set; }

        public TransactionExceptions(TransactionLeg leg)
        {
            ParentLeg = leg;
        }

        public TransactionExceptions(Transaction transaction)
        {
            ParentTransaction = transaction;
        }

        public void AddException(Exception exception, ExceptionType exceptionType)
        {
            if (ParentLeg == null)
            {
                throw new Exception("Can not add exception from transaction");
            }

            Add(new ExceptionModel(ParentLeg.Name, ParentLeg.GetType(), exception, exceptionType));
        }
    }
}