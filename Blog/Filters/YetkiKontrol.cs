using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Filters
{
    public class Yetki
    {
        private static Blog.DAL.BlogEntities db = new DAL.BlogEntities();

        public static bool Kontrol(int id)
        {
            var UyeID = db.Uye.Find(id);

            if (UyeID.Yetki == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}