   M S B A M L   `   `   `  ����  	Bekerites    Bekerites.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Bekerit játék��5   	   $	��450��6   $	��400��6,   $	��450��68   $	��400��6H   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   .@�5      m� $��Fájl��6   .@�5      m� $��Új játék��6   .@�5      m� $��Kicsi pálya��6    m� Command 64   �� *NewGameSmallCommand+m� 5      $��Közepes pálya��6    66   �� *NewGameMediumCommand+m� 5      $��Nagy pálya��6    63   �� *NewGameBigCommand+5      �� 5      m� 5      $��Játék betöltése...��6    67   �� *LoadGameOpenCommand+m� 5      $��Játék mentése...��6    65   �� *SaveGameOpenCommand+{�6]   �� *
EnableSave+�� 5      m� 5      $��	Kilépés��6    6,   �� *ExitCommand+5      5      �� 5   
   $��1��6   ��6$   �� *Fields+��5      �� 5       .R�5"      A�  A� Rows 6"   �� *Size+ A� Columns 68   �� *Size+5#      ��5%      �� 5'      .R�5(      �� |�.   J� RenderTransformOrigin$ 0.5, 0.5&�6s   $��24(�6�   $
��Bold"�6�   ��6   �� *StepCommand+��6=   �� *Number+z�5+      �� 5,       �� ScaleX$ 1Q�6-    �� ScaleY$ 1Q�68   ��5.      �� 50       �� 
TargetType" ��6$   	 �� Triggers	 51   "   �� 54   &   
 �� Binding
 62   �� *PlayerColor+ �� Value Gray6R   .a�55   *   ��  �� Property � �� �� Value
 �����6G   56   '   �� 58   &   
 62   �� *PlayerColor+ 	LightBlue6R   .a�59   *   ��  � ��
 ��ح�6G   5:   '   �� 5<   &   
 62   �� *PlayerColor+ LightRed6R   .a�5=   *   ��  � ��
 �����6G   5>   '   �� 5A   &   
 62   �� *PlayerColor+ 
PlayerBlue6R   .a�5B   *   ��  � ��
 ��  �6G   5C   '   �� 5D   &   
 62   �� *PlayerColor+ NoPlayer6R   .a�5E   *   ��  � ��
 �����6G   5F   '   �� 5H   &   
 62   �� *PlayerColor+ 	PlayerRed6R   .a�5I   *   ��  � ��
 �  ��6G   5J   '   5N       � Triggers 5P      :� 5R       :� RoutedEvent$ Button.Loaded��6+   .[�5S   "   �� .��5T   &   �� .��5U   *   W�  g� Duration$ 0:00:02D�6:    W� From$ 0X�6M   	 W� To$ 1X�6V    �� TargetProperty$ Opacity�6]   5V   '   5W   #   5X      5[      5\      ��5^      �� 5`      .�5a      ��  � �� 61   �� *X+�� 5b       � �� 64   �� *Y+5c      5e      �� 5f   
   $��2��6   .@�5h      �� .��5i      �� $��Kék: ��6   5j      �� 5k      .��5l      �� ��6   �� *	BlueScore+5m      �� 5n      �� 5o      .��5p      �� $��Piros: ��6   5q      �� 5r      .��5s      �� ��6   �� *
RedScore+5t      �� 5u      �� 5v      5w      5x      5y      