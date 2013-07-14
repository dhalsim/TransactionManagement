﻿namespace TransactionManagement.Enums
{
    public enum TransactionHistoryEnum
    {
        TRANSACTION_EXECUTION_STARTED = 1,
        TRANSACTION_EXECUTION_FINISHED = 2,

        TRANSACTION_ROLLBACK_STARTED = 3,
        TRANSACTION_ROLLBACK_FINISHED = 4,

        TRANSACTION_SUCCEEDED = 5,
        TRANSACTION_FAILED = 6,
        TRANSACTION_BROKEN = 7,
        TRANSACTION_CONTINUED = 8,

        TRANSACTION_ROLLBACK_SUCCEEDED = 9,
        TRANSACTION_ROLLBACK_FAILED = 10,
        TRANSACTION_ROLLBACK_BROKEN = 11,
        TRANSACTION_ROLLBACK_CONTINUED = 12,

        LEG_EXECUTION_STARTED = 13,
        LEG_EXECUTION_FINISHED = 14,

        LEG_ROLLBACK_STARTED = 15,
        LEG_ROLLBACK_FINISHED = 16,
        LEG_ROLLBACK_PASSED = 17,

        LEG_SUCCEEDED = 18,
        LEG_FAILED = 19,
        LEG_BROKEN = 20,
        LEG_CONTINUED = 21,

        LEG_ROLLBACK_SUCCEEDED = 22,
        LEG_ROLLBACK_FAILED = 23,
        LEG_ROLLBACK_BROKEN = 24,
        LEG_ROLLBACK_CONTINUED = 25
    }
}