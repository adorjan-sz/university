   M S B A M L   `   `   `  ����C  ?RubikWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:RubikWPFRubikWPF  5   	       RubikWPF.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:RubikWPF     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   .+�5   
   �� $��0��6   ��6$   �� *Fields+��5      �� 5      .R�5      A�  A� Rows 6"   �� *Size+ A� Columns 68   �� *Size+5      ��5      �� 5      .R�5      �� $|�False��6v    J� RenderTransformOrigin$ 0.5, 0.5&�6�   $��24(�6�   $
��Bold"�6�   ��6   �� *StepCommand+��6=   �� *XY+��6]   �� *Text+z�5      �� 5        �� ScaleX$ 1Q�6-    �� ScaleY$ 1Q�68   ��5"      �� 5$       �� 
TargetType" ��6$    �� Triggers 5%   "   �� 5'   &   	 �� Binding	 62   �� *colour+
 �� Value
 06M   .a�5(   *   ��  �� Property � �� �� Value
 �����6G   5)   '   �� 5*   &   	 62   �� *colour+
 16M   .a�5+   *   ��  � ��
 �  ��6G   5,   '   �� 5-   &   	 62   �� *colour+
 26M   .a�5.   *   ��  � ��
 � ���6G   5/   '   �� 50   &   	 62   �� *colour+
 36M   .a�51   *   ��  � ��
 ��  �6G   52   '   �� 53   &   	 62   �� *colour+
 46M   .a�54   *   ��  � ��
 � � �6G   55   '   57      5:      5;      5=      5?      5@      