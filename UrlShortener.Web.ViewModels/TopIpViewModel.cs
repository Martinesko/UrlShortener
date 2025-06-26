using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Web.ViewModels
{
    public class TopIpViewModel
    {
        public string Ip { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
