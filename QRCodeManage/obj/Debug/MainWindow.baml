   M S B A M L   `   `   `  ����  QRCodeManage    QRCodeManage.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  二维码生成工具��5   	   $	��350��6   $	��525��6&   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5      $��* �6   6)   �� 5	      $��* �6   6)   .+�5   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   .+�5      �� -   5        $��0��5      $��80��6$   $��创建二维码��62   6B   �� 5      -   5        $��0��5      $��1��6$   $��80��64   $��查看二维码��6B   6R   5      �� 5   
   -   5        $��PrintCantent��5      $��1��6   $
��auto��63    �� TextWrapping$
 Wrap=�6A    �� AcceptsReturn . " �� VerticalScrollBarVisibility$ Visible=�6j   6�   �� 5   
   -   5        $��LogBox��5      $��2��6   $
��auto��6-   $
 Wrap=�6;    . $ Visible=�6d   6�   5      5      