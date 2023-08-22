using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDataFinder.ConsoleApp
{
    internal class KApplication
    {
        private readonly ILoginService _loginService;
        private readonly ITableDataObtainer _tableDataObtainer;
        private readonly IWebDriver _webDriver;

        public KApplication(ILoginService loginService, ITableDataObtainer tableDataObtainer, IWebDriver webDriver)
        {
            _loginService = loginService;
            _tableDataObtainer = tableDataObtainer;
            _webDriver = webDriver;
        }

        public void Run()
        {
            IOperationResult loginResult = _loginService.Login();
            if (!loginResult.IsSucceeded)
                throw new InvalidDataException(loginResult.AdditionalData?.ToString());
            _tableDataObtainer.Obtain(x =>
            {
                Console.Write(x.rowNumber + "-" + x.Columns.Length + "- \t");
                foreach (var item in x.Columns)
                    Console.Write(item.ToString() + "- \t");
            });

            _webDriver.Quit();
        }
    }
}
