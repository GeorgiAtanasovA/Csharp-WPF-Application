using System;
using System.Diagnostics;
using System.Windows;

namespace ZaplataCompute
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class SalaryCompute : Window
   {
      public SalaryCompute()
      {
         InitializeComponent();
         dateTime.Content = DateTime.Now;
         labelInfo.Content = labelInfo.Content + "\nHi, write your hourly wage, your hours and your tax.";
      }

      private void button_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            double hourlyWage = double.Parse(hourlyWageTB.Text);
            double hours = double.Parse(timeTB.Text);
            double skatt = double.Parse(taxTB.Text);

            double sum = hourlyWage / 100;
            sum = sum * skatt;
            sum = hourlyWage - sum;
            sum = sum * hours;

            labelInfo.Content = "";
            labelInfo.Content = "Info:";
            labelInfo.Content = labelInfo.Content + "\nHi, write your hourly wage, your hours and your tax.";

            resultL.HorizontalContentAlignment = HorizontalAlignment.Left;
            resultL.Content = "      Amount to receive:";
            resultLDigit.Content = sum;
         }
         catch (Exception)
         {
            labelInfo.Content = "Enter numbers in all fields!";
         }
      }

      private void calcB_Click(object sender, RoutedEventArgs e)
      {
         Process.Start("Calc");
      }

      private void button_Click_1(object sender, RoutedEventArgs e)
      {
         hourlyWageTB.Text = "";
         timeTB.Text = "";
         taxTB.Text = "";
         resultL.HorizontalContentAlignment = HorizontalAlignment.Center;
         resultL.Content = "Result:";
         resultLDigit.Content = "";
      }
   }
}
