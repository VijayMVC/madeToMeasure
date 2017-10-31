using Madetomeasure.ViewModels.Shop;
using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
            Receipt = new HashSet<Receipt>();
        }

        public int InvoiceId { get; set; }
        public int ShopCode { get; set; }
        public int SalesPersonId { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public int AdvanceReceived { get; set; }

        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual ICollection<Receipt> Receipt { get; set; }
        public virtual Users SalesPerson { get; set; }
        public virtual BusinessEntity ShopCodeNavigation { get; set; }
    }
}
