   M S B A M L   `   `   `  ����D  @Bekerites, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Bekerites.ViewBekerites.View  5   	       Bekerites.View.LoadWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Bekerites.View     5   	     � Title$  Jaték betöltése��5   	   $	��300��6!   $	��450��6.    � 
ResizeMode$ NoResize=�6:   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ��6   �� *Games+ �� SelectedItem 6=   ��  �� Path$ SelectedGame� �� Mode$ TwoWay=���5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� ��6$   �� *Name+�� 5      $��10,0,0,0q�6d   ��6$   �� $
 Time� �� StringFormat$ yyyy. MM. dd. HH:mm:ss��6d   5      5      5      �� 5   
   $��1��6   $ 
Horizontal=�6"   $��RightToLeft=�6;   .+�5      �� $��75��6>   $��
Betöltés��6I   $��0,0,10,0q�6\   ��6   �� *LoadGameCloseCommand+6\   5      5      5      