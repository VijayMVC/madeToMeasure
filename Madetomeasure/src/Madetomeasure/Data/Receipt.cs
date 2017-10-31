using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Receipt
    {
        public int ReceiptId { get; set; }
        public int InvoiceId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
