using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using WinAppDriver.Helper.Classes;

namespace WinAppDriver.Helper
{
    public class Helper
    {
        private WindowsDriver<WindowsElement> _driver;

        public Helper(string exePath, string folderPath, string appArguments, bool isLocal = false, string remoteEndpoint = "")
        {
            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", exePath);
            opt.AddAdditionalCapability("appWorkingDir", folderPath);
            opt.AddAdditionalCapability("appArguments", appArguments);

            if (isLocal)
                _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), opt, TimeSpan.FromMinutes(10));
            else
                _driver = new WindowsDriver<WindowsElement>(new Uri(remoteEndpoint), opt, TimeSpan.FromMinutes(10));
        }

        private WindowsElement GetWindowsElement(WinElement element)
        {
            switch (element.by)
            {
                case FindBy.AccessibilityId:
                    return _driver.FindElementByAccessibilityId(element.value);
                case FindBy.ClassName:
                    return _driver.FindElementByClassName(element.value);
                case FindBy.CssSelector:
                    return _driver.FindElementByCssSelector(element.value);
                case FindBy.Id:
                    return _driver.FindElementById(element.value);
                case FindBy.Name:
                    return _driver.FindElementByName(element.value);
                case FindBy.WindowsUIAutomation:
                    return _driver.FindElementByWindowsUIAutomation(element.value);
                case FindBy.XPath:
                    return _driver.FindElementByXPath(element.value);
            }
            return null;
        }

        private ReadOnlyCollection<WindowsElement> GetWindowsElements(WinElement element)
        {
            switch (element.by)
            {
                case FindBy.AccessibilityId:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByAccessibilityId(element.value);
                case FindBy.ClassName:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByClassName(element.value);
                case FindBy.CssSelector:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByCssSelector(element.value);
                case FindBy.Id:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsById(element.value);
                case FindBy.Name:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByName(element.value);
                case FindBy.WindowsUIAutomation:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByWindowsUIAutomation(element.value);
                case FindBy.XPath:
                    return (ReadOnlyCollection<WindowsElement>)_driver.FindElementsByXPath(element.value);
            }
            return null;
        }

        public bool Click(WinElement element)
        {
            WindowsElement winElement = GetWindowsElement(element);
            if (winElement != null)
                winElement.Click();
            return false;
        }

        public bool SetText(WinElement element, string text, bool clearText = false, bool sendTabAfter = false)
        {
            WindowsElement winElement = GetWindowsElement(element);
            if (winElement != null)
            {
                if (clearText)
                    winElement.Clear();
                winElement.SendKeys(text);
                if (sendTabAfter)
                    winElement.SendKeys(Keys.Tab + Keys.Tab);
            }
            return false;
        }

        public string GetText(WinElement element)
        {

            WindowsElement winElement = GetWindowsElement(element);
            if (winElement != null)
            {
                return winElement.Text;
            }
            throw new Exception("Element not found");
        }

        public bool AlterWindow(string windowTitle, int timeoutInSeconds)
        {
            bool found = false;
            DateTime waitUntil = DateTime.Now.AddSeconds(timeoutInSeconds);
            while (!found && DateTime.Now < waitUntil)
            {
                var winHandles = _driver.WindowHandles;
                foreach (string handle in winHandles)
                {
                    _driver.SwitchTo().Window(handle);
                    if (_driver.Title.ToLower().Trim().Equals(windowTitle.ToLower().Trim()))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        public bool SelectSubMenu(string menu, string submenu_1, string submenu_2)
        {
            WindowsElement winMenu = GetWindowsElement(new WinElement("menu", FindBy.XPath, $"//MenuItem[starts-with(@Name,'" + menu + "')]"));
            if (winMenu != null)
            {
                winMenu.Click();

                if (!String.IsNullOrEmpty(submenu_1))
                {
                    ReadOnlyCollection<WindowsElement> subMenuElements = GetWindowsElements(new WinElement("submenu_1", FindBy.XPath, $"//MenuItem[starts-with(@Name,'" + menu + "')]/*"));
                    foreach (WindowsElement subMenuElement in subMenuElements)
                    {
                        if (subMenuElement.Text.ToLower().Trim().Equals(submenu_1.ToLower().Trim()))
                        {
                            new Actions(_driver).MoveToElement(subMenuElement).Click().Build().Perform();
                            Thread.Sleep(500);

                            if (!String.IsNullOrEmpty(submenu_2))
                            {
                                ReadOnlyCollection<WindowsElement> subMenu2Elements = GetWindowsElements(new WinElement("submenu_2", FindBy.XPath, $"//MenuItem[starts-with(@Name,'" + menu + "')]/*/*"));
                                foreach (WindowsElement subMenu2Element in subMenu2Elements)
                                {
                                    if (subMenu2Element.Text.ToLower().Trim().Equals(submenu_2.ToLower().Trim()))
                                    {
                                        new Actions(_driver).MoveToElement(subMenu2Element).Click().Build().Perform();
                                        Thread.Sleep(500);
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public void Close()
        {
            _driver.Close();
            _driver.CloseApp();
        }

    }
}
