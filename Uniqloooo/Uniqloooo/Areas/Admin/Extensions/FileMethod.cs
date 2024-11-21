using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Uniqloooo.ViewModels.Sliders;

namespace Uniqloooo.Areas.Admin.Extensions
{
    public static class FileMethod 
    {
     
        public static bool  IsValidType(this string contentType)
        {
            if (contentType.StartsWith("image"))
            {
                return false;
            }
            return true;
        }
        public static bool IsValidSize(this int kb)
        {
            if (kb>1024*1024)
            {
                return false;
            }
            return true;
        }
     

    }
}
