   M S B A M L   `   `   `  ���� F  BVenn.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Venn.Client.MVVM.ViewModelsVenn.Client.MVVM.ViewModels  ?$clr-namespace:Venn.Client.MVVM.ViewsVenn.Client.MVVM.Views  2    ,Venn.Client.MVVM.Views.EmailVerificationView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ?	viewmodel)clr-namespace:Venn.Client.MVVM.ViewModels     _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  0local$clr-namespace:Venn.Client.MVVM.Views  ���#E7ECEF.��� .+��� +  %MaterialDesignThemes.Wpf.ButtonAssist    CornerRadius$  10��$
��Left=�$	��Top=����#E7ECEF���#274C77$��0q�$��0q�$��
20 20 0 30q�$��46��$��46������ *ToBackViewCommand+.��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ ChevronLeft=�$��40��$��40���� $��Center=�$��Center=� �� Orientation$ Vertical=�.+��� $��89��$��89��$&�� /MVVM/Views/Images/venn_logo.png���� $��Email Vertification��$��Center=�   MainFont#��[  $
��Bold"�$��48(�$��0 0 0 20q����#274C77�� $��0 0 0 20q�$��4q����#274C77 �� CornerRadius$ 10��$	��725��$	��114�����#6096BA._��� $��5q�$
��Wrap=�$��Center=�$��Center=����#E7ECEF$��17(�$��Microsoft YaHei UI)�$����Thank you for providing your email address for verification. As part of our security measures, we require a 6-digit verification code to ensure the validity and ownership of the provided email address.���� $	��520��$��60 0 0 0q�$ 
Horizontal=�.+��� $ 
Horizontal=�$	��460��$��	0 20 0 45q�.+��� -   $��Digit1�� ! MaterialDesignOutlinedTextBox#��[ $
��10 0q�$��0 0 3 0q�.  (MaterialDesignThemes.Wpf.TextFieldAssist   TextFieldCornerRadius$ 10��$��Center=�$��Center=� �� 	MaxLength$ 1��$��1q����#6096BA
������ )  #MaterialDesignThemes.Wpf.HintAssist   
Foreground �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit1+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� -   $��Digit2��#��[ $
��10 0q�$��0 0 3 0q�$ 10��$��Center=�$��Center=�$ 1��$��1q����#6096BA
������  �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit2+$ PropertyChanged=�$ TwoWay=��� -   $��Digit3��#��[ $
��10 0q�$��0 0 3 0q�$ 10��$��Center=�$��Center=�$ 1��$��1q����#6096BA
������  �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit3+$ PropertyChanged=�$ TwoWay=��� $	��5 0q�$��30��$��7�����#274C77$ 3���� -   $��Digit4��#��[ $
��10 0q�$��0 0 3 0q�$ 10��$��Center=�$��Center=�$ 1��$��1q����#6096BA
������  �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit4+$ PropertyChanged=�$ TwoWay=��� -   $��Digit5��#��[ $
��10 0q�$��0 0 3 0q�$ 10��$��Center=�$��Center=�$ 1��$��1q����#6096BA
������  �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit5+$ PropertyChanged=�$ TwoWay=��� -   $��Digit6��#��[ $
��10 0q�$��0 0 3 0q�$ 10��$��Center=�$��Center=�$ 1��$��1q����#6096BA
������  �#6096BA$��50��$��50��$��Microsoft YaHei UI)�$
��Bold"�$��16(����#6096BA���� *Digit6+$ PropertyChanged=�$ TwoWay=��� 
������ $��0q�$��40��$��40��$��0q�$��20, 0, 0, 20q����#274C77���� *ResendMailCommand+.��  $ EmailSyncOutline=�$��40��$��40���� -   $��VerifyButton��${�False��$  20��+  %MaterialDesignThemes.Wpf.ShadowAssist	   ShadowDepth$	 Depth2=�$��Verify��$��Microsoft YaHei UI)�$��14(�$��10q�$��50 10q�$	��401��$��40�����#E7ECEF���#274C77$��0q����� *VerifyCommand+�� $��Center=�$��Center=�
���  ��
 J� Opacity$	
 0.4Q�$��12(�$��Microsoft YaHei UI)�$��Please try again.��y��� *ErrorTextVisibility+$ PropertyChanged=�$ TwoWay=�