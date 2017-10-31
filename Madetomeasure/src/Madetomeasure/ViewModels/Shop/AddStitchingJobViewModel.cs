using Madetomeasure.Models.Shop;
using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.ViewModels.Shop
{
    public class AddStitchingJobViewModel
    {

        public AddStitchingJobViewModel()
        {
            StitchingJobDetails = new HashSet<StitchingJobDetails>();
        }

        public int? JobId { get; set; }
        public int? JobTypeId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public int? ShopCode { get; set; }
        public int? WarehouseStatus { get; set; }
        public int? CurrentStatus { get; set; }

        public int FabricfromCustomer { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpectedDate { get; set; }



        public int? ShirtItemCode1 { get; set; }
        public int? ShirtQuantity1 { get; set; }

        public int? ShirtItemCode2 { get; set; }
        public int? ShirtQuantity2 { get; set; }

        public int? SuitItemCode1 { get; set; }
        public int? SuitQuantity1 { get; set; }
        public int? SuitItemCode2 { get; set; }
        public int? SuitQuantity2 { get; set; }
        public int? SuitItemCode3 { get; set; }
        public int? SuitQuantity3 { get; set; }

        public int? PantItemCode1 { get; set; }
        public int? PantQuantity1 { get; set; }
        public int? PantItemCode2 { get; set; }
        public int? PantQuantity2 { get; set; }

        public int? ShalwarItemCode1 { get; set; }
        public int? ShalwarQuantity1 { get; set; }

        public int? KurtaItemCode1 { get; set; }
        public int? KurtaQuantity1 { get; set; }

        public int? KurtaItemCode2 { get; set; }
        public int? KurtaQuantity2 { get; set; }

        public int? ShalwaarKameezItemCode1 { get; set; }
        public int? ShalwarKameezQuantity1 { get; set; }

        public int? ShalwarKameezItemCode2 { get; set; }
        public int? ShalwarKameezQuantity2 { get; set; }

        public int? BlazerItemCode1 { get; set; }
        public int? BlazerQuantity1 { get; set; }

        public int? BlazerItemCode2 { get; set; }
        public int? BlazerQuantity2 { get; set; }



        public virtual ICollection<StitchingJobDetails> StitchingJobDetails { get; set; }

        public virtual BlazerMeasurements BlazerMeasurements { get; set; }

        public virtual KurtaMeasurements KurtaMeasurements { get; set; }

        public virtual PantMeasurements PantMeasurements { get; set; }
        public virtual ShalwarMeasurements ShalwarMeasurements { get; set; }
        public virtual ShirtMeasurements ShirtMeasurements { get; set; }
        public virtual SuitMeasurements SuitMeasurements { get; set; }


    }
}
