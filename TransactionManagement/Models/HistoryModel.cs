using TransactionManagement.Enums;

namespace TransactionManagement.Models
{
    public class HistoryModel
    {
        public HistoryModel(string transactionName, string transactionLegName, TransactionHistoryEnum historyType)
        {
            TransactionName = transactionName;
            TransactionLegName = transactionLegName;
            HistoryType = historyType;
        }

        public string TransactionName { get; set; }

        public string TransactionLegName { get; set; }

        public TransactionHistoryEnum HistoryType { get; set; }

        public override string ToString()
        {
            return string.Format("Transaction: {0}, Leg: {1}, {2}", TransactionName, TransactionLegName, HistoryType);
        }
    }
}