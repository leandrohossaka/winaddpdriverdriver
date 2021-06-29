using Newtonsoft.Json;
using System;
using System.IO;
using WinAppDriver.Helper;
using WinAppDriver.Helper.Classes;

namespace Calculadora
{
    class Program
    {
        static  JsonLocator locators;
        static void Main(string[] args)
        {
            log("Iniciando execução");
            log("Carregando Locators");
            
            string locatorsPath = @"..\..\..\Locators\LocatorsCalculadora.json";

            locators = JsonConvert.DeserializeObject<JsonLocator>(File.ReadAllText(locatorsPath));


            log("Abrindo Calculadora");
            Helper helper = new Helper("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App", @"c:\windows\System32\", "", true);

            log("Calculadora Aberta!");

            log("-----------------------------------------------------");
            while (true)
            {
                log("");
                log("Entre com o calculo desejado:");
                string calculo = Console.ReadLine();

                if (calculo.ToLower() == "sair")
                    break;

                foreach (Char c in calculo)
                {
                    helper.Click(getLocators("Calculadora." + c));
                }
                log("");
                helper.Click(getLocators("Calculadora.="));
                log("O resultado para o calculo " + calculo + " é: " + helper.GetText(getLocators("Calculadora.resultado")).Replace("A exibição é ", ""));
                log("");
                log("----------------------------------------------------");
                log("");
            }
            log("");
            log("Fim do Processo");
        }

        static void log(string log)
        {
            Console.WriteLine(log);
        }

        static WinElement getLocators(string path)
        {
            string[] arrayPath = path.Split('.');

            Page p = locators.locators.Find(x => x.page == arrayPath[0]);

            if(p != null)
            {
                return p.elementos.Find(x => x.name == arrayPath[1]);
            }
            return null; 
        }
    }
}
