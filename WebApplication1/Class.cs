using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string Imya {get;set;}
        public string Familiya {get;set;}
        public string Otchestvo {get;set;}
    }
}
