using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace ShortcutBrowser
{
   public partial class MainWindow : Window
   {
      double iconTopPos;
      double iconLeftPos;
      string programName;
      bool isStartSearch;
      bool checkIfMove;
      bool checkMissIcon;
      bool isProgramStart;
      bool droppedElementBool;
      bool isProgramExists;
      bool registrySearchStop;
      bool isDialogWindowOpen;
      bool turningOffTheProgram;
      bool isRegistrySearchWork;
      bool checkMissContextIcon;
      bool isElementExists_InGrid;
      bool isIconsJuggle;
      string openBtnText = "Open";
      string closeBtnText = "Close";
      static int iconCounter;
      int marginTop_Icon;
      int marginLeft_Icon;
      int marginTop_IconText;
      int marginLeft_IconText;

      DispatcherTimer popupTimer;
      DispatcherTimer maximizeTimer;
      DispatcherTimer minimizeTimer;
      DispatcherTimer moveIconTimer;
      DispatcherTimer startClosingTimer;
      DispatcherTimer showIconsInWindowTimer;
      DispatcherTimer progressBarTimer;

      TextBlock customElement;
      TextBlock customElementText;
      TextBlock someIconContMenu;
      TextBlock someIconContMenuText;
      TextBlock someIconInfo;
      TextBlock someIconStart;

      Point mousePos;
      BitmapSource bmpSrc;
      TextBlock popupText;
      ImageBrush pictureBackground;
      List<TextBlock> listMinMaxSize;
      List<TextBlock> listElements_SearchTemp;
      List<TextBlock> listElements_SearchResult;
      List<string> listDelElements_RegSearch;
      SolidColorBrush counter_BackgroundTemp;

      Assembly curAssembly;
      RegistryKey regKeyFolder;
      RegistryKey StartupPath;

      public MainWindow()
      {
         InitializeComponent();

         maximizeTimer = new DispatcherTimer();
         minimizeTimer = new DispatcherTimer();
         popupTimer = new DispatcherTimer();
         moveIconTimer = new DispatcherTimer();
         startClosingTimer = new DispatcherTimer();
         showIconsInWindowTimer = new DispatcherTimer();
         progressBarTimer = new DispatcherTimer();

       //Activated += StartTheProgram;
      }

      private void StartTheProgram(object sender, EventArgs e)//Добавено на 28.03.2021
      {
         maximizeTimer.Start();
         minimizeTimer.Stop();
      }
   
      ///------------------------------------ Main Events ------------------------------------
      private void scrollGrid_Loading(object sender, RoutedEventArgs e) //Промяна на 28.03.2021
      {
         this.Left = 0;
         this.Top = 0;

         RemoveFromTaskbar_Behaviour();

         //maximizeTimer = new DispatcherTimer();
         //minimizeTimer = new DispatcherTimer();
         //popupTimer = new DispatcherTimer();
         //moveIconTimer = new DispatcherTimer();
         //startClosingTimer = new DispatcherTimer();
         //showIconsInWindowTimer = new DispatcherTimer();
         //progressBarTimer = new DispatcherTimer();

         checkMissContextIcon = true;

         maximizeTimer.Start();
         SetWindowBackground();
         AutoStart_Behaviour();
         isProgramStart = true;

         counter_BackgroundTemp = (SolidColorBrush)counter_Background.Background;//цвят за background
      }
      private void scrollGrid_IconsDropSave(object sender, DragEventArgs e)
      {
         mousePos = e.GetPosition(scrollGrid);

         if (isRegistrySearchWork == true) { MessageBox.Show("Изчакайте търсенето на програми да приключи!", "Момент..."); return; }
         if (isStartSearch == true) { MessageBox.Show("Първо завършете търсенето!", "Момент..."); return; }
         droppedElementBool = true;

         if (scrollGrid.Children.Count < 1500)
         {
            try
            {
               string[] droppedObject = (string[])e.Data.GetData(DataFormats.FileDrop, false);//Взима пътя на файла
               string fullPathAndName = droppedObject[0];

               //Създава се икона, задава й се текст под нея, задават се методи, подредба, сянка и др.
               CreateElements_DropSaveElements_Method(fullPathAndName);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
         }
         else { MessageBox.Show("No more space.", "Opss!!!", MessageBoxButton.OK, MessageBoxImage.Information); }
      }
      private async void loadFromRegistry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         isRegistrySearchWork = true;//Да на се свива прозореца докато се търси в регистрите
         droppedElementBool = false;
         isDialogWindowOpen = true;
         startClosingTimer.Stop();

         if (e.ChangedButton == MouseButton.Left && isStartSearch == false)
         {
            if (MessageBox.Show("Load programs from windows registry?", "Loading Programs", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
               //Извършване на ново търсене и изтриване на файла със запазени, изтрити програми
               var dialogRes = MessageBox.Show("'Yes' - Търсене на новоинсталирани програми! \n'No' - Извършване на начално търсене?", "Loading Programs", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

               if (dialogRes == MessageBoxResult.No)
               {
                  using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedDeletedElementsPath.ini"))
                  {
                     writer.WriteLine(string.Empty);
                     writer.Close();
                  }
               }
               else if (dialogRes == MessageBoxResult.Cancel) { return; }

               listDelElements_RegSearch = new List<string>(); //Временно прехвърляне на изтрити елементи, за сравнение, за да не се повтарят при повторно търсене в регистъра
               progressBar.Visibility = Visibility.Visible;
               progressBarText.Visibility = Visibility.Visible;
               progressBarTimer.Start();
               labelRegistrySearchStop.Visibility = Visibility.Visible;
               backgrStopReg_Search.Visibility = Visibility.Visible;

               RegistryKey HKEY_CURRENT_USER = Registry.CurrentUser.OpenSubKey("Software");
               RegistryKey HKEY_USERS = Registry.Users;
               RegistryKey HKEY_LOCAL_MACHINE = Registry.LocalMachine;
               RegistryKey HKEY_CLASSES_ROOT = Registry.ClassesRoot;

               //Метод за async изпълнение на рекурсивно търсене на програми в windows регистъра 
               //трябва да има и думата 'async' в метода горе "private async <--!!!!! void loadFromRegistry_MouseDoubleClick(...)"
               var task1 = Task.Factory.StartNew(() => SearchRegistry_Recursion(HKEY_CURRENT_USER));//Метода за търсене
               await task1;
               HKEY_CURRENT_USER.Close();
               var task2 = Task.Factory.StartNew(() => SearchRegistry_Recursion(HKEY_USERS));//Метода за търсене
               await task2;
               HKEY_USERS.Close();
               var task3 = Task.Factory.StartNew(() => SearchRegistry_Recursion(HKEY_LOCAL_MACHINE));//Метода за търсене
               await task3;
               HKEY_LOCAL_MACHINE.Close();
               var task4 = Task.Factory.StartNew(() => SearchRegistry_Recursion(HKEY_CLASSES_ROOT));//Метода за търсене
               await task4;
               HKEY_CLASSES_ROOT.Close();

               progressBarTimer.Stop();
               MessageBox.Show("  Done!  ");
               isRegistrySearchWork = false;//Да на се свива прозореца донато се търси в регистрите
               progressBar.Visibility = Visibility.Hidden;
               progressBarText.Visibility = Visibility.Hidden;
               progressBar.Value = 0;
               listDelElements_RegSearch = null;
               labelRegistrySearchStop.Visibility = Visibility.Hidden;
               backgrStopReg_Search.Visibility = Visibility.Hidden;
               registrySearchStop = false;
            }
         }
         else
         {
            MessageBox.Show("Opps!!!\nFirst clean searching in window elements!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            isRegistrySearchWork = false;
         }
      }
      private void label_StopRegSearch_Scale_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
         {
            labelRegistrySearchStop.RenderTransform = new ScaleTransform(0.8, 0.8);
            backgrStopReg_Search.RenderTransform = new ScaleTransform(0.8, 0.8);
         }
      }
      private void label_StopRegSearch_MouseUp(object sender, MouseButtonEventArgs e)
      {
         labelRegistrySearchStop.RenderTransform = new ScaleTransform(1, 1);
         backgrStopReg_Search.RenderTransform = new ScaleTransform(1, 1);
         popupField.IsOpen = false;
         popupTimer.Stop();
         registrySearchStop = true;
      }
      private void label_StopRegSearch_MouseEnter(object sender, MouseEventArgs e)
      {
         someIconInfo = new TextBlock();
         someIconInfo.DataContext = " Stop search!";
         popupTimer.Start();
         backgrStopReg_Search.Background = Brushes.AntiqueWhite;
      }
      private void label_StopRegSearch_MouseLeave(object sender, MouseEventArgs e)
      {
         labelRegistrySearchStop.RenderTransform = new ScaleTransform(1, 1);
         backgrStopReg_Search.RenderTransform = new ScaleTransform(1, 1);
         backgrStopReg_Search.Background = Brushes.Transparent;
         popupField.IsOpen = false;
         popupTimer.Stop();
      }
    
      ///-------------------------------------- Timers ----------------------------------------
      private void Timers_GridLoaded(object sender, RoutedEventArgs e)
      {
         maximizeTimer.Tick += new EventHandler(MaximizeTimer_Method);
         maximizeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
         minimizeTimer.Tick += new EventHandler(MinimizeTimer_Method);
         minimizeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
         popupTimer.Tick += new EventHandler(PopupTextTimer_Method);
         popupTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
         moveIconTimer.Tick += new EventHandler(MoveIconTimer_Method);
         moveIconTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
         startClosingTimer.Tick += new EventHandler(StartClosingTimer_Method);
         startClosingTimer.Interval = new TimeSpan(0, 0, 0, 15, 0);
         showIconsInWindowTimer.Tick += new EventHandler(ShowIconsInWindowTimer_Method);
         showIconsInWindowTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
         progressBarTimer.Tick += new EventHandler(ProgressBar_Method);
         progressBarTimer.Interval = new TimeSpan(0, 0, 0, 0, 6);

         //Прехвърляне на иконите от scrollGrid в лист за да се махнат от екрана. За оптимизация на плавното движение 
         listMinMaxSize = new List<TextBlock>();

         // Опрределя минималното свиване на прозореца. Различно е според различните операционни системи и дисплеи
         // this.Height --------------------------------------- цялата височина на прозореца
         // ((FrameworkElement)this.Content).ActualHeight ----- височината на mainGrid, без рамката
         // buttonOpenClose.Height-2 -------------------------- височина на бутона, минус 2, за по точно пасване
         this.MinHeight = this.Height - ((FrameworkElement)this.Content).ActualHeight + btnOpenClose.Height - 1;
      }
      private void MaximizeTimer_Method(object sender, EventArgs e)
      {
         if (this.Width < 1080) { this.Width += 50; }
         else if (this.Height < 720) { this.Height += 50; }
         else
         {
            //При начално стартиране създава елементите след като прозореца е разширен
            if (isProgramStart == true) { ReadProgramsPathFromFile(); isProgramStart = false; }
            else { showIconsInWindowTimer.Start(); }

            maximizeTimer.Stop();
            startClosingTimer.Start();
            counter_Background.Background = counter_BackgroundTemp;
         }
      }
      private void MinimizeTimer_Method(object sender, EventArgs e) //<------Промяна-Отменено в 19.1.2021------
      {
         //Изчистване на searchBox-a преди затваряне
         if (string.IsNullOrWhiteSpace(textBoxSearch.Text) && isRegistrySearchWork == false)
         {
            labelDel_Search.Visibility = Visibility.Visible;
            labelDel_Search.Focus();
            labelDel_Search.Visibility = Visibility.Hidden;
            backgrDel_Search.Visibility = Visibility.Hidden;
         }
         //Прехвърляне на иконите от scrollGrid в лист за да се махнат от екрана. За оптимизация на плавно движение 
         for (int i = 0; i < scrollGrid.Children.Count;)
         {
            listMinMaxSize.Add(scrollGrid.Children[0] as TextBlock);
            scrollGrid.Children.RemoveAt(0);

         }
         btnOpenClose.Content = openBtnText;

         if (this.Height > 36)
         {
            if (this.Height - 50 < 0) { this.Height = 35; }//Ако this.Height e < от 0, дава грешка
            else { this.Height -= 50; }
         }
         else if (this.Width > 260) { this.Width -= 5; counter_Background.Background = Brushes.Transparent; }
         else
         {
            minimizeTimer.Stop();
            //this.Close();  //<-------------------Промяна-Отменено в 19.1.2021--------------------

            if (turningOffTheProgram == true) { this.Close(); }
         }
      }
      private void MoveIconTimer_Method(object sender, EventArgs e)
      {
         //Докато роботи този таймер се движат иконите.

         if (isIconsJuggle == true) { JiggleIconsWhileMove(); }/*Добавено на 7.2.2021 (променено на 26.3.2021)*/
         someIconContMenu.LayoutTransform = new RotateTransform(0);

         //съответната икона се взима от метода 'openProgram_ContextMenu_Click()'
         mousePos = Mouse.GetPosition(scrollGrid);
         iconLeftPos = (mousePos.X - someIconContMenu.Width / 2);
         iconTopPos = (mousePos.Y - someIconContMenu.Height / 2);
         someIconContMenu.Margin = new Thickness(iconLeftPos, iconTopPos, 0, 0);
         someIconContMenuText.Margin = new Thickness(iconLeftPos - 10, iconTopPos + 60, 0, 0);

         startClosingTimer.Stop();//----- Добавено на 7.2.2021
      }
      private void StartClosingTimer_Method(object sender, EventArgs e)
      {
         maximizeTimer.Stop();
         minimizeTimer.Start();
         startClosingTimer.Stop();
      }
      private void PopupTextTimer_Method(object sender, EventArgs e)
      {
         //---Показва изскачащ прозорец с инфото на файла-------
         popupText = new TextBlock();
         popupText.Text = " " + (string)someIconInfo.DataContext + " ";
         popupText.Background = Brushes.AliceBlue;
         popupText.Foreground = Brushes.DarkBlue;
         popupText.FontSize = 14;

         popupField.Child = popupText;
         popupField.IsOpen = true;
      }
      private void ShowIconsInWindowTimer_Method(object sender, EventArgs e)
      {
         //Прехвърляне на иконите от scrollGrid в лист за да се махнат от екрана. За оптимизация на плавното движение 
         if (listMinMaxSize.Count != 0)
         {
            scrollGrid.Children.Add(listMinMaxSize[0]);
            listMinMaxSize.RemoveAt(0);
         }
         else { showIconsInWindowTimer.Stop(); }
      }
      private void ProgressBar_Method(object sender, EventArgs e)
      {
         progressBar.Value++;
         if (progressBar.Value == 100) { progressBar.Value = 0; }
      }
   
      ///------------------------------ Open/Close btn functions ----------------------------------
      private void btnOpenClose_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
         {
            DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 5, ShadowDepth = 3, Direction = 315, Opacity = 100 };
            btnOpenClose.Effect = shadow;
         }
      }
      private void btnOpenClose_QuitProgram_DoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Right && isRegistrySearchWork == false && turningOffTheProgram == false)
         {
            isDialogWindowOpen = true;
            startClosingTimer.Stop();
            if (MessageBox.Show("Изход от програмата?", "Изключване!", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
               turningOffTheProgram = true;
               minimizeTimer.Start();
            }
         }
      }
      private void btnOpenClose_OpenCloseWindow_MouseUp(object sender, MouseButtonEventArgs e)
      {
         DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 10, ShadowDepth = 7, Direction = 315, Opacity = 100 };
         btnOpenClose.Effect = shadow;

         if (isRegistrySearchWork == false && e.ChangedButton == MouseButton.Left && checkIfMove == false)
         {
            if (btnOpenClose.Content.ToString() == openBtnText)
            {
               btnOpenClose.Content = closeBtnText;
               maximizeTimer.Start();
               minimizeTimer.Stop();
            }
            else
            {
               btnOpenClose.Content = openBtnText;
               maximizeTimer.Stop();
               minimizeTimer.Start();
            }
         }
         checkIfMove = false;

         //Изчистване на searchBox-a преди затваряне
         if (string.IsNullOrWhiteSpace(textBoxSearch.Text) && isRegistrySearchWork == false)
         {
            labelDel_Search.Visibility = Visibility.Visible;
            labelDel_Search.Focus();
            labelDel_Search.Visibility = Visibility.Hidden;
            backgrDel_Search.Visibility = Visibility.Hidden;
         }
      }
      private void btnOpenClose_PopUppInfo_MouseEnter(object sender, MouseEventArgs e)
      {
         DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 12, ShadowDepth = 6, Direction = 315, Opacity = 100 };
         btnOpenClose.Effect = shadow;

         someIconInfo = new TextBlock();
         someIconInfo.DataContext = " Double click 'Right mouse button' to close the program  " + "\n  Hold 'Right mouse button' to move the window ";
         btnOpenClose.Content = closeBtnText;
         maximizeTimer.Start();
         minimizeTimer.Stop();
         popupTimer.Start();
      }
      private void btnOpenClose_MouseLeave(object sender, MouseEventArgs e)
      {
         DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 5, ShadowDepth = 3, Direction = 315, Opacity = 100 };
         btnOpenClose.Effect = shadow;

         popupField.IsOpen = false;
         popupTimer.Stop();
      }
  
      ///---------------------------- Open Program -> Context Menu ---------------------------------
      private void openProgram_MouseUp(object sender, MouseButtonEventArgs e)
      {
         //Всеки бутон има метода "openProgram_ContextMenu_Click" и като се натисне се взима пътя от бутона
         //От тук се прехвърля към метода - OpenProgram_Method(), който създава бутона, който е извън този метод и може да се обработва 
         if (e.ChangedButton == MouseButton.Left && !moveIconTimer.IsEnabled && !progressBarTimer.IsEnabled)//Отваряне на програма
         {
            try
            {
               someIconStart = e.Source as TextBlock;
               Process.Start((string)someIconStart.DataContext);

               btnOpenClose.Content = openBtnText;
               maximizeTimer.Stop();
               minimizeTimer.Start();
            }
            catch (Win32Exception exc)
            {
               MessageBox.Show(exc.Message + Environment.NewLine + "Advice: Drag an element directly from the installation folder!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
         }
         else if (e.ChangedButton == MouseButton.Right)//Отваряне на контекстното меню
         {
            isDialogWindowOpen = true; startClosingTimer.Stop();
            //В този метод се отваря контекстното меню и се създава елемент, който се обработва от методите на контекстното меню
            someIconContMenu = e.Source as TextBlock;//Взима се кликната икона
            int indexIcon = scrollGrid.Children.IndexOf(someIconContMenu); //Взима се текста на кликната икона по нейния индекс
            someIconContMenuText = scrollGrid.Children[indexIcon + 1] as TextBlock;

            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
               labelDel_Search.Visibility = Visibility.Visible;
               labelDel_Search.Focus();
               labelDel_Search.Visibility = Visibility.Hidden;
               backgrDel_Search.Visibility = Visibility.Hidden;
            }
         }
      }
   
      ///--------------------------------- Context Menu ------------------------------------------
      private void openContainingFolder_MenuItem_Click(object sender, RoutedEventArgs e)
      {
         isDialogWindowOpen = true;
         try
         {
            string exeOrFolder = Path.GetExtension(someIconContMenu.DataContext.ToString());
            if (exeOrFolder == "")//Отваряне и влизане в папка
            {
               Process.Start(someIconContMenu.DataContext.ToString());
            }
            else//Отваряне на папката на програма
            {
               exeOrFolder = Path.GetFullPath(someIconContMenu.DataContext.ToString());
               string selectFile = "/select, \"" + exeOrFolder;//Маркира търсения файл
               Process.Start("explorer.exe", selectFile);
            }
         }
         catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
      }
      private void deleteElement_MenuItem_Click(object sender, RoutedEventArgs e)
      {
         isDialogWindowOpen = true;
         startClosingTimer.Stop();
         if (isRegistrySearchWork == false)
         {
            try { DeleteElement_Method(); isDialogWindowOpen = false; }
            catch (Exception exc) { MessageBox.Show(exc.Message, "Error"); }
         }
      }
      private void movingElement_TimerStart_MouseDown(object sender, RoutedEventArgs e)
      {
         if (isRegistrySearchWork == false)
         {
            Panel.SetZIndex(someIconContMenu, 1);
            Panel.SetZIndex(someIconContMenuText, 1);
            moveIconTimer.Start();
         }
      }
      private void movingElement_TimerStop_MouseUp(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left && moveIconTimer.IsEnabled)
         {
            moveIconTimer.Stop();
            Panel.SetZIndex(someIconContMenu, 0);

            foreach (TextBlock icon in scrollGrid.Children) //Задаване 'angle = 0' на иконите след преместване
            { icon.LayoutTransform = new RotateTransform(0); }

            CompareIcon_Margins_InsertInScrollGrid(someIconContMenu, someIconContMenuText);
            ReWriteMovedElements();
         }
      }
    
      ///---------------------------------- Hover Icons Info ---------------------------------------
      private void iconInfo_MouseEnter(object sender, MouseEventArgs e)
      {
         if (!moveIconTimer.IsEnabled)
         {
            someIconInfo = e.Source as TextBlock;
            DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 20, ShadowDepth = 7, Direction = 315, Opacity = 100 };

            someIconInfo.RenderTransform = new ScaleTransform(1.3, 1.3, 20, 20);
            someIconInfo.Effect = shadow;

            popupTimer.Start();
         }
      }
      private void iconInfo_MouseLeave(object sender, MouseEventArgs e)
      {
         if (!moveIconTimer.IsEnabled)
         {
            someIconInfo = e.Source as TextBlock;
            DropShadowEffect shadow = new DropShadowEffect() { BlurRadius = 15, ShadowDepth = 3, Direction = 315, Opacity = 100 };

            someIconInfo.RenderTransform = new ScaleTransform(1, 1);
            someIconInfo.Foreground = Brushes.Black;
            someIconInfo.Effect = shadow;

            popupField.IsOpen = false;
            popupTimer.Stop();
         }
      }
    
      ///---------------------------------- Window Mouse Entere/Leave -------------------------------
      private void mainWindow_MouseEnter(object sender, MouseEventArgs e)
      {
         if (startClosingTimer.IsEnabled) { startClosingTimer.Stop(); }
         isDialogWindowOpen = false;
      }
      private void mainWindow_MouseLeave(object sender, MouseEventArgs e)
      {
         if (!moveIconTimer.IsEnabled && isRegistrySearchWork == false && isDialogWindowOpen == false && isStartSearch == false)
         {
            startClosingTimer.Start();
         }
      }
      private void mainWindow_Close_MouseMove(object sender, MouseEventArgs e)
      {
         if (!moveIconTimer.IsEnabled && isRegistrySearchWork == false && isDialogWindowOpen == false && isStartSearch == false)
         {
            startClosingTimer.Stop();
            startClosingTimer.Start();
         }
      }
    
      ///---------------------------------------- Settings ---------------------------------------------
      private void changeFoldersIcon_DoubleClick(object sender, MouseButtonEventArgs e)
      {
         isDialogWindowOpen = true;
         startClosingTimer.Stop();
         if (e.ChangedButton == MouseButton.Left)
         {
            string lineFolderIcons = "";

            System.Windows.Forms.DialogResult result;
            using (var fdb = new System.Windows.Forms.OpenFileDialog())
            {
               fdb.Filter = "Image file|*.jpg|Image file|*.jpeg|Bitmap file|*.bmp|Transparent image|*.png|All files|*";
               result = fdb.ShowDialog();
               lineFolderIcons = fdb.FileName;
            }
            if (result == System.Windows.Forms.DialogResult.OK)
            {
               //Изтрива пътя към икона във файла с пътища на икони и записва нов път към икона
               using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedIconForFolders.ini"))
               {
                  writer.Write(string.Empty);
                  writer.Write(lineFolderIcons);
               }
               scrollGrid.Children.Clear();
               ReadProgramsPathFromFile();
            }
         }
      }
      private void changeBackgraund_DoubleClick(object sender, MouseButtonEventArgs e)
      {
         isDialogWindowOpen = true;
         startClosingTimer.Stop();
         if (e.ChangedButton == MouseButton.Right)//Задава default background за фон
         {
            var messageBox = MessageBox.Show("Set default background?", "Background", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (messageBox == MessageBoxResult.Yes)
            {
               isDialogWindowOpen = true; startClosingTimer.Stop();
               //Изтрива пътя към снимка във файла с пътища на снимки за background
               using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedBackgroundPicture.ini"))
               {
                  writer.Write(string.Empty);
               }
               mainWindow.Background = defBackground;
            }
            else if (messageBox == MessageBoxResult.No)
            {
               isDialogWindowOpen = true; startClosingTimer.Stop();
               //Задаване на плътен цвят за фон
               System.Windows.Forms.ColorDialog colorBox = new System.Windows.Forms.ColorDialog();
               if (colorBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
               {
                  mainWindow.Background = new SolidColorBrush(Color.FromArgb(colorBox.Color.A, colorBox.Color.R, colorBox.Color.G, colorBox.Color.B));
               }
               //Изтрива пътя към снимка .ini файл за background и записва цвят
               using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedBackgroundPicture.ini"))
               {
                  writer.Write(string.Empty);
                  writer.Write(mainWindow.Background);
               }
            }
         }
         else if (e.ChangedButton == MouseButton.Left)
         {
            isDialogWindowOpen = true; startClosingTimer.Stop();
            //Взима снимка и я слага като тапет и записва пътя й във .ini файл
            string lineBackgrPath;
            System.Windows.Forms.DialogResult result;

            using (var fdb = new System.Windows.Forms.OpenFileDialog())
            {
               fdb.Filter = "Image file|*.jpg|Image file|*.jpeg|Bitmap file|*.bmp|Transparent image|*.png|All files|*";
               result = fdb.ShowDialog();
               lineBackgrPath = fdb.FileName;
            }
            if (result == System.Windows.Forms.DialogResult.OK)
            {
               //Изтрива пътя към снимка от файла за снимки за bacground и записва нов път 
               //След това метода SetWindowBackground(); чете от файла и слага снимката
               using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedBackgroundPicture.ini"))
               {
                  writer.Write(string.Empty);
                  writer.Write(lineBackgrPath);
               }
               SetWindowBackground();
            }
         }
      }
      private void radioBtn_Uniform_Checked(object sender, RoutedEventArgs e)
      {
         try
         {
            pictureBackground.Stretch = Stretch.Uniform;
            WriterSave_StretchMode();
         }
         catch (NullReferenceException) { MessageBox.Show("Background picture is missing!", "Info"); }
      }
      private void radioBtn_UniformToFill_Checked(object sender, RoutedEventArgs e)
      {
         try
         {
            pictureBackground.Stretch = Stretch.UniformToFill;
            WriterSave_StretchMode();
         }
         catch (NullReferenceException) { MessageBox.Show("Background picture is missing!", "Info"); }
      }
      private void radioBtn_None_Checked(object sender, RoutedEventArgs e)
      {
         try
         {
            pictureBackground.Stretch = Stretch.None;
            WriterSave_StretchMode();
         }
         catch (NullReferenceException) { MessageBox.Show("Background picture is missing!", "Info"); }
      }
      private void openSavedElementsFile(object sender, MouseButtonEventArgs e)
      {
         try
         {
            Process.Start(CurrentExeDirectory() + "\\SavedElementsPath.ini");
         }
         catch (Exception ex) { MessageBox.Show(ex.Message + ". \n Плъзнете папка или програма за да създадете файла!", "Info", MessageBoxButton.OK, MessageBoxImage.Information); }
      }
      private void autostart_checkBox_Checked(object sender, RoutedEventArgs e)
      {
         //Автоматично стартиране с Windows - Turn On
         try
         {
            AutoStart_WindowsRegedit();

            using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedAutoStartBehaviour.ini"))
            {
               writer.Write(string.Empty);
               writer.WriteLine("AutoStart-yes");
            }
            if (StartupPath.GetValue(programName) == null)
            {
               StartupPath.SetValue(programName, "\"" + curAssembly.Location + "\"", RegistryValueKind.String);
               StartupPath.Close();
            }
         }
         catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information); }
      }
      private void autostart_checkBox_Unchecked(object sender, RoutedEventArgs e)
      {
         //Автоматично стартиране с windows - Turn Off

         AutoStart_WindowsRegedit();

         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedAutoStartBehaviour.ini"))
         {
            writer.Write(string.Empty);
            writer.WriteLine("AutoStart-no");
         }
         if (StartupPath.GetValue(programName.ToString()) != null)
         {
            StartupPath.DeleteValue(programName.ToString());
            StartupPath.Close();
         }
      }
      private void showInTaskbar_checkBox_Checked(object sender, RoutedEventArgs e)
      {
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedRemoveFromTaskbar.ini"))
         {
            writer.Write(string.Empty);
            writer.Write("RemoveFromTaskbar-yes");
         }
         RemoveFromTaskbar_Behaviour();
      }
      private void showInTaskbar_checkBox_Unchecked(object sender, RoutedEventArgs e)
      {
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedRemoveFromTaskbar.ini"))
         {
            writer.Write(string.Empty);
            writer.Write("RemoveFromTaskbar-no");
         }
         RemoveFromTaskbar_Behaviour();
      }
      private void Icons_Jiggle_Checked(object sender, RoutedEventArgs e)
      {
         isIconsJuggle = true;
      }
      private void Icons_Jiggle_Unchecked(object sender, RoutedEventArgs e)
      {
         isIconsJuggle = false;
      }
   
      ///------------------------------------- Expander --------------------------------------------
      private void expander_Expanded(object sender, RoutedEventArgs e)
      {
         if (progressBarTimer.IsEnabled == true) { expander.IsExpanded = false; }
         Panel.SetZIndex(expander, 1);//Задава приоритет на expandera за да е върху бутоните
      }
      private void gridExpander_MouseLeave(object sender, MouseEventArgs e)
      {
         expander.IsExpanded = false;
         expanderChangeBackgr.IsExpanded = false;
      }
      private void infoLabel_MouseEnter(object sender, MouseEventArgs e)
      {
         //----- Показва инфо -------
         popupText = new TextBlock();
         popupText.Text =
            "  'Right' mouse button click over 'Change backgroung' "
            + "\n  'Left' mouse button click over 'Change backgroung' "
            + "\n  Double click 'Left' mouse button over 'Saved programs list'  "
            + "\n  Double click 'Left' mouse button over 'Load from registry?' to load all programs "
            + "\n  Double click 'Right' mouse button over 'Open/Close button' to exit program "
            + "\n  Hold 'Right' mouse button over 'Open/Close button' to move the window ";
         popupText.Background = Brushes.LightCyan;
         popupText.Foreground = Brushes.DarkBlue;
         popupText.FontSize = 12;

         popupField.Child = popupText;
         popupField.IsOpen = true;
      }
      private void infoLabel_MouseLeave(object sender, MouseEventArgs e)
      {
         popupField.IsOpen = false;
      }
    
      ///-------------------------------------- Move Window -----------------------------------------
      private void buttonOpenClose_MoveWindow_MouseMove(object sender, MouseEventArgs e)
      {
         if (e.LeftButton == MouseButtonState.Pressed)
         {
            popupField.IsOpen = false;
            popupTimer.Stop();
            checkIfMove = true;
            this.DragMove();//Местене на прозореца
         }
      }
    
      ///---------------------------------------- Search --------------------------------------------
      private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
      {
         if (isStartSearch == true && isRegistrySearchWork == false && textBoxSearch.Text != "")//Проверка при стартиране и при празен стринг, като се търси
         {
            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))//Проверка за червения'x'- допълнително
            {
               ///-----------------Търсене-----------------
               foreach (TextBlock iconText in listElements_SearchTemp)
               {
                  if (iconText.Text != "")
                  {
                     if (iconText.Text.StartsWith(textBoxSearch.Text, true, CultureInfo.CurrentCulture))
                     {
                        int index = listElements_SearchTemp.IndexOf(iconText);//Индекса на текста
                        listElements_SearchResult.Add(listElements_SearchTemp[index - 1]);//Иконата
                        listElements_SearchResult.Add(listElements_SearchTemp[index]);//Текста под иконата
                     }
                  }
               }
               scrollGrid.Children.Clear();
               RearrangeElementsFromList(listElements_SearchResult);//Метода за пренареждане на иконите от временния лист в Grid-а
            }
            else
            {
               scrollGrid.Children.Clear();
               RearrangeElementsFromList(listElements_SearchTemp);//Метода за пренареждане на иконите от временния лист в Grid-а
               foreach (TextBlock element in scrollGrid.Children)
               {
                  listElements_SearchTemp.Add(element);
               }
            }
            labelDel_Search.Visibility = Visibility.Visible;
            backgrDel_Search.Visibility = Visibility.Visible;
         }
         else if (isStartSearch == true && isRegistrySearchWork == false && string.IsNullOrWhiteSpace(textBoxSearch.Text))
         {
            scrollGrid.Children.Clear();
            RearrangeElementsFromList(listElements_SearchTemp);
            foreach (TextBlock element in scrollGrid.Children)//Повторно прехвърляне за следващо търсене
            {
               listElements_SearchTemp.Add(element);
            }
            labelDel_Search.Visibility = Visibility.Hidden;
            backgrDel_Search.Visibility = Visibility.Hidden;
         }
         if (textBoxSearch.Text == " ")
         {
            labelDel_Search.Visibility = Visibility.Visible;
            backgrDel_Search.Visibility = Visibility.Visible;
         }
         if (isRegistrySearchWork == true) { labelDel_Search.Visibility = Visibility.Visible; backgrDel_Search.Visibility = Visibility.Visible; }
      }
      private void textBoxSearch_GotFocus(object sender, RoutedEventArgs e)
      {
         if (textBoxSearch.Text == "Search: " && isRegistrySearchWork == false)
         {
            //Създавам временни листи в който временно държа иконите за да ги прехвърля обратно в Grid-а 
            listElements_SearchTemp = new List<TextBlock>();
            //В този лист слагам елементите които са намерени, те се подават на метода за- 
            // -подреждане и след това се прехвърлят в Grid-а за да се покажат на екрана 
            listElements_SearchResult = new List<TextBlock>();//Листа с намерени елементи

            foreach (TextBlock element in scrollGrid.Children)
            {
               listElements_SearchTemp.Add(element);
            }
            textBoxSearch.Text = "";
            isStartSearch = true;
         }
      }
      private void textBoxSearch_LostFocus(object sender, RoutedEventArgs e)
      {
         if (string.IsNullOrWhiteSpace(textBoxSearch.Text) && isRegistrySearchWork == false)
         {
            isStartSearch = false;
            textBoxSearch.Text = "Search: ";

            scrollGrid.Children.Clear();
            RearrangeElementsFromList(listElements_SearchTemp);

            listElements_SearchResult = null;
            listElements_SearchTemp = null; //Нужно е за проверката, за да не се изтриват всички елементи без намерените при търсене
            labelDel_Search.Visibility = Visibility.Hidden;
            backgrDel_Search.Visibility = Visibility.Hidden;
         }
      }
      private void labelDel_Scale_ClearSearch_MouseUp(object sender, MouseButtonEventArgs e)
      {
         //Изчистване на думата в "textBoxSearch" и изчакване за въвеждане на нова дума
         if (isRegistrySearchWork == false)
         {
            scrollGrid.Children.Clear();
            foreach (TextBlock element in listElements_SearchTemp)
            {
               scrollGrid.Children.Add(element);
            }
            listElements_SearchResult.Clear();
            textBoxSearch.Text = "";
         }
         if (isRegistrySearchWork == true)
         {
            isStartSearch = false;
            textBoxSearch.Text = "Search: ";
         }
      }
      private void textBoxSearch_KeyUp_Escape(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Escape)
         {
            isStartSearch = false;
            textBoxSearch.Text = "Search: ";

            if (isRegistrySearchWork == false && labelDel_Search.Visibility == Visibility.Visible)
            {
               scrollGrid.Children.Clear();
               RearrangeElementsFromList(listElements_SearchTemp);
               listElements_SearchResult = null;
               listElements_SearchTemp = null; //Нужно е за проверката, за да не се изтриват всички елементи без намерените при търсене
            }
            labelDel_Search.Visibility = Visibility.Visible;
            labelDel_Search.Focus();
            labelDel_Search.Visibility = Visibility.Hidden;
            backgrDel_Search.Visibility = Visibility.Hidden;
         }
      }
      private void labelDel_Search_Scale_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
         {
            labelDel_Search.RenderTransform = new ScaleTransform(0.8, 0.8);
            backgrDel_Search.RenderTransform = new ScaleTransform(0.8, 0.8);
         }
      }
      private void labelDel_PopUppInfo_MouseEnter(object sender, MouseEventArgs e)
      {
         someIconInfo = new TextBlock();
         someIconInfo.DataContext = " Clear Search box";
         popupTimer.Start();
         backgrDel_Search.Background = Brushes.AntiqueWhite;
      }
      private void labelDel_Search_InfoStop_MouseLeave(object sender, MouseEventArgs e)
      {
         labelDel_Search.RenderTransform = new ScaleTransform(1, 1);
         backgrDel_Search.RenderTransform = new ScaleTransform(1, 1);
         backgrDel_Search.Background = Brushes.Transparent;
         popupField.IsOpen = false;
         popupTimer.Stop();
      }
    
      ///------------------- Methods ------------------------
      private ContextMenu mainContextMenu()
      {
         Image openFolderIcon = new Image();
         Image iconDeleteFile = new Image();
         Image moveIconIcon = new Image();
         try
         {
            openFolderIcon.Source = new BitmapImage(new Uri(CurrentExeDirectory() + "\\Resources\\open-icon-context-m.png"));
            iconDeleteFile.Source = new BitmapImage(new Uri(CurrentExeDirectory() + "\\Resources\\delete-icon-context-m.png"));
            moveIconIcon.Source = new BitmapImage(new Uri(CurrentExeDirectory() + "\\Resources\\move-icon-context-m.png"));
         }
         catch (DirectoryNotFoundException ex)
         {
            if (checkMissContextIcon == true) { MessageBox.Show(ex.Message); checkMissContextIcon = false; }
         }
         catch (FileNotFoundException ex)
         {
            if (checkMissContextIcon == true) { MessageBox.Show(ex.Message); checkMissContextIcon = false; }
         }

         MenuItem openFolder_MenuIt = new MenuItem();
         openFolder_MenuIt.Icon = openFolderIcon;
         openFolder_MenuIt.Header = "Open containing folder";
         openFolder_MenuIt.Height = 21;
         openFolder_MenuIt.FontSize = 12;
         openFolder_MenuIt.Click += openContainingFolder_MenuItem_Click;


         MenuItem deleteFolder_MenuIt = new MenuItem();
         deleteFolder_MenuIt.Icon = iconDeleteFile;
         deleteFolder_MenuIt.Header = "Delete item";
         deleteFolder_MenuIt.Height = 21;
         deleteFolder_MenuIt.FontSize = 12;
         deleteFolder_MenuIt.Click += deleteElement_MenuItem_Click;

         MenuItem moveIcon_MenuIt = new MenuItem();
         moveIcon_MenuIt.Icon = moveIconIcon;
         moveIcon_MenuIt.Header = "Move item";
         moveIcon_MenuIt.Height = 21;
         moveIcon_MenuIt.FontSize = 12;
         moveIcon_MenuIt.Click += movingElement_TimerStart_MouseDown;


         List<MenuItem> menuItemList = new List<MenuItem>();
         menuItemList.Add(openFolder_MenuIt);
         menuItemList.Add(deleteFolder_MenuIt);
         menuItemList.Add(moveIcon_MenuIt);

         ContextMenu contextMenu = new ContextMenu();
         contextMenu.Height = 74;
         contextMenu.Width = 172;
         contextMenu.ItemsSource = menuItemList;

         return contextMenu;
      }
      private void ReadProgramsPathFromFile()
      {
         iconCounter = 0;
         marginTop_Icon = 15;
         marginLeft_Icon = 25;
         marginTop_IconText = 70;
         marginLeft_IconText = 15;

         //----Прочита файла с бутоните и ги създава----
         string linePath = "";
         try
         {
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedElementsPath.ini"))
            {
               while ((linePath = reader.ReadLine()) != null)
               {
                  if (linePath != "")
                  {
                     customElement = new TextBlock();
                     customElementText = new TextBlock();
                     iconCounter++;

                     customElement.DataContext = linePath;// Пътя към файл(програма)
                     customElementText.Text = Path.GetFileName(linePath);// Текст под икона
                     if (customElementText.Text.ToString() == "")//Проверка за прихващане на C: D: E: и др. директории
                     {
                        customElementText.Text = linePath;// Текст под икона
                     }
                     SetMethodsToElements();
                     IconsStyle_Arrangement();
                     CheckIfFolder_SetIcon(linePath);

                     Panel.SetZIndex(customElement, 0);
                     Panel.SetZIndex(customElementText, 0);

                     // Слага елемент в прозореца в "Grid"("Grid"-а и трябва да има име)
                     scrollGrid.Children.Add(customElement);
                     scrollGrid.Children.Add(customElementText);
                  }
               }
            }
            labelCounter.Content = "Count: ";
            labelCounter.Content = labelCounter.Content + iconCounter.ToString();
         }
         catch (FileNotFoundException ex)
         {
            MessageBox.Show(ex.Message + "\n Файлът ще се създаде когато се добавят програми или папки!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
         }
      }
      private void CreateElements_DropSaveElements_Method(string fullPathAndName)
      {
         string elementPath;

         customElement = new TextBlock();
         customElementText = new TextBlock();

         customElement.DataContext = fullPathAndName;// Пътя към файл(програма)
         customElementText.Text = Path.GetFileName(fullPathAndName);// Текст под икона

         if (customElementText.Text.ToString() == "")//Проверка за прихващане на C: D: E: и др. директории
         { customElementText.Text = fullPathAndName; }// Текст под икона

         foreach (TextBlock element in scrollGrid.Children)//Проверка за съществуваща икона в списъка
         {
            if (element.DataContext != null && element.DataContext.Equals(customElement.DataContext))
            {
               if (droppedElementBool == true)
               {
                  isElementExists_InGrid = true;
                  MessageBox.Show("This item already exists in the list!"); break;
               }
               else { isElementExists_InGrid = true; break; }
            }
         }

         if (isElementExists_InGrid == false)//Ако папките нямат зададена икона и са много, ще покаже грешка само веднъж
         {
            elementPath = fullPathAndName;//Взима се пътя за проверка за икона, папка или несъществуващ път

            CheckIfFolder_SetIcon(elementPath);//Взима се иконата от дропнатата програма или се слага папка

            if (isProgramExists == true)//Ако програмата е изтрита и само пътя е останал-той е невалиден
            {
               //Създаване на елемент
               SetMethodsToElements();
               IconsStyle_Arrangement();

               Panel.SetZIndex(customElement, 0);
               Panel.SetZIndex(customElementText, 0);

               if (scrollGrid.Children.Count == 0 || isRegistrySearchWork == true)//За първия пустан елемент
               {
                  scrollGrid.Children.Add(customElement);
                  scrollGrid.Children.Add(customElementText);
                  SavingElementInFile_StrWriter(customElement.DataContext.ToString());//За търсене в регистъра
               }
               else
               {
                  var point = System.Windows.Forms.Control.MousePosition;
                  //Point mousePos = new Point(point.X, point.Y);

                  //Задава позиция(.Margin)на елемента, за да се постави на позицията на курсора
                  iconLeftPos = (mousePos.X - customElement.Width / 2);
                  iconTopPos = (mousePos.Y - customElement.Height / 2);
                  customElement.Margin = new Thickness(iconLeftPos, iconTopPos, 0, 0);
                  customElementText.Margin = new Thickness(iconLeftPos/* - 10*/, iconTopPos /*+ 60*/, 0, 0);

                  //Добавяне на елементи в "Grid"-а, на определена позиция ("Grid"-а трябва да има име)
                  CompareIcon_Margins_InsertInScrollGrid(customElement, customElementText);

                  ReWriteMovedElements();//Презаписване на елементите в текстовия файл
               }
               iconCounter++;
               labelCounter.Content = "Count: ";
               labelCounter.Content = labelCounter.Content + iconCounter.ToString();
            }
         }
         isElementExists_InGrid = false;
      }
      private void IconsStyle_Arrangement()
      {
         DropShadowEffect shadowIcons = new DropShadowEffect() { BlurRadius = 15, ShadowDepth = 3, Direction = 315, Opacity = 100 };
         DropShadowEffect shadowText = new DropShadowEffect() { BlurRadius = 5, ShadowDepth = 1, Direction = 315, Opacity = 100 };

         customElement.VerticalAlignment = VerticalAlignment.Top;
         customElement.HorizontalAlignment = HorizontalAlignment.Left;
         customElement.Margin = new Thickness(marginLeft_Icon, marginTop_Icon, 0, 0);
         customElement.Cursor = Cursors.Hand;
         customElement.Effect = shadowIcons;
         customElement.Height = 45;
         customElement.Width = 45;

         // TextBlock с имената на бутоните
         customElementText.VerticalAlignment = VerticalAlignment.Top;
         customElementText.HorizontalAlignment = HorizontalAlignment.Left;
         customElementText.Margin = new Thickness(marginLeft_IconText, marginTop_IconText, 0, 0);
         customElementText.FontFamily = new FontFamily("Segoe UI Symbol");
         customElementText.Foreground = Brushes.White;
         customElementText.Padding = new Thickness(0, 0, 0, 0);
         customElementText.Effect = shadowText;
         customElementText.TextAlignment = TextAlignment.Center;
         customElementText.Height = 35;
         customElementText.Width = customElement.Width + 17;
         customElementText.TextTrimming = TextTrimming.CharacterEllipsis;
         customElementText.TextWrapping = TextWrapping.Wrap;
         customElementText.FontSize = 12;

         //За подреждане на бутоните
         marginLeft_Icon += 75;
         if (marginLeft_Icon > 1000) { marginLeft_Icon = 20; marginTop_Icon += 100; }

         //За подреждане на текста под бутоните
         marginLeft_IconText += 75;
         if (marginLeft_IconText > 1000) { marginLeft_IconText = 10; marginTop_IconText += 100; }
      }
      private void SetMethodsToElements()
      {
         //Задава методи на всеки бутон
         customElement.MouseUp += openProgram_MouseUp;
         customElement.MouseUp += movingElement_TimerStop_MouseUp;
         customElement.MouseEnter += iconInfo_MouseEnter;
         customElement.MouseLeave += iconInfo_MouseLeave;
         customElement.ContextMenu = mainContextMenu();
      }
      private void DeleteElement_Method()
      {
         //Иконата-'someIcon' се създава като се кликне върху икона и се мине през метода 'openProgram_ContextMenu_Click()'
         if (MessageBox.Show("Изтриване на пряк път - '" + someIconContMenu.DataContext + "'", "Изтриване", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
         {
            List<string> iconsList = new List<string>();

            //Премахване на изтрития бутон от файла със записани елементи
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedElementsPath.ini"))
            {
               string lineIcon = "";
               while ((lineIcon = reader.ReadLine()) != null)
               {
                  if (lineIcon != "")
                  {
                     //Всеки различен път към прогр.се записва в List<>, а изтриваният се записва в лист за изтрити елементи
                     if (!lineIcon.Equals(someIconContMenu.DataContext)) { iconsList.Add(lineIcon); }
                     else if (lineIcon.Equals(someIconContMenu.DataContext)) { WriteDeletedElementInFile(lineIcon); }
                  }
               }
            }

            //Изтрива всички пътища на икони във файла, за да се презапишат
            DeleteElementsInFile_StrWriter();

            //Презаписва пътищата във файлa без изтрития бутон
            foreach (var IconToFile in iconsList)
            {
               SavingElementInFile_StrWriter(IconToFile);
            }

            //Изтриване на икона и от 'scrollGrid.Children()'
            //index-а е същия защото първо се изтрива елемента и те се пренареждат с едно напред и текста заема същия index
            int index = scrollGrid.Children.IndexOf(someIconContMenu);
            scrollGrid.Children.RemoveAt(index);//Изтрива икона
            scrollGrid.Children.RemoveAt(index);//Изтрива текста под икона 

            //Изтриване на елемент и от временния лист'listElements_SearchTemp' 
            //index-а е същия защото първо се изтрива елемента и те се пренареждат с едно напред и текста заема същия index
            if (isStartSearch == true)
            {
               int indexList = listElements_SearchTemp.IndexOf(someIconContMenu);
               listElements_SearchTemp.RemoveAt(indexList);//Изтрива икона
               listElements_SearchTemp.RemoveAt(indexList);//Изтрива текста под икона 
            }
            RearrangeElements_AfterDeleted();//Прочитане на елементите от файла с пътища и създаване без изтрития
         }
      }
      private void DeleteElementsInFile_StrWriter()
      {
         //Изтрива всички пътища на елементи във файла - за презаписване, след това
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedElementsPath.ini"))
         {
            writer.Write(string.Empty);
         }
      }
      private void WriteDeletedElementInFile(string deletedElPath)
      {
         //Записване на изтрит елемент, за да не се повтаря при повторно търсене на прогр.в регистъра
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedDeletedElementsPath.ini", true))
         {
            writer.WriteLine(deletedElPath);
         }
      }
      private void SavingElementInFile_StrWriter(string pathToFile)
      {
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedElementsPath.ini", true))
         {
            writer.WriteLine(pathToFile);
         }
      }
      private void CheckIfFolder_SetIcon(string path)
      {
         try
         {
            if (Path.GetExtension(path) != "")
            {
               try //Взима иконата на файла, а ако e папка, я пропуска
               {
                  bmpSrc = null;
                  System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                  bmpSrc = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(600, 600));
                  icon.Dispose();
                  isProgramExists = true;//Ако е програма и пътя е валиден 
                  customElement.Background = new ImageBrush(bmpSrc);
               }
               catch (Exception) { isProgramExists = false; }//Ако е програма но пътя е празен-невалиден 
            }
            else if (Path.GetExtension(path) == "")//Aко e папка
            {
               //Зареждане на пътя до иконата за папкитe
               using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedIconForFolders.ini"))
               {
                  string lineSetIconToFolder = reader.ReadLine();
                  customElement.Background = new ImageBrush(new BitmapImage(new Uri(lineSetIconToFolder, UriKind.RelativeOrAbsolute)));
               }
            }
         }
         catch (Exception)
         {
            if (checkMissIcon == false)//Проверка за да не показва грешката за всяка папка защото може да са много
            {
               MessageBox.Show("Hello there! \nCannot find path to file contains folder icon information! \nSet icon image for the folders from the Settings!!!", "Error", MessageBoxButton.OK);
            }
            checkMissIcon = true;
            isProgramExists = true;
         }
      }
      private void SetWindowBackground()
      {
         string lineBackgrPath = "";
         try
         {
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedBackgroundPicture.ini"))
            {
               lineBackgrPath = reader.ReadLine();
            }
            if (lineBackgrPath != null)//Задаване на снимка за фон
            {
               pictureBackground = new ImageBrush(new BitmapImage(new Uri(lineBackgrPath)));
               mainWindow.Background = pictureBackground;
               Background_StretchMode();
            }
         }
         catch (FormatException)//Задаване на записан цвят за фон
         {
            mainWindow.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(lineBackgrPath));
         }
         catch (DirectoryNotFoundException)
         {
            mainWindow.Background = defBackground;//Фон по подразбиране, задава от метода 'changeBackgraund_DoubleClick()' по горе
         }
         catch (FileNotFoundException) { }
         catch (NotSupportedException)
         {
            MessageBox.Show("Този файл не се поддържа! \n Изберете друг файл за фон!");
         }
      }
      private void Background_StretchMode()
      {
         try
         {
            string line;
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedBackgroundStretchMode.ini"))
            {
               line = reader.ReadLine();
            }
            switch (line)
            {
               case "None": pictureBackground.Stretch = Stretch.None; radioButtNone.IsChecked = true; break;
               case "Uniform": pictureBackground.Stretch = Stretch.Uniform; radioButtUniform.IsChecked = true; break;
               case "UniformToFill": pictureBackground.Stretch = Stretch.UniformToFill; radioButtUniformToFill.IsChecked = true; break;
               default: pictureBackground.Stretch = Stretch.Fill; break;
            }
         }
         catch (FileNotFoundException) { WriterSave_StretchMode(); }
      }
      private void WriterSave_StretchMode()
      {
         //Изтрива "Stretch Mode", на снимката за background във файла и записва нов "Stretch Mode"
         using (StreamWriter writer = new StreamWriter(CurrentExeDirectory() + "\\SavedBackgroundStretchMode.ini"))
         {
            writer.Write(string.Empty);
            writer.Write(pictureBackground.Stretch.ToString());
         }
      }
      private void AutoStart_Behaviour()
      {
         //Този метод се изпълнява при стартиране на програмата
         try
         {
            AutoStart_WindowsRegedit();

            string line;
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedAutoStartBehaviour.ini"))
            {
               line = reader.ReadLine();
            }
            if (line == "AutoStart-yes") { autostart_checkBox.IsChecked = true; }
         }
         catch (FileNotFoundException)
         {
            //Ако текстовия файл го няма, изтриване и на автостартирането в "regedit"
            if (StartupPath.GetValue(programName.ToString()) != null)
            {
               StartupPath.DeleteValue(programName.ToString()); StartupPath.Close();
            }
         }
      }
      private void AutoStart_WindowsRegedit()
      {
         // Метод за записване в regedit на пътя на програмата за старт с Windows-a
         programName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
         curAssembly = Assembly.GetExecutingAssembly();
         regKeyFolder = Registry.CurrentUser;
         StartupPath = regKeyFolder.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
      }
      private void RemoveFromTaskbar_Behaviour()
      {
         try
         {
            string line;

            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedRemoveFromTaskbar.ini"))
            {
               line = reader.ReadLine();
            }
            if (line == "RemoveFromTaskbar-yes")
            {
               showInTaskbar_checkBox.IsChecked = true;
               mainWindow.ShowInTaskbar = false;
            }
            if (line == "RemoveFromTaskbar-no")
            {
               showInTaskbar_checkBox.IsChecked = false;
               mainWindow.ShowInTaskbar = true;
            }
         }
         catch (FileNotFoundException) { }
      }
      private void CompareIcon_Margins_InsertInScrollGrid(TextBlock iconMove, TextBlock iconTextMove)
      {
         //Изтриване на преместваната икона, за да не се бъркат
         int index = scrollGrid.Children.IndexOf(iconMove);
         if (index != -1)//Ако е = '1' значи се мести елемент, ако е = '-1' значи се дропва нов елемент
         {
            scrollGrid.Children.RemoveAt(index);
            scrollGrid.Children.RemoveAt(index);
         }

         for (int i = 0; i < scrollGrid.Children.Count; i += 2)//Сравнение на margin-ите на иконите и вмъкване на позиция
         {
            TextBlock compareIcon = (TextBlock)scrollGrid.Children[i];
            //TextBlock tempText = scrollGrid.Children[i + 1] as TextBlock;

            //вмъкване на елемент на избраната с мишката позиция
            //'i += 2' - Проверка - прескача TextBlock - а с текста, само преместваната икона
            if (iconMove.Margin.Left < compareIcon.Margin.Left && iconMove.Margin.Top < compareIcon.Margin.Top + 50)
            {
               scrollGrid.Children.Insert(i, iconMove);
               scrollGrid.Children.Insert(i + 1, iconTextMove);
               break;
            }
            else
            if (i + 2 == scrollGrid.Children.Count)//Поставя иконата най-накрая
            {
               scrollGrid.Children.Add(iconMove);
               scrollGrid.Children.Add(iconTextMove);
               return;
            }
         }
      }
      private void RearrangeElements_AfterDeleted()
      {
         //След като е изтрита икона се влиза в този метод
         //Първо създавам 'TextBlock'-иначе не се получава
         TextBlock someIconOrText;
         List<TextBlock> listIconsTemp = new List<TextBlock>();//За времеменно превърляне на иконите

         //Прехвърлят се в List<>, временно, изтриват се от scrollGrid
         for (int i = 0; i < scrollGrid.Children.Count; i++)
         {
            //'i'-то се намаля защото и 'çoint'-а на 'scrollGrid' се намаля като се изтриват елементи
            someIconOrText = scrollGrid.Children[i] as TextBlock;
            scrollGrid.Children.RemoveAt(i);
            listIconsTemp.Add(someIconOrText);
            i--;
         }
         //Пренареждане на иконите от временния лист с икони
         RearrangeElementsFromList(listIconsTemp);

         iconCounter--;
         labelCounter.Content = "Count: ";
         labelCounter.Content += iconCounter.ToString();
      }
      private void ReWriteMovedElements()
      {
         List<TextBlock> listIconsTemp = new List<TextBlock>();

         //Като се намери мястото на вмъкване, всички икони се прехвърлят в listIconsTemp и после пак в scrollGrid
         for (int i = 0; i < scrollGrid.Children.Count; i++)
         {
            listIconsTemp.Add(scrollGrid.Children[i] as TextBlock);
            scrollGrid.Children.RemoveAt(i);
            i--;
         }
         DeleteElementsInFile_StrWriter();

         //Презаписване само на иконите, без текста, във файла с икони
         bool check_IconOrText = true;
         foreach (var icons in listIconsTemp)
         {
            if (check_IconOrText == true)//Първо е икона в списъка,а тескта се пропуска със false-а
            {
               SavingElementInFile_StrWriter(icons.DataContext.ToString());
               check_IconOrText = false;
            }
            else { check_IconOrText = true; }
         }
         RearrangeElementsFromList(listIconsTemp); //Пренареждане на иконите от временния лист с икони и добавяне в scrollGrid
      }
      private void RearrangeElementsFromList(List<TextBlock> listElements)
      {
         bool iconOrText = true;
         marginTop_Icon = 15;
         marginLeft_Icon = 25;
         marginTop_IconText = 70;
         marginLeft_IconText = 15;

         for (int i = 0; i < listElements.Count; i++)
         {
            TextBlock someIconOrText = listElements[i] as TextBlock;
            listElements.RemoveAt(i);

            if (iconOrText == true)
            {
               //За подреждане на иконите
               someIconOrText.VerticalAlignment = VerticalAlignment.Top;
               someIconOrText.HorizontalAlignment = HorizontalAlignment.Left;
               someIconOrText.Margin = new Thickness(marginLeft_Icon, marginTop_Icon, 0, 0);
               iconOrText = false;
               marginLeft_Icon += 75;

               if (marginLeft_Icon > 1000) { marginLeft_Icon = 20; marginTop_Icon += 100; }
            }
            else
            {
               //За подреждане на текста под иконите
               someIconOrText.VerticalAlignment = VerticalAlignment.Top;
               someIconOrText.HorizontalAlignment = HorizontalAlignment.Left;
               someIconOrText.Margin = new Thickness(marginLeft_IconText, marginTop_IconText, 0, 0);
               iconOrText = true;
               marginLeft_IconText += 75;

               if (marginLeft_IconText > 1000) { marginLeft_IconText = 10; marginTop_IconText += 100; }
            }
            scrollGrid.Children.Add(someIconOrText);
            i--;
         }
      }
      private void SearchRegistry_Recursion(RegistryKey registrySubKeys)
      {
         if (registrySearchStop == true) { return; }

         string exe = "";
         string[] regArr = new string[0];
         RegistryKey regSubKeyTemp = null;

         try
         {
            regArr = registrySubKeys.GetSubKeyNames();
         }
         catch (Exception) { return; }

         foreach (string subSubKey in regArr)
         {
            try
            {
               regSubKeyTemp = registrySubKeys.OpenSubKey(subSubKey);

               string muiCache = Path.GetFileName(regSubKeyTemp.ToString());

               foreach (var newExePath in regSubKeyTemp.GetValueNames())
               {
                  try
                  {
                     exe = Path.GetExtension(newExePath);
                     if (exe == ".exe")
                     {
                        bool isElementDeleted = SearchForDeletedElements(newExePath);
                        if (isElementDeleted == false)//Проверка за изтрити елементи след предно търсене в регистъра
                        {
                           //Този метод помага едновременно(async) да се движи прозореца, да работи progress bar-a и да се създават елементи
                           Application.Current.Dispatcher.Invoke(delegate
                           {
                              CreateElements_DropSaveElements_Method(newExePath);
                           });
                        }
                     }
                  }
                  catch (Exception) { continue; }
               }
            }
            catch (Exception) { continue; }

            SearchRegistry_Recursion(regSubKeyTemp);
         }
      }
      private bool SearchForDeletedElements(string newExePath)
      {
         //Този метод се намира в метода 'SearchRegistry_Recursion()' и се използва за да не се взимат отново всички програми, ако някои излишни са били изтрити, след повторно търсене на прогр. в регистъра 

         string lineExePath = "";
         bool isElementDeleted = false;
         try
         {
            using (StreamReader reader = new StreamReader(CurrentExeDirectory() + "\\SavedDeletedElementsPath.ini"))
            {
               if (listDelElements_RegSearch.Count == 0)//Прехвърляне на изтритите пътища на програми в листа за по бързо търсене
               {
                  while ((lineExePath = reader.ReadLine()) != null)
                  {
                     listDelElements_RegSearch.Add(lineExePath);
                  }
               }
            }

            foreach (var exePath in listDelElements_RegSearch) //Сравнение на пътищата на прогромите
            {
               if (newExePath.Equals(exePath))
               {
                  isElementDeleted = true;
               }
            }
         }
         catch (FileNotFoundException) { isElementDeleted = false; }

         return isElementDeleted;
      }
      private static string CurrentExeDirectory()
      {
         //Целия път и името на .exe-то
         return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
      }
   }
}

///                                        ------ План ------
//1. Увеличаване когато мишката е върху панела ---------------------------------------------------------------> (Да)
//2. Намаляване когато мишката не е върху панела -------------------------------------------------------------> (Да)
//3. Добавяне на бутони --------------------------------------------------------------------------------------> (Да)
//4. Пускане на бутон в прозореца ----------------------------------------------------------------------------> (Да)
//5. Прихващане на пътя на програма --------------------------------------------------------------------------> (Да)
//6. Слагане на пътя на пр. към бутон ------------------------------------------------------------------------> (Да)
//7. Слагане име на бутон ------------------------------------------------------------------------------------> (Да)  
//8. Отваряне на програма --------------------------------Проблем с някои преки пътища -----------------------> (Да) 
//9. Да се появява инфо като ховърна върху бутона ------------------------------------------------------------> (Да)
//10. Записване пътя на програмите във файл за зареждане при отваряне прозореца на главната програма ---------> (Да)
//11. Подреждане на бутоните като на iPhone ------------------------------------------------------------------> (Да)
//12. Промяна на размера на буквите на бутона --------------------------------------------------------------> (НЕ)
//13. Отметка дали да се стартира с Windows-a ----------------------------------------------------------------> (Да)
//14. Изтриване на бутони ------------------------------------------------------------------------------------> (Да)
//15. Да взима иконата на съответната програма и да я слага на бутона ----------------------------------------> (Да)
//16. Увеличаване размера на бутона когато мишката е върху него ----------------------------------------------> (Да)
//17. Контекст меню ------------------------------------------------------------------------------------------> (Да)
//18. Има проблем с отварянато на пряк път на програми -------------------------------------------------------> (Да)
//19. Проверки - try/catch -----------------------------------------------------------------------------------> (Да)
//20. Да могат да се местят иконите --------------------------------------------------------------------------> (Да)
//21. Изтриване на икони -------------------------------------------------------------------------------------> (Да)
//22. Презаписване на останалите икони -----------------------------------------------------------------------> (Да)
//23. Добавяне на разширения за избор на фон и икона на папките ico, jpg, bmp --------------------------------> (Да)
//24. Прозореца да може да се мести с натискане върху 'buttonOpenClose' бутона -------------------------------> (Да)
//25. Търсачка -----------------------------------------------------------------------------------------------> (Да)
//26. Търсене на програми в регистрите(има рекурсия и async метод) -------------------------------------------> (Да)
//27. Записване на изтрити елементи --------------------------------------------------------------------------> (Да)