using Madetomeasure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Madetomeasure.Models.WarehouseModels
{
    public class CategoryManagement
    {
        MadeToMeasureContext db;

        public CategoryManagement(MadeToMeasureContext dbContext)
        {
            db = dbContext;
        }


        public bool checkCategoryPresence(string category)
        {

            int c = 0;

            var cat = from cate in db.Category
                      where cate.CategoryName == category
                      select cate;

            foreach (var w in cat)
            {

                c++;
            }

            if (c > 0)
                return true;

            return false;



        }


        public async Task addCategory(string category)
        {
            Category cat = new Category();
            cat.CategoryName = category;

             db.Category.Add(cat);
             await db.SaveChangesAsync();



        }

        

   public bool checkSubCategoryPresence(string category)
        {

            int c = 0;

            var cat = from cate in db.SubCategory
                      where cate.SubCategoryName == category
                      select cate;

            foreach (var w in cat)
            {

                c++;
            }

            if (c > 0)
                return true;

            return false;



        }



        public async Task addSubCategory(int category, string subcategory)
        {
            SubCategory sub = new SubCategory();
            sub.CategoryId = category;
            sub.SubCategoryName = subcategory;

            db.SubCategory.Add(sub);
            await db.SaveChangesAsync();



        }

        public List<Category> retrieveCategories()
        {
            List <Category> catList= new List<Category>();

            var categories = from cate in db.Category
                             select cate;


            foreach (var x in categories)
            {
                catList.Add(new Category {CategoryId = x.CategoryId, CategoryName =x.CategoryName });


            }

            return catList;


        }

        public List<SubCategory> retrieveSubCategories()
        {
            List<SubCategory> subcatList = new List<SubCategory>();

            var subcategories = from cate in db.SubCategory
                             select cate;


            foreach (var x in subcategories)
            {
                subcatList.Add(new SubCategory { SubCategoryId = x.SubCategoryId,  CategoryId = x.CategoryId, SubCategoryName = x.SubCategoryName });


            }

            return subcatList;


        }

        public List<SubCategoryDetail> retrieveSubCategories(int catId)
        {
            List<SubCategoryDetail> sub = new List<SubCategoryDetail>();

            var subcategories = from cate in db.SubCategory
                                where cate.CategoryId == catId
                                select cate;


            foreach (var x in subcategories)
            {
                sub.Add(new SubCategoryDetail {SubcategoryId  = x.SubCategoryId, SubcategoryName = x.SubCategoryName });

            }

            return sub;


        }

        
    }
}
