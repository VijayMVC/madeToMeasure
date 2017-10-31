using Madetomeasure.Models.Shop;
using Madetomeasure.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Shop
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            InvoiceDetails = new HashSet<InvoiceDetailViewModel>();
        }

        public int InvoiceId { get; set; }
        public int ShopCode { get; set; }
        public int SalesPersonId { get; set; }
        public string salesperson_name { get; set; }
        public string shop_address { get; set; }
        public int balance { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public int AdvanceReceived { get; set; }
        public int ReceiptId { get; set; }

        public virtual ICollection<InvoiceDetailViewModel> InvoiceDetails { get; set; }
    }
}
