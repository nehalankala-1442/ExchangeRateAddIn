using ExcelDna.Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ExchangeRate
{
    public class ExchangeRate
    {
        [ExcelFunction(Description = "Exchange Rate", Name = "Exchange")]
        public static double Exchange(string currency, DateTime dateTime)
        {         

            double amount = 0;
            if (ValidateCurrency(currency))
            {
                //For comparision with excel default date
                DateTime date = new DateTime(1899, 12, 30);
                int res = dateTime.CompareTo(date);
                if (res == 0)
                {
                    dateTime = DateTime.Now;
                }
                try
                {
                    HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.ratesapi.io/api/" + dateTime.ToString("yyyy-MM-dd") + "?base=EUR&symbols=" + currency));
                    WebReq.Method = "GET";
                    HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                    string jsonString;
                    using (Stream stream = WebResp.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    ExchangeRateDTO exchangeRateDTO = JsonConvert.DeserializeObject<ExchangeRateDTO>(jsonString);
                    if (exchangeRateDTO.Rates.Any())
                        amount = exchangeRateDTO.Rates.First().Value;
                }
                catch (Exception ex)
                {
                    //persist the exception
                    throw ex;
                }
            }
            return amount;            
        }

        public static bool ValidateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency) || currency.Length < 3)
            {
                ExcelDna.Logging.LogDisplay.WriteLine("currency should be min 3 char length");
                throw new Exception("currency error");
            }
            else
                return true;
        }
    }
}
