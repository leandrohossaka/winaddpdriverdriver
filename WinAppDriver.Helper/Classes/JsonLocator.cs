using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppDriver.Helper.Classes
{
    public class JsonLocator
    {
        public string version { get; set; }
        public string application { get; set; }
        public List<Page> locators { get; set; }
    }
}
