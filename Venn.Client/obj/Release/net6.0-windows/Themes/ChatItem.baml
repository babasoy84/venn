   M S B A M L   `   `   `  ���� W  SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089_ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a  �� 
TargetType   ChatItem �� Property �� Value �� Orientation �� ImageSource �� RelativeSource �� AncestorType �� UpdateSourceTrigger �� Mode	 �� 	Resources  Key
 r� LoadedBehavior r� Source & "MaterialDesignIconForegroundButton'   !MaterialDesignThemes.Wpf.PackIcon    Kind �� Triggers ^� 
Conditions �� Binding �� Value  SelfImageRow  ImageRow  SelfVideoRow  VideoRow  SelfTextRow�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  %,  &	        7  7  7  7  7  7  7  �� "  ��.���  � �� �� .R��� $ 
Horizontal=�.+�A� $	��Top=�$��30��$��30��$��10, 0, 0, -5q�����  �� */-DataContext.SelectedContact.User2.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=��� .+��� $ 
Horizontal=�.+��� ���#B13333$��SemiBold"�$��Center=�$��0, 0, -5, 0q����� *,*DataContext.SelectedContact.User2.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+�� $
{�True��$
��Wrap=�
�������$��SemiBold"�$��Center=����� *Data+	 ���� SelfImageRow .R��� $ 
Horizontal=�$��Right=�.+��� .+��� $ 
Horizontal=�$��Right=�.+��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+�� 
����� �$��SemiBold"�$��Center=�$��0, -5, 0, 0q����� *DataContext.User.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=�� $��10q�$��Right=�.+��� $	��150��$��Center=����� *Data+�� $	��150��$��Center=�$��Stretch=�
������ $��0q����� *Data+���� *!DataContext.DisplayImageCommand+ ��  ListViewA� $	��Top=�$��30��$��30��$��0, 0, 10, -5q�����  �� *DataContext.User.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=��� ImageRow .R��� $ 
Horizontal=�.+�A� $	��Top=�$��30��$��30��$��10, 0, 0, -5q�����  �� */-DataContext.SelectedContact.User2.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=��� .+��� $ 
Horizontal=�.+��� ���#B13333$��SemiBold"�$��Center=�$��0, 0, -5, 0q����� *,*DataContext.SelectedContact.User2.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+� $��10q�.+��� $	��150��$��Center=����� *Data+�� $	��150��$��Center=�$��Stretch=�
������ $��0q����� *Data+���� *!DataContext.DisplayImageCommand+ ��  ListView�� SelfVideoRow .R��� $ 
Horizontal=�$��Right=�.+��� .+��� $ 
Horizontal=�$��Right=�.+��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+�� 
����� �$��SemiBold"�$��Center=�$��0, -5, 0, 0q����� *DataContext.User.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=�� $	��150��$��10q�.+�r� $
 Pause=�$	��150�� �� *Data+�� $��
PlayButton��$��Center=�$��Center=�8��  {��� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window���� *Data+���� *!DataContext.DisplayVideoCommand+ ��  ListView.��   ���#B13333$
 Play=�A� $	��Top=�$��30��$��30��$��0, 0, 10, -5q�����  �� *DataContext.User.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=��� VideoRow .R��� $ 
Horizontal=�.+�A� $	��Top=�$��30��$��30��$��10, 0, 0, -5q�����  �� */-DataContext.SelectedContact.User2.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=��� .+��� $ 
Horizontal=�.+��� ���#B13333$��SemiBold"�$��Center=�$��0, 0, -5, 0q����� *,*DataContext.SelectedContact.User2.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+� $	��150��$��10q�.+�r� $
 Pause=�$	��150�� �� *Data+�� $��
PlayButton��$��Center=�$��Center=�8�� {��� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window���� *Data+���� *!DataContext.DisplayVideoCommand+ ��  ListView.��   ���#B13333$
 Play=��� SelfTextRow .R��� $ 
Horizontal=�$��Right=�.+��� .+��� $ 
Horizontal=�$��Right=�.+��� ���#44474D$��SemiBold"�$��8(�$��Center=����� *SendingTime+�� 
����� �$��SemiBold"�$��Center=�$��0, -5, 0, 0q����� *DataContext.User.Username+ ��  ListView$ PropertyChanged=�$ TwoWay=��� $
{�True��$
��Wrap=�
�������$��SemiBold"�$��Center=�$��Right=����� *Data+A� $	��Top=�$��30��$��30��$��0, 0, 10, -5q�����  �� *DataContext.User.ImageSource+ ��  ListView$ PropertyChanged=�$ TwoWay=� ^�  ��  �� *IsSelf+ True��  �� *MessageType+	 image.0���  � ��8  ^�  ��  �� *IsSelf+	 False��  �� *MessageType+	 image.0���  � ��8  ^�  ��  �� *IsSelf+ True��  �� *MessageType+	 video.0���  � ��8  ^�  ��  �� *IsSelf+	 False��  �� *MessageType+	 video.0���  � ��8  ^�  ��  �� *IsSelf+ True��  �� *MessageType+ text.0���  � ��8  