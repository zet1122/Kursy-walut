using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CurrencyRates
{
    static class Download
    {
        public static List<Currency> listOfCurrency = new List<Currency>();

        public static void DownloadData()
        {
            ParseData("http://api.nbp.pl/api/exchangerates/rates/a/usd/last/15/?format=xml");
            ParseData("http://api.nbp.pl/api/exchangerates/rates/a/eur/last/15/?format=xml");
            ParseData("http://api.nbp.pl/api/exchangerates/rates/a/gbp/last/15/?format=xml");
            ParseData("http://api.nbp.pl/api/exchangerates/rates/a/chf/last/15/?format=xml");
        }

        public static void ParseData(string url)
        {
            XmlDocument doc = new XmlDocument();

            try
            {   
                if (string.IsNullOrEmpty(url) == false)
                doc.Load(url);
            }
            catch (Exception)
            {                
                throw;
            }
            
            var dict = new Dictionary<string, double>();

            int count = doc.GetElementsByTagName("Rate").Count;
            string name = doc.GetElementsByTagName("Currency").Item(0).InnerText;
            string code = doc.GetElementsByTagName("Code").Item(0).InnerText;
            string lastCostString = doc.GetElementsByTagName("Mid").Item(count - 1).InnerText;
            double lastCost = double.Parse(lastCostString, System.Globalization.CultureInfo.InvariantCulture);

            for (int i = count - 1; i > 0; i--)
            {
                string stringDate = doc.GetElementsByTagName("EffectiveDate").Item(i).InnerText;
                //DateTime date = DateTime.ParseExact(stringDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string exchange = doc.GetElementsByTagName("Mid").Item(i).InnerText;
                double cost = double.Parse(exchange, System.Globalization.CultureInfo.InvariantCulture);
                dict.Add(stringDate, cost);
            }

            Currency currency = new Currency(name, code, dict, lastCost);
            listOfCurrency.Add(currency);

        }
    }
}
