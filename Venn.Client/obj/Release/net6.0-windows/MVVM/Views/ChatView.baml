   M S B A M L   `   `   `  ���� F  BVenn.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Venn.Client.MVVM.ViewModelsVenn.Client.MVVM.ViewModels  ?$clr-namespace:Venn.Client.MVVM.ViewsVenn.Client.MVVM.Views  %    Venn.Client.MVVM.Views.ChatView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ?	viewmodel)clr-namespace:Venn.Client.MVVM.ViewModels     _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  0local$clr-namespace:Venn.Client.MVVM.Views  ���#36393F.��� .+�)  #MaterialDesignThemes.Wpf.DrawerHost      RightDrawerContent  � ���#2F3136.+��� ���#B13333$��1q�+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth3=����#292b2f �� CornerRadius$ 25, 25, 0, 0��$��Stretch=�$��0, 100, 0, 0q���  �� Orientation$ Vertical=�$	��400��.+�� .+�A� $��0, 50, 0, 0q�$	��102��$	��102�����#B13333A� $��0, 50, 0, 0q�$	��100��$	��100������  �� ImageSource �� *#!SelectedContact.User2.ImageSource+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� 
�������$��20(�$��SemiBold"�$��Center=�$��5q����� * SelectedContact.User2.Username+$ PropertyChanged=�$ TwoWay=��� ���#B13333$��15(�$��SemiBold"�$��Center=����� *SelectedContact.User2.Email+$ PropertyChanged=�$ TwoWay=�.���  � ColumnDefinitions �� $	��200 ��� .+�� ���#2F3136 � RowDefinitions �� $��50 ��� �� $��60 �.+��� $��Contacts��$��Center=�$��Medium"�
�������$��
8, 0, 0, 0q��� 
������ $��0q�$��1��   ContactCard#��[  ���� *User.Contacts+$ PropertyChanged=�$ TwoWay=�	 �� SelectedItem	 �� *SelectedContact+$ PropertyChanged=�$ TwoWay=�
 �� SelectedIndex
 �� *SelectedContactIndex+$ PropertyChanged=�$ TwoWay=�� $��2�����#292b2f �� �� $��35 �.+��� $ 
Horizontal=�.+�A� $��18, 0, 0, 0q�$��30��$��30������  �� *ImageSource+$ PropertyChanged=�$ TwoWay=��� 
�������$��SemiBold"�$��Center=����� *
Username+$ PropertyChanged=�$ TwoWay=��� $��1��$
��Left=�$��Center=�
������ $��0q�$��17��$��17��$��0q�$��0q�
�������.��o� $��0q�$��0q� o� 
IsMainMenu$
 True��.@�m� $��Center=�$��0q�
�������$��SemiBold"����#292b2f$��34����0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ DotsVertical=�.@�m� $��Notifications�� m� Icon   $ Notifications=� m� Command �� *OpenNotificationsPopupCommand+m� $��Find Friend��   $ AccountSearch=� �� *OpenFriendsPopupCommand+m� $��Upload Photo��   $ Upload=� �� *UploadProfilPhotoCommand+m� $��Log Out��   $ Logout=� �� *LogoutCommand+� $��1�� �� $��50 ��� �� $��70 �.+��� ���#2F3136$��
0, 0, 0, 2q�._�� $��Stretch=�$��8q� �� �� $	��200 �.+��� $ 
Horizontal=�.+�� .+�A� $��18, 0, 0, 0q�$��30��$��30������  �� *#!SelectedContact.User2.ImageSource+$ PropertyChanged=�$ TwoWay=��� $��30��$��30��$��18, 0, 0, 0q�
������ $��0q�+  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 25��   OpenDrawerCommand#��Z �� 
�������$
��Bold"�$��
5, 0, 5, 0q�$��Center=����� * SelectedContact.User2.Username+$ PropertyChanged=�$ TwoWay=��� $
 0��$��1��$
��15 0q����	#FF252525)  #MaterialDesignThemes.Wpf.HintAssist   
Foreground �#B13333.  (MaterialDesignThemes.Wpf.TextFieldAssist   UnderlineBrush �#B13333   Hint$ Language��$��5q�$��Microsoft YaHei UI)�$��15(����#FFFFFF	 �� *SelectedLanguage+$ PropertyChanged=�$ TwoWay=����� *	Languages+ � 	Resources   MaterialDesignPaper �� Color��%   &	       �� $ 	#FF252525����   ChatItem#��[ 
������ $��0q�$��1��$��
8, 0, 0, 0q����� *
Messages+$ PropertyChanged=�$ TwoWay=�� $��2�� �� �� $��50 ��� $��50 ��� $	��180 �.+��� $��0q�  
MessageBox#��[ $��50��$��3���� 
������ $��0q�$��2��$��20��$��20��$��0q�$��0, 0, 25, 0q�
������� J� RenderTransformOrigin$ 0.5,0.5&����� *OpenFileCommand+z�V� .~��� �� ��  �� Angle$ 39.806Q�T� .��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	Paperclip=�$��20��$��20���� 
������ $��0q�$��1��$��20��$��20��$��0q�$��10, 0, 0, 0q�
�������.��  $ 
Microphone=�$��20��$��20���� $ 7��$��3�����#B13333$��0q�$��50��$��0, 0, 10, 0q�
����������� *SendMessageCommand+.���� $ 
Horizontal=�.+�  $
 Send=�$��Center=�$��20��$��20��$ 0.5,0.5&�z�V� .~��� �� �� $	 -35Q�T� �� 
�������$
��Bold"�$
��Send��'  !MaterialDesignThemes.Wpf.Snackbar  $��2��$	��550��$��4q�$��Center=�$��Bottom=�   MessageQueue �� *SnackbarMessageQueue+)	  #MaterialDesignThemes.Wpf.DialogHost	   	  CloseOnClickAway$
 True�� 	  IsOpen �� *FriendsPopupIsOpen+$ PropertyChanged=�$ TwoWay=� 	  DialogContent �� $	��500��$ Vertical=����#2F3136.+�(
  "MaterialDesignThemes.Wpf.ColorZone
   
  CornerRadius$ 5�� 
  Mode$ Standard=�$��8 4 8 4q� K� ZIndex$ 1��$��10q����#3e4147$ Depth1=�.���� $ 8�����#3e4147._�� .+��� -   $��
MessageBox��$��50��$��Stretch=�$��Center=�$��Stretch=�
������   �� TextWrapping$
  Wrap=�$��0q�
�������! �� 
CaretBrush
! �����$��
8, 0, 8, 0q����� *
FriendName+$ TwoWay=�$ PropertyChanged=��� " J� IsHitTestVisible$" False��$��Find Friend...��$��Center=�$
��Left=�$��10, 0, 0, 0q�
����������� # �� 
TargetType"# ��.��� $ �� Property$ � y�% �� Value$% 	Collapsed=�& �� Triggers& �� ' �� Binding' �� *Text+( �� ElementName$( 
MessageBox��) �� Value)  .a��� $ � y�$% Visible=��� 
������ $��0q�$��1��  
FriendCard#��[ ���� *Users+$ PropertyChanged=�$ TwoWay=�	  $
 True�� �� *NotificationsPopupIsOpen+$ PropertyChanged=�$ TwoWay=� �� $	��500��$ Vertical=����#2F3136.+��� 
������ $��0q�  NotificationCard#��[ ���� *User.Notifications+$ PropertyChanged=�$ TwoWay=�	  $
 True�� �� *OpenFilePopupIsOpen+$ PropertyChanged=�$ TwoWay=� �� $	��450��$ Vertical=����#2F3136.+��� $	��400��$	��500��$��20q�y��� *ImageVisibility+���� *
FileIcon+$ PropertyChanged=�$ TwoWay=�� $	��310��$��20q�y��� *VideoVisibility+.+�r� -   * r�Name$* Player��+ r� UnloadedBehavior$
+ Stop=�, r� LoadedBehavior$, Manual=�$	��310��- r� Source- �� *VideoSource+$ PropertyChanged=�$ TwoWay=�� -   . �Name$. ControlPanel��/ J� Opacity$
/ 0.01Q�$��Stretch=�$	��310��   MakeVisible	0 W� To1 g� Duration2 �� TargetProperty  MakeInvisible��%�   &	       &	 <     �� .��W� 0 1.0$1 0:00:0.5D�$2 (Viewbox.Opacity)��� .��W� 0 0.01$1 0:00:0.5D�$2 (Viewbox.Opacity)����� "# �& :� 3 :� RoutedEvent$3 
MouseEnter��.[��� #��[ :� $3 
MouseLeave��.[��� #��[ .+��� -   $��
PlayButton��$��Center=�$��Center=� & "MaterialDesignIconForegroundButton#��[ {��� *DataContext.ControlsEnabled+4 �� RelativeSource4 �� *FindAncestor+5 �� AncestorType
5 Window.��  ���#B13333$
 Play=��� $��5q�
�������$��SemiBold"�$��Center=����� *
FileName+$ PropertyChanged=�$ TwoWay=��� $��5q�
�������$��SemiBold"�$��Center=����� *
FileSize+$ PropertyChanged=�$ TwoWay=��� $��30��$��45��$��0q�
����������#B13333$��10q�$��Bottom=�$��Right=�$��0q����� *SendFileCommand+.��  $
 Send=�$��20��$��20��	  $
 True�� �� *DisplayFilePopupIsOpen+$ PropertyChanged=�$ TwoWay=� �� $ Vertical=����#2F3136.+��� $	��850��$	��500��y��� *ImageVisibility+���� *
FileIcon+$ PropertyChanged=�$ TwoWay=�� y��� *VideoVisibility+.+�r� -   $* _Player��$
+ Stop=�$, Manual=�$	��850��$	��600��- �� *VideoSource+$ PropertyChanged=�$ TwoWay=�� $
/ 0.01Q�$��Stretch=�$��Stretch=� ��%�   &	       &	 <     �� .��W� 0 1.0$1 0:00:0.5D�$2 (Viewbox.Opacity)��� .��W� 0 0.01$1 0:00:0.5D�$2 (Viewbox.Opacity)����� "# �& :� $3 
MouseEnter��.[��� #��[ :� $3 
MouseLeave��.[��� #��[ .+��� -   $��Center=�$��Center=�#��[ {��� *DataContext.ControlsEnabled+4 �� *FindAncestor+
5 Window.��  ���#B13333$
 Play=��� -   6 ��Name$6 TimelineSlider��7 � Minimum$7 0Q�$	��400��$��Bottom=�$��Center=�	   �� *LoadingPopupIsOpen+$ PropertyChanged=�$ TwoWay=� � ���#2F3136$	��100��$	��100��.+�� $��60��$��60�� %	 !MaterialDesignCircularProgressBar#��[	 8 � Value$8 0Q�9 � IsIndeterminate$
9 True��	   �� *SoundPopupIsOpen+$ PropertyChanged=�$ TwoWay=� � ���#2F3136$	��450��$	��300��.+��� $7 0Q�: � Maximum$	: 400Q�$8 0Q�