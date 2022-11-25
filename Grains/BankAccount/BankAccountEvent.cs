using System;
using System.Collections.Generic;
using System.Text;

namespace Grains.BankAccount
{
    public abstract class BankAccountEvent
    {
        public decimal Amount { get; set; }
    }

    public class Deposited : BankAccountEvent { }

    public class Withdrawn : BankAccountEvent { }
}
