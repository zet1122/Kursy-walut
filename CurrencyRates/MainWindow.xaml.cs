using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace CurrencyRates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Currency> CurrencyList = new List<Currency>();
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeBindings()
        {
            chooseCurrency.ItemsSource = CurrencyList;
            chooseCurrency.DisplayMemberPath = "Code";
            singleCurrencyView.ItemsSource = CurrencyList;
            nowLabel.Content = "-- Aktualne średnie kursy: " + DateTime.Now.ToShortDateString() + " --";
        }
        
        private void downloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Download.listOfCurrency.Clear();

            try
            {
                Download.DownloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania danych: " + ex.Message, "Currency Rates", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            CurrencyList = Download.listOfCurrency;
            InitializeBindings();
        }

        private Dictionary<string, double> getChartData(List<Currency> lst, string str)
        {
            int index = 0;

            switch (str)
            {
                case "USD":
                    index = 0;
                    break;
                case "EUR":
                    index = 1;
                    break;
                case "GBP":
                    index = 2;
                    break;
                case "CHF":
                    index = 3;
                    break;
                default: break;
            }

            return new Dictionary<string, double>(lst[index].Exchange);
        }

        private void chooseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = chooseCurrency.SelectedItem.ToString();
            Dictionary<string, double> chartData = new Dictionary<string, double>(getChartData(CurrencyList, value));

            allCurrencyView.ItemsSource = chartData;
            lineChart.Title = value;
            showLineChart(chartData);
        }

        private void showLineChart(Dictionary<string, double> dict)
        {
            List<KeyValuePair<string, double>> valueList = new List<KeyValuePair<string, double>>();

            foreach (var item in dict)
            {
                valueList.Add(item);
            }

            valueList.Reverse();
            lineChart.DataContext = valueList;
        }

        private void Save()
        {
            string fileName = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour + DateTime.Now.Minute + ".txt";
            string date = "Utworzono: " + DateTime.Now.ToString();

            File.WriteAllText(fileName, date + Environment.NewLine);

            foreach (var item in CurrencyList)
            {
                var codeName = item.Code;
                var rank = item.Exchange;

                File.AppendAllText(fileName, codeName + Environment.NewLine);
                File.AppendAllLines(fileName, rank.Select(x => x.ToString()));
            }

            MessageBox.Show("Zapis się udał", "Currency Rates", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void InitializeChart()
        {
            List<KeyValuePair<string, double>> MyValue = new List<KeyValuePair<string, double>>();
            MyValue.Add(new KeyValuePair<string, double>("Data", 0.0));
            lineChart.DataContext = MyValue;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
    }
}
