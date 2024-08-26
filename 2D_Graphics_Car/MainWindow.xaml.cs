using System;
using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GraphicsCar
{
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();

         timerPause.Start();//Pause Label effect

         userCarTopPos = (int)UserCar.Margin.Top;
         userCarLeftPos = (int)UserCar.Margin.Left;
      }

      int crash = 0;
      double angleArrow = 220;
      int userCarDrivingTick = 10;
      int accelerate = 5;
      static int angle = 0;
      static int userCarTopPos;
      static int userCarLeftPos;
      static int roadSpeed = 4;
      int distanceTraveled = 0;
      static double enemyCarSpeed = 0.4;
      static int userCarControlSpeed = 10;
      static int userCarTimerSpeed = 1;
      static int pauseTimerSpeed = 10;
      static bool pauseCheck = true;
      static bool pauseScaleCheck = true;
      static double enemCarsScale = 1.0;
      static double userCarScale = 1.0;
      static double pauseScale = 1.0;
      static double scaleZoom = 0.007;

      Process stopStartScreen = new Process();

      Image[] enemyCarsArr = new Image[4];
      Image[] pavementArr = new Image[6];
      Image[] linesArr = new Image[10];
      Image[] treesArr = new Image[4];
      Random randomGen = new Random();

      DispatcherTimer timerAllObjects = new DispatcherTimer();
      DispatcherTimer timerPause = new DispatcherTimer();
      DispatcherTimer timerUserCar = new DispatcherTimer();
      DispatcherTimer timerUserCarDriving_Media = new DispatcherTimer();

      RotateTransform rotateTransform = new RotateTransform(angle);

      //-------------------Methods--------------------
      private void PavementMove()
      {
         foreach (var pavement in pavementArr)
         {
            //Lines move
            pavement.Margin = new Thickness(pavement.Margin.Left, pavement.Margin.Top + roadSpeed, 0, 0);

            //Repeate Pavements
            if (pavement.Margin.Top >= Height)
            {
               pavement.Margin = new Thickness(pavement.Margin.Left, -pavement.Height + 5, 0, 0);
            }
         }
      }
      private void LinesMove()
      {
         foreach (var line in linesArr)
         {
            //Move Lines
            line.Margin = new Thickness(line.Margin.Left, line.Margin.Top + roadSpeed, 0, 0);

            //Repeate Lines
            if (line.Margin.Top >= this.Height + line.Height) { line.Margin = new Thickness(line.Margin.Left, -line.Height, 0, 0); }
         }
      }
      private void SpeedBumpMove()
      {
         //Speed Bump Print
         speedBump1.Margin = new Thickness(speedBump1.Margin.Left, speedBump1.Margin.Top + roadSpeed, 0, 0);
         speedBump2.Margin = new Thickness(speedBump2.Margin.Left, speedBump2.Margin.Top + roadSpeed, 0, 0);

         if (speedBump1.Margin.Top >= this.Height + 100)
         { speedBump1.Margin = new Thickness(speedBump1.Margin.Left, randomGen.Next(-1000, -500), 0, 0); }
         if (speedBump2.Margin.Top >= this.Height + 100)
         { speedBump2.Margin = new Thickness(speedBump2.Margin.Left, randomGen.Next(-500, -10), 0, 0); }
      }
      private void UserCarScale()
      {
         ///UserCar Over speedBump1
         //Front of the car
         if (UserCar.Margin.Top < speedBump1.Margin.Top
            &&
            (UserCar.Margin.Top + UserCar.Height + 50 > speedBump1.Margin.Top + speedBump1.Height)
            &&
           (UserCar.Margin.Left + UserCar.Width / 2 + 5 > speedBump1.Margin.Left
            &&
           (UserCar.Margin.Left + 30 < speedBump1.Margin.Left + speedBump1.Width)))
         {
            if (userCarScale < 1.15) { userCarScale += scaleZoom; }
         }
         ///UserCar Over speedBump2
         //Front of the car
         else if (UserCar.Margin.Top < speedBump2.Margin.Top
             &&
             (UserCar.Margin.Top + UserCar.Height + 50 > speedBump2.Margin.Top + speedBump2.Height)
             &&
            (UserCar.Margin.Left + UserCar.Width / 2 + 5 > speedBump2.Margin.Left
             &&
            (UserCar.Margin.Left + 30 < speedBump2.Margin.Left + speedBump2.Width)))
         {
            if (userCarScale < 1.15) { userCarScale += scaleZoom; }
         }
         ///After Speed Bump
         //Back of the car
         else if (userCarScale > 1.0) { userCarScale -= scaleZoom; }

         //Scale Users Car
         UserCar.LayoutTransform = new ScaleTransform(userCarScale, userCarScale);
      }
      private void UserCarCrash_PushEnemyCars()
      {
         foreach (var carEnemyItem in enemyCarsArr)
         {
            double userCarLeft = UserCar.Margin.Left;
            double userCarRight = UserCar.Margin.Left + UserCar.Width;
            double userCarTop = UserCar.Margin.Top;
            double userCarBottom = UserCar.Margin.Top + UserCar.Height;

            double enemyCarLeft = carEnemyItem.Margin.Left;
            double enemyCarRight = carEnemyItem.Margin.Left + carEnemyItem.Width;
            double enemyCarTop = carEnemyItem.Margin.Top;
            double enemyCarBottom = carEnemyItem.Margin.Top + carEnemyItem.Height;

            //Crash with other cars
            if ((userCarTop < enemyCarBottom && userCarTop > enemyCarBottom - 20)
               &&
               (userCarRight - 15 > enemyCarLeft && userCarLeft + 15 < enemyCarRight))
            {
               pauseLabel.Visibility = Visibility.Visible;
               splash.Visibility = Visibility.Visible;
               timerPause.Start();//Pause Label effect
               timerUserCar.Stop();
               timerAllObjects.Stop();
               pauseCheck = false;
               splash.Margin = new Thickness(userCarLeft, userCarTop - splash.Height / 2, 0, 0);
               UserCarCrashDeformation();
               
               userCarCrashMedia.Source = new Uri(@"..\..\Resources\CarBrakeCrash.wav", UriKind.Relative);
               userCarCrashMedia.LoadedBehavior = MediaState.Play;

               timerUserCarDriving_Media.Stop();
               userCarDrivingMedia.LoadedBehavior = MediaState.Pause;
               angleArrow = 220;
               arrow.RenderTransform = new RotateTransform(angleArrow);
            }

            //Pushing enemy cars 
            //Right side user car
            if ((userCarTop + 175 > enemyCarTop && userCarTop + 10 < enemyCarBottom)//Push with front of the user car
               &&
               (userCarRight - 5 > enemyCarLeft && userCarRight < enemyCarLeft + 17))//Push with side of the user car
            {
               pushEnemyCarsMedia.Source = new Uri(@"..\..\Resources\pushEnemyCars.wav", UriKind.Relative);
               pushEnemyCarsMedia.LoadedBehavior = MediaState.Play;
               carEnemyItem.RenderTransform = new ScaleTransform(enemCarsScale, enemCarsScale);
               carEnemyItem.Margin = new Thickness(enemyCarLeft + userCarControlSpeed, enemyCarTop + enemyCarSpeed, 0, 0);
            }
            //Left side user car
            if ((userCarTop + 175 > enemyCarTop && userCarTop + 10 < enemyCarBottom) //Push with front of the user car
              &&
              (userCarLeft + 5 < enemyCarRight && userCarLeft > enemyCarRight - 17)) //Push with side of the user car
            {
               pushEnemyCarsMedia.Source = new Uri(@"..\..\Resources\pushEnemyCars.wav", UriKind.Relative);
               pushEnemyCarsMedia.LoadedBehavior = MediaState.Play;
               carEnemyItem.RenderTransform = new ScaleTransform(enemCarsScale, enemCarsScale);
               carEnemyItem.Margin = new Thickness(enemyCarLeft - userCarControlSpeed, enemyCarTop + enemyCarSpeed, 0, 0);
            }
         }
      }
      private void userCarCrashWithTrees()
      {
         foreach (var tree in treesArr)
         {
            double userCarTop = UserCar.Margin.Top;
            double userCarLeft = UserCar.Margin.Left;
            double userCarRight = UserCar.Margin.Left + UserCar.Width;

            double treeLeft = tree.Margin.Left;
            double treeRight = tree.Margin.Left + tree.Width;
            double treeBottom = tree.Margin.Top + tree.Height;

            if (treeLeft > 510)//Right trees impact
            {
               if (userCarRight > treeRight - 100 && (userCarTop < treeBottom - 80 && userCarTop > treeBottom - 100))
               {
                  pauseLabel.Visibility = Visibility.Visible;
                  timerPause.Start();//Pause Label effect
                  timerUserCar.Stop();
                  timerAllObjects.Stop();
                  pauseCheck = false;
                  userCarCrashMedia.Source = new Uri(@"..\..\Resources\carBrakeCrash.wav", UriKind.Relative);
                  userCarCrashMedia.LoadedBehavior = MediaState.Play;
                  timerUserCarDriving_Media.Stop();
                  splash.Visibility = Visibility.Visible;
                  splash.Margin = new Thickness(userCarLeft, userCarTop - splash.Height / 2, 0, 0);
                  UserCarCrashDeformation();
                  timerUserCarDriving_Media.Stop();
                  userCarDrivingMedia.LoadedBehavior = MediaState.Pause;
                  angleArrow = 220;
                  arrow.RenderTransform = new RotateTransform(angleArrow);
               }
            }
            if (treeRight < 200)//Left trees impact
            {
               if (userCarLeft < treeRight - 80 && (userCarTop < treeBottom - 80 && userCarTop > treeBottom - 100))
               {
                  pauseLabel.Visibility = Visibility.Visible;
                  timerPause.Start();//Pause Label effect
                  timerUserCar.Stop();
                  timerAllObjects.Stop();
                  pauseCheck = false;
                  userCarCrashMedia.Source = new Uri(@"..\..\Resources\carBrakeCrash.wav", UriKind.Relative);
                  userCarCrashMedia.LoadedBehavior = MediaState.Play;
                  timerUserCarDriving_Media.Stop();
                  splash.Visibility = Visibility.Visible;
                  splash.Margin = new Thickness(userCarLeft, userCarTop - splash.Height / 2, 0, 0);
                  UserCarCrashDeformation();
                  timerUserCarDriving_Media.Stop();
                  userCarDrivingMedia.LoadedBehavior = MediaState.Pause;
               }
            }
         }
      }
      private void UserCarCrashDeformation()
      {
         pauseCheck = true;

         if (crash == 0)
         {
            crash += 1;
         }
         else if (crash == 1)
         {
            UserCar.Source = new BitmapImage(new Uri(@"..\..\Resources\Red Car Crash1.png", UriKind.Relative));
            crash += 1;
         }
         else if (crash == 2)
         {
            UserCar.Source = new BitmapImage(new Uri(@"..\..\Resources\Red Car Crash2.png", UriKind.Relative));
            crash += 1;
         }
         else if (crash == 3)
         {
            UserCar.Source = new BitmapImage(new Uri(@"..\..\Resources\Red Car Crash3.png", UriKind.Relative));
            crash += 1;
         }
         else if (crash == 4)
         {
            UserCar.Source = new BitmapImage(new Uri(@"..\..\Resources\Red Car Crash4.png", UriKind.Relative));
         }
      }
      private void CarEnemyMoveScale()
      {
         foreach (var enemyCar in enemyCarsArr)
         {
            //Take Car's scale
            enemCarsScale = enemyCar.RenderTransform.Value.M11;

            //For Out of Main Window
            if (enemyCar.Margin.Top > this.Height)
            {
               //Cars appear over the this.Top
               int randomPos = 0;
               switch (randomGen.Next(1, 4))
               {
                  case 1: randomPos = 140; break;
                  case 2: randomPos = 310; break;
                  case 3: randomPos = 470; break;
               }
               Thread.Sleep(3);
               enemyCar.Margin = new Thickness(randomPos, this.Top - randomGen.Next(200, 1500), 0, 0);
            }

            ///Over speedBump1
            //Front of the car
            if (enemyCar.Margin.Top < speedBump1.Margin.Top
               &&
               (enemyCar.Margin.Top + enemyCar.Height + 50 > speedBump1.Margin.Top + speedBump1.Height)
               &&
              (enemyCar.Margin.Left + enemyCar.Width / 2 + 5 > speedBump1.Margin.Left
               &&
              (enemyCar.Margin.Left + 30 < speedBump1.Margin.Left + speedBump1.Width)))
            {
               if (enemCarsScale < 1.175) { enemCarsScale += scaleZoom; }
            }
            ///Over speedBump2
            //Front of the car
            if (enemyCar.Margin.Top < speedBump2.Margin.Top
               &&
               (enemyCar.Margin.Top + enemyCar.Height + 50 > speedBump2.Margin.Top + speedBump2.Height)
               &&
              (enemyCar.Margin.Left + enemyCar.Width / 2 + 5 > speedBump2.Margin.Left
               &&
              (enemyCar.Margin.Left + 30 < speedBump2.Margin.Left + speedBump2.Width)))
            {
               if (enemCarsScale < 1.175) { enemCarsScale += scaleZoom; }
            }
            //After Speed Bump
            //Back of the car
            else if (enemCarsScale > 1.0) { enemCarsScale -= scaleZoom; }

            enemyCar.RenderTransform = new ScaleTransform(enemCarsScale, enemCarsScale);
         }
      }
      private void CarEnemyCrashWithTree()
      {
         foreach (var enemyCar in enemyCarsArr)
         {
            foreach (var tree in treesArr)
            {
               //Enemy cars crash with trees
               double enemyCarTop = enemyCar.Margin.Top;
               double enemyCarLeft = enemyCar.Margin.Left;
               double enemyCarRight = enemyCar.Margin.Left + UserCar.Width;

               double treeLeft = tree.Margin.Left;
               double treeRight = tree.Margin.Left + tree.Width;
               double treeBottom = tree.Margin.Top + tree.Height;

               //Right trees collision 
               if (treeLeft > 520)
               {
                  ///----Трябва часовник за да се изпълни цялата дължина на файла и тогава да спре---------

                  if (enemyCarRight > treeRight - 100 && (enemyCarTop < treeBottom - 80 && enemyCarTop > treeBottom - 100))
                  {
                     carEnemyCrashMedia.Source = new Uri(@"..\..\Resources\carBrakeCrash.wav", UriKind.Relative);
                     carEnemyCrashMedia.LoadedBehavior = MediaState.Play;
                     splashEnemy.Visibility = Visibility.Visible;
                     enemyCar.Margin = new Thickness(enemyCar.Margin.Left, enemyCarTop + roadSpeed, 0, 0);
                     splashEnemy.Margin = new Thickness(enemyCarLeft, enemyCarTop + roadSpeed - splashEnemy.Height / 2, 0, 0);
                  }
                  else
                  {
                     splashEnemy.Visibility = Visibility.Hidden;
                     splashEnemy.Margin = new Thickness(800, 510, 0, 0);
                     enemyCar.Margin = new Thickness(enemyCar.Margin.Left, enemyCarTop + enemyCarSpeed, 0, 0);
                  }
               }
               //Left trees collision
               if (treeLeft < 10)
               {
                  if (enemyCarLeft < treeRight - 80 && (enemyCarTop < treeBottom - 80 && enemyCarTop > treeBottom - 100))
                  {
                     carEnemyCrashMedia.Source = new Uri(@"..\..\Resources\carBrakeCrash.wav", UriKind.Relative);
                     carEnemyCrashMedia.LoadedBehavior = MediaState.Play;
                     splashEnemy.Visibility = Visibility.Visible;
                     enemyCar.Margin = new Thickness(enemyCar.Margin.Left, enemyCarTop + roadSpeed, 0, 0);
                     splashEnemy.Margin = new Thickness(enemyCarLeft, enemyCarTop + roadSpeed - splashEnemy.Height / 2, 0, 0);
                  }
                  else
                  {
                     splashEnemy.Visibility = Visibility.Hidden;
                     splashEnemy.Margin = new Thickness(800, 510, 0, 0);
                     enemyCar.Margin = new Thickness(enemyCar.Margin.Left, enemyCarTop + enemyCarSpeed, 0, 0);
                  }
               }
            }
         }
      }
      private void TreesMove()
      {
         foreach (var tree in treesArr)
         {
            tree.Margin = new Thickness(tree.Margin.Left, tree.Margin.Top + roadSpeed, 0, 0);
            if (tree.Margin.Top > this.Height)
            {
               tree.Margin = new Thickness(tree.Margin.Left, randomGen.Next(-1000, -500) + roadSpeed, 0, 0);
            }
         }
      }
      private void BicycleMove()
      {
         Bicycle.Margin = new Thickness(Bicycle.Margin.Left, Bicycle.Margin.Top + 2, 0, 0);
         if (Bicycle.Margin.Top > this.Height + 1000)
         {
            Bicycle.Margin = new Thickness(Bicycle.Margin.Left, -1000, 0, 0);
         }

         Bird.Margin = new Thickness(Bird.Margin.Left - 1, Bird.Margin.Top - 1, 0, 0);
         if (Bird.Margin.Left < -2000)
         {
            Bird.Margin = new Thickness(2235, 2580 + 3, 0, 0);
         }
      }
      private void ExitTheGame()
      {
         pauseLabel.Visibility = Visibility.Visible;
         timerPause.Start();
         timerUserCar.Stop();
         timerAllObjects.Stop();
         pauseCheck = true;

         if (MessageBox.Show("Are You Sure ?", "Exit Game", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
         {
            Thread.Sleep(400);
            Close();
         }
      }

      //-------------------Events-------------------- 
      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         //Initialize of timers and start
         timerUserCar.Tick += new EventHandler(TimerUserCar);
         timerUserCar.Interval = new TimeSpan(0, 0, 0, 0, userCarTimerSpeed);

         timerAllObjects.Tick += new EventHandler(TimerAllObjects);
         timerAllObjects.Interval = new TimeSpan(0, 0, 0, 0, roadSpeed);

         timerPause.Tick += new EventHandler(TimerPause);
         timerPause.Interval = new TimeSpan(0, 0, 0, 0, pauseTimerSpeed);

         timerUserCarDriving_Media.Tick += new EventHandler(TimerUserCarDrivingMedia);
         timerUserCarDriving_Media.Interval = new TimeSpan(0, 0, 0, userCarDrivingTick);

         //Add Cars in Array
         enemyCarsArr[0] = CarEnemy1;
         enemyCarsArr[1] = CarEnemy2;
         enemyCarsArr[2] = CarEnemy3;
         enemyCarsArr[3] = CarEnemy4;

         //Add pavements
         pavementArr[0] = pavement0;
         pavementArr[1] = pavement1;
         pavementArr[2] = pavement2;
         pavementArr[3] = pavement3;
         pavementArr[4] = pavement4;
         pavementArr[5] = pavement5;

         //Add Lines
         linesArr[0] = line0;
         linesArr[1] = line1;
         linesArr[2] = line2;
         linesArr[3] = line3;
         linesArr[4] = line4;
         linesArr[5] = line5;
         linesArr[6] = line6;
         linesArr[7] = line7;
         linesArr[8] = line8;
         linesArr[9] = line9;

         //Add trees
         treesArr[0] = tree1;
         treesArr[1] = tree2;
         treesArr[2] = tree3;
         treesArr[3] = tree4;

         userCarDrivingMedia.Source = new Uri(@"..\..\Resources\CarDriving.wav", UriKind.Relative);
         userCarDrivingMedia.LoadedBehavior = MediaState.Pause;
      }
      private void Window_KeyDown(object sender, KeyEventArgs e)//Move users car-Pause 
      {
         //Right move car
         if (e.Key == Key.Right && angle > -1)
         {
            if (userCarLeftPos < this.Width - UserCar.Width - 20) { userCarLeftPos += userCarControlSpeed; }
            //Return car stright line
            if (angle <= 15) { timerUserCar.Stop(); angle += 2; }

            //Slowing car while turn right
            angleArrow -= 0.5;
         }
         //Left move car
         if (e.Key == Key.Left && angle < 1)
         {
            if (userCarLeftPos > 20) { userCarLeftPos -= userCarControlSpeed; }
            //Return car stright line
            if (angle >= -15) { timerUserCar.Stop(); angle -= 2; }

            //Slowing car while turn left
            angleArrow -= 0.5;
         }

         if (e.Key == Key.Up)
         {
            if (angleArrow < 420) { angleArrow += accelerate; }
            if (userCarTopPos > this.Top + 10) { userCarTopPos -= userCarControlSpeed; }

         }
         if (e.Key == Key.Down)
         {
            if (angleArrow > 240) { angleArrow -= accelerate; }
            if (userCarTopPos + UserCar.Height + 40 < this.Height) { userCarTopPos += userCarControlSpeed; }
         }

         //Rotate UserCar
         rotateTransform.Angle = angle;
         UserCar.RenderTransform = rotateTransform;

         //Pause Effect
         //Stop Pause Label effect
         if (e.Key == Key.P && pauseCheck == true)
         {
            pauseLabel.Visibility = Visibility.Hidden;
            splash.Visibility = Visibility.Hidden;
            timerPause.Stop();
            timerUserCar.Start();
            timerAllObjects.Start();
            pauseCheck = false;
            expanderInfo.IsExpanded = false;
            timerUserCarDriving_Media.Start();
            userCarDrivingMedia.LoadedBehavior = MediaState.Play;
         }
         //Start Pause Label effect
         else if (e.Key == Key.P && pauseCheck == false)
         {
            pauseLabel.Visibility = Visibility.Visible;
            timerPause.Start();
            timerUserCar.Stop();
            timerAllObjects.Stop();
            pauseCheck = true;
            expanderInfo.IsExpanded = true;
            timerUserCarDriving_Media.Stop();
            userCarDrivingMedia.LoadedBehavior = MediaState.Pause;
         }

         //Repair the car
         if (e.Key == Key.R && pauseCheck == true)
         {
            crash = 0;
            UserCar.Source = new BitmapImage(new Uri(@"..\..\Resources\RedCar.png", UriKind.Relative));

            userCarLeftPos = (int)this.Width / 2 - (int)UserCar.Width / 2;
            userCarTopPos = (int)this.Height / 2 + (int)UserCar.Height;
         }
         arrow.RenderTransform = new RotateTransform(angleArrow);
         UserCar.Margin = new Thickness(userCarLeftPos, userCarTopPos, 0, 0);

         //Exit game
         if (e.Key == Key.E)
         {
            ExitTheGame();
         }
      }
      private void Window_KeyUp(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Right) { timerUserCar.Start(); }
         if (e.Key == Key.Left) { timerUserCar.Start(); }
      }
      private void labelInfoExit_MouseDown(object sender, MouseButtonEventArgs e)//Exit 
      {
         ExitTheGame();
      }

      //--------------------Timers--------------------
      private void TimerUserCar(object sender, EventArgs e)
      {
         //Return car straight line
         if (angle < 0) { angle += 2; }
         if (angle > 0) { angle -= 2; }

         //Rotate UserCar
         rotateTransform.Angle = angle;
         UserCar.RenderTransform = rotateTransform;

         //Accelerate/Slowing user car
         if (angleArrow < 320) { angleArrow += 1; arrow.RenderTransform = new RotateTransform(angleArrow); }
         else if (angleArrow > 320) { angleArrow -= 1; arrow.RenderTransform = new RotateTransform(angleArrow); }
      }
      private void TimerUserCarDrivingMedia(object sender, EventArgs e)
      {
         userCarDrivingMedia.Source = new Uri(@"..\..\Resources\CarDriving.wav", UriKind.Relative);
         userCarDrivingMedia.LoadedBehavior = MediaState.Play;
      }
      private void TimerPause(object sender, EventArgs e)
      {
         //Zoom In-Out Pause Label
         if (pauseScaleCheck == true) { pauseScale += 0.03; if (pauseScale >= 2.5) { pauseScaleCheck = false; } }
         if (pauseScaleCheck == false) { pauseScale -= 0.03; if (pauseScale <= 1) { pauseScaleCheck = true; } }
         pauseLabel.RenderTransform = new ScaleTransform(pauseScale, pauseScale);
      }
      private void TimerAllObjects(object sender, EventArgs e)
      {
         CarEnemyMoveScale();
         CarEnemyCrashWithTree();
         UserCarScale();
         UserCarCrash_PushEnemyCars();
         TreesMove();
         userCarCrashWithTrees();

         LinesMove();
         PavementMove();
         SpeedBumpMove();
         BicycleMove();

         distanceTraveled += 1;
         kmLabel.Content = distanceTraveled.ToString();
      }
   }
}
