   M S B A M L   `   `   `  ����J  F2D_Graphics_Car, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:GraphicsCarGraphicsCar  5   	   M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089=+clr-namespace:System.Timers;assembly=SystemSystem.Timers 5   	   i eMicrosoft.VisualStudio.Shell.14.0, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�cclr-namespace:Microsoft.Internal.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.14.0*Microsoft.Internal.VisualStudio.PlatformUI 6�   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Q6clr-namespace:System.Reflection.Emit;assembly=mscorlibSystem.Reflection.Emit 6�       GraphicsCar.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:GraphicsCar     5   	   >Timers+clr-namespace:System.Timers;assembly=System     5   	   c	 _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing 	 6D   t
PlatformUIcclr-namespace:Microsoft.Internal.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.14.0 	 6�   AEmit6clr-namespace:System.Reflection.Emit;assembly=mscorlib 	 6�   -   5        $	��706��5   _   $	��960��6k     � WindowStartupLocation$  CenterScreen=�6x    � WindowStyle$
 None=�6�    � 
ResizeMode$ NoResize=�6�    � Title$ 2D Graphics Car��6�   .��5	      �  J� RenderTransformOrigin$	 0,0&�6   ���	#FF878787$
��Auto=�6>   $	��960��6p   $��
0,1,0,-456q�6}   $	��Top=�6�   .+�5   
   �� -   5         ��Name$ line9��5      $
��Left=�6   $��437,1064,0,0q�6:   $��8��6P   $ ��Resources/Line - White.jpg��6Z   $��UniformToFill=�6~   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line4��5      $
��Left=�6   $��260,1064,0,0q�6:   $��8��6P   $ ��Resources/Line - White.jpg��6Z   $��UniformToFill=�6~   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line0��5      $
��Left=�6   $��	260,0,0,0q�6:   $��8��6M   $ ��Resources/Line - White.jpg��6W   $��UniformToFill=�6{   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line1��5      $
��Left=�6   $��260,266,0,0q�6:   $��8��6O   $ ��Resources/Line - White.jpg��6Y   $��UniformToFill=�6}   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line2��5      $
��Left=�6   $��260,532,0,0q�6:   $��8��6O   $ ��Resources/Line - White.jpg��6Y   $��UniformToFill=�6}   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line3��5      $
��Left=�6   $��260,798,0,-3q�6:   $��8��6P   $ ��Resources/Line - White.jpg��6Z   $��UniformToFill=�6~   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line5��5      $
��Left=�6   $��	437,0,0,0q�6:   $��8��6M   $ ��Resources/Line - White.jpg��6W   $��UniformToFill=�6{   $	��166��6�   $	��Top=�6�   �� 5   
   -	   5        $ line6��5      $
��Left=�6   $��437,266,0,0q�6:   $��8��6O   $ ��Resources/Line - White.jpg��6Y   $��UniformToFill=�6}   $	��166��6�   $	��Top=�6�   �� 5   
   -
   5        $ line7��5      $
��Left=�6   $��437,532,0,0q�6:   $��8��6O   $ ��Resources/Line - White.jpg��6Y   $��UniformToFill=�6}   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ line8��5      $
��Left=�6   $��437,798,0,-3q�6:   $��8��6P   $ ��Resources/Line - White.jpg��6Z   $��UniformToFill=�6~   $	��166��6�   $	��Top=�6�   �� 5   
   -   5        $ 
speedBump1��5      $
��Left=�6$   $��60��6?   $��	360,5,0,0q�6K   $	��Top=�6^   $	��233��6v   $#��Resources/LayingPolicemen.png��6�   $
��Fill=�6�   �� 5   
   -   5        $ 
speedBump2��5      $
��Left=�6$   $��60��6?   $��105,865,0,0q�6K   $	��Top=�6`   $	��237��6x   $#��Resources/LayingPolicemen.png��6�   $
��Fill=�6�   �� 5!   
   -   5        $ linePavL��5!      $��8��6"   $
��Left=�6,   $��	100,0,0,0q�6G   $ ��Resources/Line - White.jpg��6Z   $
��Fill=�6~   $
��1000��6�    J� Effect 5"      3  -System.Windows.Media.Effects.DropShadowEffect  5#         Color$ 	#FF7C7C7C��6#   5%      �� 5&   
   -   5        $ linePavR��5&      $��8��6"   $ ��Resources/Line - White.jpg��6,   $	��Top=�6P   $
��Left=�6h   $��	598,0,0,0q�6�   $
��1000��6�   $
��Fill=�6�    5'        5(         
BlurRadius$ 20Q�6#   	   ShadowDepth$	 20Q�63   5*      �� 5+   
   -   5        $ 	pavement4��5+      $
��Left=�6#   $	��495��6>   $��606,495,0,-30q�6K   $	��Top=�6b   $	��100��6z   $
��Fill=�6�   $��Resources\pavement.jpg��6�   �� 5-   
   -   5        $ 	pavement3��5-      $	��495��6#   $��	606,0,0,0q�60   $	��Top=�6C   $
��Fill=�6[   $��Resources\pavement.jpg��6j   $
��Left=�6�   $	��100��6�   �� 5/   
   -   5        $ 	pavement5��5/      $
��Left=�6#   $	��100��6>   $
��Fill=�6J   $��Resources\pavement.jpg��6Y   $	��495��6y   $	��Top=�6�   $��606,990,0,0q�6�   �� 50   
   -   5        $ 	pavement2��50      $
��Left=�6#   $��	0,990,0,0q�6>   $	��100��6Q   $
��Fill=�6]   $��Resources\pavement.jpg��6l   $	��495��6�   $	��Top=�6�   �� 51   
   -   5        $ 	pavement1��51      $
��Left=�6#   $	��495��6>   $��0,495,0,-30q�6K   $	��Top=�6`   $	��100��6x   $
��Fill=�6�   $��Resources\pavement.jpg��6�   �� 52   
   -   5        $ 	pavement0��52      $
��Left=�6#   $	��495��6>   $	��Top=�6K   $	��100��6c   $
��Fill=�6o   $��Resources\pavement.jpg��6~   �� 53   
   -   5        $ UserCar��53      $
��Left=�6!   $	��180��6<   $��313,677,0,0q�6I   $	��Top=�6^   $��88��6v   $��Resources\RedCar.png��6�   $ 0.5,0.35&�6�    54        55      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   57      �� 58   
   -   5        $ 	CarEnemy1��58      $
��Left=�6#   $	��175��6>   $��145,-262,0,0q�6K   $	��Top=�6a   $��87��6y   $��Resources\YellowCar.png��6�   $ 0.467,0.375&�6�    59        5:      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   5<      �� 5=   
   -   5        $ 	CarEnemy2��5=      $
��Left=�6#   $	��175��6>   $��313,-52,0,0q�6K   $	��Top=�6`   $��87��6x   $��Resources\Polize Car.png��6�   $ 0.467,0.375&�6�    5>        5?      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   5A      �� 5B   
   -   5        $ 	CarEnemy3��5B      $
��Left=�6#   $	��175��6>   $��145,468,0,0q�6K   $	��Top=�6`   $��87��6x   $��Resources/Bugatti.png��6�   $ 0.467,0.375&�6�    5C        5D      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   5F      �� 5G   
   -   5        $ Bicycle��5G      $
��Left=�6!   $	��112��6<   $��651,1144,0,0q�6I   $	��Top=�6_   $��55��6w   $ 0.455,0.491&�6�   $��Resources\Bicycle.png��6�    5H        5I      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   5K      �� 5L   
   -   5        $ splashEnemy��5L      $
��Left=�6%   $	��115��6@   $	��Top=�6M   $	��130��6e   $��Resources\FireSplash.png��6q   $��800,495,0,0q�6�   �� 5M   
   -   5        $ 	CarEnemy4��5M      $
��Left=�6#   $	��175��6>   $��473,239,0,0q�6K   $	��Top=�6`   $��87��6x   $��Resources/Taxi.png��6�   $ 0.467,0.375&�6�    5N        5O      $ 15Q�6#   $	 15Q�63   $ 	#FF7C7C7C��6D   5Q      �� 5R   
   -   5        $ tree3��5R      $
��Left=�6   $	��165��6:   $��530,-170,0,0q�6G   $	��Top=�6]   $	��160��6u   $��Resources\tree3.png��6�   $ 0.5,0.5&�6�   z�5S      V� 5T      .~�5U      �� �� 5V      
 �� AngleX$
 0.258Q�6$   �� 5W      T� 5X       T� X$ 0.495Q�6)   5Y       5[        5\      $	 40Q�6#   $ 30Q�64   $ 	#FF7C7C7C��6D   5^      �� 5_   
   -   5        $ tree1��5_      $
��Left=�6   $	��165��6:   $��	0,-34,0,0q�6G   $	��Top=�6Z   $	��165��6r   $��Resources\tree1.png��6~   $ 0.5,0.5&�6�   z�5`      V� 5a      .~�5b      �� �� 5c      $
 0.258Q�6$   �� 5d      T� 5e      $ 0.495Q�6)   5f       5h        5i      $	 40Q�6#   $ 30Q�64   $ 	#FF7C7C7C��6D   5k      �� 5l   
   -   5        $ tree4��5l      $
��Left=�6   $	��165��6:   $��530,597,-2,0q�6G   $	��Top=�6]   $	��180��6u   $��Resources\tree2.png��6�   $ 0.5,0.5&�6�   z�5m      V� 5n      .~�5o      �� �� 5p      $
 0.258Q�6$   �� 5q       �� Angle$ -0.219Q�6&   T� 5r      $ 0.495Q�6)   5s       5u        5v      $	 40Q�6#   $ 30Q�64   $ 	#FF7C7C7C��6D   5x      �� 5y   
   -    5        $ tree2��5y      $
��Left=�6   $	��165��6:   $��	0,660,0,0q�6G   $	��Top=�6Z   $	��170��6r   $��Resources\tree4.png��6~   $ 0.5,0.5&�6�   z�5z      V� 5{      .~�5|      �� �� 5}      $
 0.258Q�6$   �� 5~      T� 5      $ 0.495Q�6)   5�       5�        5�      $	 40Q�6#   $ 30Q�64   $ 	#FF7C7C7C��6D   5�      �� 5�   
   -!   5        $
 Bird��5�      $
��Left=�6   $��25��69   $��2235,2300,0,0q�6E   $	��Top=�6\   $��40��6t   $ 0.455,0.491&�6   $��Resources\birg.png��6�   z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�      $ -30.864Q�6&   T� 5�      5�       5�        5�      $ 25Q�6#   $	 70Q�63   $ 	#FF7C7C7C��6D   5�      �� 5�   
   -"   5        $ splash��5�      $
��Left=�6    $	��105��6;   $	��Top=�6H   $	��110��6`   $��Resources\splash.png��6l   $��800,285,0,0q�6�   9� 5�   
   -#   5         9�Name$ expanderInfo��5�      $
��Info��6)   $
��Left=�67   $��
567,13,0,0q�6R   $	��Top=�6f   $	��113��6~   $	��136��6�   $ 0.5,1&�6�   $��18(�6�   ���	#FF10F784&   System.Windows.Media.TextOptions   TextHintingMode$ Animated=�6�    9� 
IsExpanded . $��RightToLeft=�6  $��SemiBold"�6/   5�        5�      $ 4Q�6#   $	 3Q�62   z�5�      V� 5�      .~�5�      ��  �� ScaleY$ 1Q�6%    �� ScaleX$ 1Q�60   �� 5�       �� AngleY$ 0Q�6$   $
 0Q�6/   �� 5�      $ 0Q�6&   T� 5�      5�      .��5�      �� -$   5         ��Name$ labelInfoExit��5�      $0��*'R' repair car
'P' for Pause
'E' for Exit ��6+   $
��Left=�6h   $��92��6�   $	��132��6�   $��20(�6�   ���	#FFDCFF00 J� IsHitTestVisible . $��2q�6�   $ 0.505,0.624&�6�   $��-4,0,0,0q�6B  $��LeftToRight=�6T  ��6�   Z�  5�        5�      $ 	#FF787878��6'   $ 10Q�69   $	 4Q�6I      RenderingBias$ Quality=�6Y   5�      5�      A� 5�   
   $
��Left=�6   $	��120��6-   $��461,828,0,0q�6:   ���	#FFF4FF86$	��Top=�6b   $	��120��6z   $��5��6�    J� Opacity$	 0.9Q�6�    5�        5�      $ 12Q�6#   ��5�      �� 5�       �� Color$ 	#FF474747��6"    �� Opacity$	 0.6Q�64   5�      A� 5�   
   -%   5         A�Name$ center��5�      $
��Left=�6"   $��10��6=   $��516,883,0,0q�6I   ���	#FF151515$	��Top=�6q   $��10��6�   $��10��6�   $	 0.9Q�6�    5�        5�      $	 1Q�6#   $ 	#FF00F3FF��63   $ 3Q�6E   ��5�      �� 5�      $ 	#FF191919��6"   $	 0.6Q�64   5�      �� 5�   
   -&   5        $
 _140��5�      $	��140��6   $��514,833,179.036,0q�6,   $	��Top=�6G   $��10��6_   $��8(�6k   ���	#FFFFDE88$ 0.5,0.5&�6�   $��0q�6�   $��13��6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -'   5        $ _0��5�      $��0��6   $��487,920,215,0q�6(   $	��Top=�6?   $��10��6W   $��8(�6c   $ 0.5,0.5&�6p   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -(   5        $
 _300��5�      $	��300��6   $��531,927,162,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   $ 0.5,0.5&�6t   $��0q�6�   
���  �� 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -)   5        $	 _60��5�      $��60��6   $��469,874,228,0q�6*   $	��Top=�6A   $��9��6Y   $��8(�6d   $ 0.5,0.5&�6q   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -*   5        $
 _220��5�      $	��220��6   $��561,875,132,0q�6,   $	��Top=�6C   $��11��6[   $��8(�6g   ���	#FFFF5151$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -+   5        $
 _100��5�      $	��100��6   $��484,847,210,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   $ 0.5,0.5&�6t   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -,   5        $
 _200��5�      $	��200��6   $��556,860,137,0q�6,   $	��Top=�6C   $��9��6[   $��8(�6f   ���	#FFFF5151$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   --   5        $	 _20��5�      $��20��6   $��476,906,221,0q�6*   $	��Top=�6A   $��11��6Y   $��8(�6e   $ 0.5,0.5&�6r   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -.   5        $	 _40��5�      $��40��6   $��470,891,227,0q�6*   $	��Top=�6A   $��11��6Y   $��8(�6e   $ 0.5,0.5&�6r   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -/   5        $	 _80��5�      $��80��6   $��475,859,222,0q�6*   $	��Top=�6A   $��10��6Y   $��8(�6e   $ 0.5,0.5&�6r   $��0q�6�   ���	#FFC6FF2E 5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -0   5        $
 _120��5�      $	��120��6   $��498,837,194,0q�6,   $	��Top=�6C   $��11��6[   $��8(�6g   ���	#FFFFDE88$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -1   5        $
 _160��5�      $	��160��6   $��532,837,161,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   ���	#FFFFDE88$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -2   5        $
 _180��5�      $	��180��6   $��546,847,147,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   ���	#FFFFDE88$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -3   5        $
 _240��5�      $	��240��6   $��560,890,132,0q�6,   $	��Top=�6C   $��11��6[   $��8(�6g   ���	#FFFF5151$ 0.5,0.5&�6�   $��0q�6�    5�        5�      $		 0.2Q�6#   $ 1Q�65   5�      �� 5�   
   -4   5        $
 _280��5�      $	��280��6   $��545,918,148,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   $ 0.5,0.5&�6t   $��0q�6�   
���  �� 5�        5�      $		 0.2Q�6#   $ 1Q�65   5      �� 5  
   -5   5        $
 _260��5     $	��260��6   $��555,905,138,0q�6,   $	��Top=�6C   $��10��6[   $��8(�6g   $ 0.5,0.5&�6t   $��0q�6�   
���  �� 5       5     $		 0.2Q�6#   $ 1Q�65   5     r� 5  
   -6   5         r�Name$ carEnemyCrashMedia��5     $
��Left=�63   $	��100��6N   $��750,16,-142,0q�6[   $	��Top=�6r   $	��100��6�    r� Volume$ 80Q�6�    r� LoadedBehavior$ Manual=�6�   r� 5	  
   -7   5        $ userCarCrashMedia��5	     $
��Left=�62   $	��100��6M   $��865,16,-257,0q�6Z   $	��Top=�6q   $	��100��6�   $ 80Q�6�   $ Manual=�6�    r� Source$ CarBrakeCrash.wav?�6�   r� 5  
   -8   5        $ pushEnemyCarsMedia��5     $
��Left=�63   $	��100��6N   $��750,130,-142,0q�6[   $	��Top=�6s   $	��100��6�   $ 80Q�6�   $ Manual=�6�   r� 5  
   -9   5        $ userCarDrivingMedia��5     $
��Left=�64   $	��100��6O   $��865,130,-257,0q�6\   $	��Top=�6t   $	��100��6�   $ 50Q�6�   �� 5  
   -:   5        $ 
pauseLabel��5     $��Pause��6$   $��328,161,328,780q�64   $��Center=�6M   $ 0.5,0.5&�6h   $��28 Days Later)�6�   $��20(�6�   $ Animated=�6�   ���	#FF62FF08���	#FFFF9D9D$��19��6  $��0q�6)  ��6  Z�  5       5     $ 10Q�6#   $ 	#FF7E7E7E��63   5     E  ?Microsoft.Internal.VisualStudio.PlatformUI.SpinAnimationControl  5  
   $
��Left=�6*   $��58��6E   $��490,859,0,0q�6Q   $	��Top=�6f   $��62��6~   $ 0.5,0.5&�6�    J� OpacityMask �	#FF767171z�5     V� 5     .~�5     �� $ -1Q�6%   $ -1Q�61   �� 5     $ 0Q�6$   $
 0Q�6/   �� 5     $ 0Q�6&   T� 5     5     5     �� 5  
   -;   5        $ kmLabel��5     $��00001��6!   $
��Left=�61   $��506,867,0,0q�6L   $	��Top=�6a   $ 0.5,0.5&�6y   $��15��6�   $��0q�6�   $��33��6�   ���	#FFB7E420$ Animated=�6�    . $��RightToLeft=�6(  ��6�   Z�  5        5!     $ 1Q�6#   $		 0.2Q�62   $ 	#FF060606��6D   z�5#     V� 5$     .~�5%     �� $ 1Q�6%   $ 1Q�60   �� 5&     $ 0Q�6$   $
 0Q�6/   �� 5'     $ 0Q�6&   T� 5(     5)     5+     �� 5,  
   -<   5        $ arrow��5,     $
��Left=�6   $��38��6:   $��518,850,0,0q�6F   $	��Top=�6[   $��6��6s   $��Resources/Arrow.png��6}   $
��Fill=�6�   $	 0.9Q�6�   $ 0.5,1&�6�    �	#FF060606 5-       5.     $	 1Q�6#   $ 	#FF18FFEA��63      	Direction$	 150Q�6E   $ 10Q�6U   z�50     V� 51     .~�52     �� �� 53     �� 54     $	 220Q�6&   T� 55     56     58     59     5:     