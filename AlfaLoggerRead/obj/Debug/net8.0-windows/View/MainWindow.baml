   M S B A M L   `   `   `  ����I  EAlfaLoggerRead, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:LoggerReaderLoggerReader  5   	   A%clr-namespace:LoggerReader.ViewModelsLoggerReader.ViewModels  5   	       LoggerReader.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:LoggerReader     5   	   4vm%clr-namespace:LoggerReader.ViewModels     5   	   $	��450��5   	   $	��800��5   	     � Title  5
   	   �� *Title+.��5      � $��0,0,0,-6q�6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��1��5      $��0��5      $��3��5      .��5      &   System.Windows.Controls.DataGrid  ��5      �� *Logs+5       5"      �� 5(   
   $��Bottom=�5)      $
��Left=�5*      $��10q�5+       �� Orientation$ Vertical=�5,      $��0��5-      $��2��5.      .+�5/      �� ��6   �� *BackCommand+.��65   
Назад51      52      �� 54   
   $��Bottom=�55      $��Right=�56      $��10q�57      $ Vertical=�58      $��2��59      $��2��5:      .+�5;      �� ��6   �� *NextCommand+.��65   
Далее5=      5>      �� 5@   
   $��Bottom=�5A      $��Center=�5B      $��10q�5C      $ 
Horizontal=�5D      $��1��5E      $��2��5F      .+�5G      �� $��Страница №��6   �� 5H      ��6   �� *	NumPage+�� 5I      $��Пагинация��6   �� 5J      $��40��5K      ��5J      �� *
SizePage+5K      �� 5L      ��6   �� *RefreshCommand+.��68   Обновить5N      5O      5U      5V      