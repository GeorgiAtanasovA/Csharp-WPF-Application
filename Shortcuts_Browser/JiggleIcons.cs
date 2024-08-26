using System.Windows.Controls;
using System.Windows.Media;

namespace ShortcutBrowser
{
   partial class MainWindow
   {
      int anglePlusRight = 0;
      int angleMinusRight = 4;
      double iconsRotateRight = 0;

      int anglePlusLeft = -4;
      int angleMinusLeft = 0;
      double iconsRotateLeft = 0;

      public void JiggleIconsWhileMove()
      {
         if (anglePlusRight < 4) { anglePlusRight++; iconsRotateRight = anglePlusRight; }
         else if (angleMinusRight > -4) { angleMinusRight--; iconsRotateRight = angleMinusRight; }
         else { anglePlusRight = 0; angleMinusRight = 4; }

         for (int i = 2; i < scrollGrid.Children.Count - 1;)
         {
            ((TextBlock)scrollGrid.Children[i]).LayoutTransform = new RotateTransform(iconsRotateRight);
            i = i + 4;
         }


         if (angleMinusLeft > -4) { angleMinusLeft--; iconsRotateLeft = angleMinusLeft; }
         else if (anglePlusLeft < 4) { anglePlusLeft++; iconsRotateLeft = anglePlusLeft; }
         else { anglePlusLeft = -4; angleMinusLeft = 4; }

         for (int i = 0; i < scrollGrid.Children.Count - 1;)
         {
            ((TextBlock)scrollGrid.Children[i]).LayoutTransform = new RotateTransform(iconsRotateLeft);
            i = i + 4;
         }
      }
   }
}
