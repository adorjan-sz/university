   M S B A M L   `   `   `  ����H  DGameOfLifeWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:GameOfLifeWpfGameOfLifeWpf  5   	       GameOfLifeWpf.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:GameOfLifeWpf     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   .@�5      m� $
��Time��6   .@�5      m� $��Indit/Szünet��6    m� Command 61   �� *PauseGameCommand+5      5      �� 5   
   $��1��6   ��6$   �� *Fields+��5      �� 5      .R�5      A�  A� Rows$ 9��6"    A� Columns$ 9��6+   5      ��5      �� 5!      .R�5"      �� $|�False��6^    J� RenderTransformOrigin$ 0.5, 0.5&�6p   $��24(�6�   $
��Bold"�6�   ��6   �� *StepCommand+��6=   �� *XY+z�5%      �� 5&       �� ScaleX$ 1Q�6-    �� ScaleY$ 1Q�68   ��5(      �� 5*       �� 
TargetType" ��6$   	 �� Triggers	 5+   "   �� 5-   &   
 �� Binding
 62   �� *	IsAlive+ �� Value True6N   .a�5.   *   ��  �� Property � �� �� Value
 � ���6G   5/   '   �� 50   &   
 62   �� *	IsAlive+	 False6N   .a�51   *   ��  � ��
 �����6G   52   '   54      57      58      5:      �� 5;   
   $��2��6   5=      5>      5?      