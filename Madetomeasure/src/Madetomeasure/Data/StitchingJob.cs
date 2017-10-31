using System;
using System.Collections.Generic;

namespace Madetomeasure.Data
{
    public partial class StitchingJob
    {
        public StitchingJob()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
            ProductionActivity = new HashSet<ProductionActivity>();
        }

        public int JobId { get; set; }
        public int? JobTypeId { get; set; }
        public int? CustomerId { get; set; }
        public int? ShopCode { get; set; }
        public int WarehouseStatus { get; set; }
        public int? CurrentStatus { get; set; }
        public int FabricfromCustomer { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual BlazerMeasurements BlazerMeasurements { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual KurtaMeasurements KurtaMeasurements { get; set; }
        public virtual PantMeasurements PantMeasurements { get; set; }
        public virtual ICollection<ProductionActivity> ProductionActivity { get; set; }
        public virtual ShalwarMeasurements ShalwarMeasurements { get; set; }
        public virtual ShirtMeasurements ShirtMeasurements { get; set; }
        public virtual SuitMeasurements SuitMeasurements { get; set; }
        public virtual Users Customer { get; set; }
        public virtual JobType JobType { get; set; }
        public virtual BusinessEntity ShopCodeNavigation { get; set; }
    }
}
