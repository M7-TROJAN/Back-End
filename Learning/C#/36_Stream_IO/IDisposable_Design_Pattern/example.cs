using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace CAIDisposapleDesignPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 1) not recommended because 
            //var currencyService = new currencyService();
            //var currencies = currencyService.GetCurrencies();
            //currencyService.Dispose();
            //Console.WriteLine(currencies);

            // 2) recommended
            //currencyService currencyService = null;
            //try
            //{
            //    currencyService = new currencyService();
            //    var currencies = currencyService.GetCurrencies();
            //    Console.WriteLine(currencies);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    currencyService?.Dispose();
            //}

            // 3) more recommended (using statement) Best Practice  interdused in .net framework 2.0+
            using (var currencyService = new currencyService())
            {
                var currencies = currencyService.GetCurrencies();
                Console.WriteLine(currencies);
            } // Dispose() will be called automatically

            // 4) using statement without block body (C# 8.0)
            //using var currencyService = new currencyService();
            //var currencies = currencyService.GetCurrencies();
            //Console.WriteLine(currencies);


            // 5) using statement with multiple objects 
            //using (var currencyService = new currencyService())
            //using (var currencyService2 = new currencyService())
            //{
            //    var currencies = currencyService.GetCurrencies();
            //    var currencies2 = currencyService2.GetCurrencies();
            //    Console.WriteLine(currencies);
            //    Console.WriteLine(currencies2);
            //} // Dispose() will be called automatically


            Console.ReadKey();
        }
    }

    class currencyService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed = false;
        public currencyService()
        {
            _httpClient = new HttpClient();
        }

        ~currencyService()
        {
            Dispose(false);
        }
        public string GetCurrencies()
        {
            const string url = "https://www.coinbase.com/api/v2/currencies";
            var response = _httpClient.GetStringAsync(url).Result;
            return response;
        }

        // if disposing is true  => (clean up managed resources and unmanaged resources)
        // if disposing is false => (clean up unmanaged resources and large fields)
        protected virtual void Dispose(bool disposing)
        {
            

            if (_disposed)
            {
                return;
            }

            // Dispose Logic
            if (disposing)
            {
                // dispose managed resources
                _httpClient.Dispose();
            }

            // dispose unmanaged resources
            // sets large fields to null
            _disposed = true;

        }
        public void Dispose()
        {
            Dispose(true);

            // tell the GC not to finalize the object (because we already cleaned up the resources)
            GC.SuppressFinalize(this);


        }
    }
}