using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StartScreen
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }
      private void button_Click(object sender, RoutedEventArgs e)
      {
         SoundPlayer soundStart = new SoundPlayer();
         soundStart.SoundLocation = @"..\..\CarCloseDoor.wav";
         soundStart.Play();
         Thread.Sleep(3500);
         soundStart.SoundLocation = @"..\..\CarEngineIgnition.wav";
         soundStart.Play();
         Thread.Sleep(3500);

         try
         {
            Process startGame = new Process();
            startGame.StartInfo.FileName = @"..\..\..\2D_Graphics_Car\bin\Debug\2D_Graphics_Car.exe";

            startGame.Start();
            startGame.Close();
         }
         catch (System.Exception)
         {
            MessageBox.Show("Sorry!!! \nRebuild the solution because the game .exe file is too big to upload to GitHub!", "Exit Game", MessageBoxButton.OK, MessageBoxImage.Error);
            Thread.Sleep(1000);
         }
         base.Close();
      }

      private void label1_MouseDown(object sender, MouseButtonEventArgs e)
      {
         exitLabel.RenderTransform = new ScaleTransform(1.3, 1.3);

         exitLabel.Content = "Bye";
         Thread.Sleep(300);
      }

      private void exitLabel_MouseUp(object sender, MouseButtonEventArgs e)
      {
         Thread.Sleep(1000);
         base.Close();
      }

      private void exitLabel_MouseEnter(object sender, MouseEventArgs e)
      {
         exitLabel.RenderTransform = new ScaleTransform(1.3, 1.3);
      }

      private void exitLabel_MouseLeave(object sender, MouseEventArgs e)
      {
         exitLabel.RenderTransform = new ScaleTransform(1, 1);
      }
   }
}
