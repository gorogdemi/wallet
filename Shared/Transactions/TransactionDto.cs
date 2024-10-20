﻿using Wallet.Shared.Common.Enums;

namespace Wallet.Shared.Transactions;

public class TransactionDto
{
    public double BankAmount { get; set; }

    public double CashAmount { get; set; }

    public long? CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Comment { get; set; }

    public DateTime Date { get; set; }

    public long Id { get; set; }

    public string Name { get; set; }

    public double SumAmount { get; set; }

    public TransactionType Type { get; set; }
}