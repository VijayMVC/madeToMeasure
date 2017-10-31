using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.Admin
{
    public class LinkShopWarehous
    {
        [Required(ErrorMessage = "Please enter your shop code ")]
        [Display(Name ="warehouseid")]
        public int shopc { get; set; }

        [Required(ErrorMessage = "Please enter your warehouse code ")]
        [Display(Name ="shopid")]
        public int warehousec { get; set; }

        public async Task addShop(MadeToMeasureContext context,int shopcode,int warehousecode)
        {
           Madetomeasure.Data.Shop  shop = new Madetomeasure.Data.Shop();
            shop.ShopCode = shopcode;
            shop.AssociatedWarehouseCode = warehousecode;
           

            context.Shop.Add(shop);
            await context.SaveChangesAsync();
        }
    }
}
