using BlogSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Data.DbFirst
{
    public class PortfolioContext
    {
        public PortfolioEntities db;
        public PortfolioContext()
        {
            db = new PortfolioEntities();
        }
        public void AddImage(PicPostModel pic)
        {
            PortfolioImage img = new PortfolioImage
            {
                filename = pic.files.First().FileName
                ,
                creation_date = DateTime.Now
                ,
                category = pic.Category
                ,
                id = pic.Id
            };
            //var tran = db.Database.BeginTransaction();
            try { 
           
                db.PortfolioImages.Add(img);
                db.SaveChanges();
                //tran.Commit();
            Console.WriteLine("All All student in the database:");
            }
            catch(Exception ex)
            {
                // tran.Rollback();
                throw ex;
            }
        }

        public void UpdateOrdinals(List<PicPostModel> picList)
        {
            try
            {
                foreach(var pic in picList)
                {
                    var dbPic = db.PortfolioImages.Where(p => p.id == pic.Id).FirstOrDefault();
                    if(dbPic!=null)
                         dbPic.ordinal= pic.Ordinal;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // tran.Rollback();
                throw ex;
            }
        }
    }
}
