using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRates
{
    class Currency
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public double LastValue { get; private set; }
        public Dictionary<string, double> Exchange { get; private set; }


        public Currency(string name, string code, Dictionary<string, double> exchange, double lastValue)
        {
            Name = name;
            Code = code;
            Exchange = exchange;
            LastValue = lastValue;
        }

        public override string ToString()
        {
            return Code.ToString();
        }
    }
}
