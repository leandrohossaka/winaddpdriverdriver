
namespace WinAppDriver.Helper.Classes
{
    public class WinElement
    {
        public string name;
        public FindBy by;
        public string value;

        public WinElement()
        {

        }
        public WinElement(string elementName, FindBy findBy, string findValue)
        {
            this.name = elementName;
            this.by = findBy;
            this.value = findValue;
        }
    }
}
