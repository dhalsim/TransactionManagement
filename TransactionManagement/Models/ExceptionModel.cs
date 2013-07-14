using System;
using TransactionManagement.Enums;
namespace TransactionManagement.Models
{
    public class ExceptionModel
    {
        public string LegName { get; set; }

        public Type Type { get; set; }

        public Exception Exception { get; set; }

        public ExceptionType ExceptionType { get; set; }

        public ExceptionModel(string legName, Type type, Exception exception, ExceptionType exceptionType)
        {
            LegName = legName;
            Type = type;
            Exception = exception;
            ExceptionType = exceptionType;
        }

        public override string ToString()
        {
            return string.Format("{0} - Leg: {1}, Type: {2}, ExceptionMsg: {3}", 
                ExceptionType, LegName, Type.Name, Exception.Message);
        }
    }
}