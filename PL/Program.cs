using System;
using BLL;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
            Menu menu = new Menu();
            menu.MainMenu();
        }
    }
}