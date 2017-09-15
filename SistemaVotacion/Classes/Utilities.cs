using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace SistemaVotacion.Classes
{
    public  class Utilities
    {

        public static void UploadPhoto(HttpPostedFileBase file)
        {
            //subir foto
            string path = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/Photos/"), pic);
                file.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }
        }
    }
}