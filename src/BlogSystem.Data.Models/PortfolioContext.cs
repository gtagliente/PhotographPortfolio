using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Data.Models
{
    class PortfolioContext
    {

        public void AddImage()
        {

            BlogSystemEntities db = new BlogSystemEntities();


            {
                PortfolioImage img = new PortfolioImage
                {
                    filename = "newfile.jpg"
                    ,
                    creation_date = new DateTime()
                    ,
                    category = "a_category"
                };
                db.PortfolioImages.AddObject(img);
                db.SaveChanges();
                Console.WriteLine("All All student in the database:");

            }
        }
    }
}
