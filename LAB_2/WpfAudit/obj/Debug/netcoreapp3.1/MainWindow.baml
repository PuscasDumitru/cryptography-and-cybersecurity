   M S B A M L   `   `   `  ����C  ?WpfAudit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WpfAuditWpfAudit  5   	       WpfAudit.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:WpfAudit     5   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1400��6)    � 	Resources 5
       �� 
TargetType �� Property �� Value   listTemplate d� 	IsChecked �� Path��%
  '��       '�� H     &	  �     �� 5   
   " ��6   .�5      ��  � ��$ 14(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 14(�6)   �� 5       �@��TextWrapping$ WrapWithOverflow=�6-   5      �� 5   
   .R�5      �� $	��400��6   $��0,20,5,0q�6%   .+�5      �� $��20��6@   $��Select Policy��6L    6   �� $ 	PolicyBox�6L   9� 5      $��Policy Type��6   .��5      �� ��6   �� $ 
PolicyType�5      9� 5      $��Policy Description��6   .��5       �� ��6   �� $ PolicyDescription�5!      9� 5#      $��Policy Info��6   .��5$      �� ��6   �� $ 
PolicyInfo�5%      9� 5'      $��Policy Solution��6   .��5(      �� ��6   �� $ PolicySolution�5)      9� 5+      $��Policy Reference��6   .��5,      �� ��6   �� $ PolicyReference�5-      9� 5/      $��Policy See Also��6   .��50      �� ��6   �� $ PolicySeeAlso�51      9� 53      $��Policy Value Type��6   .��54      �� ��6   �� $ PolicyValueType�55      9� 57      $��Policy Value Data��6   .��58      �� ��6   �� $ PolicyValueData�59      9� 5;      $��Policy Value Note��6   .��5<      �� ��6   �� $ PolicyValueNote�5=      9� 5?      $��Policy Value Regex��6   .��5@      �� ��6   �� $ PolicyValueRegex�5A      9� 5C      $��Policy Value Expect��6   .��5D      �� ��6   �� $ PolicyValueExpect�5E      5F      5G      5H      .��5J      �  J� RenderTransformOrigin$ 
0.48,0.527&�6    � ColumnDefinitions 5K   
   �� 5L      $��2.14* �6   �� 5M      $��5* �6   .+�5P   
   �� $��1��6   ��6   �� *Audits+	 �� SelectedItem	 5Q      �� *SelectedAudit+��5R      �� 5S      .R�5T      �� $��5q�6!   .+�5U      �� $��18(�6$   ��62   �� $ GenInfo.DisplayName�5W      5X      5Z      �� 5^   
   $��1��6   $��Bottom=�6"   .+�5_      �� ��6   �� *
AddCommand+.��64   Upload file6A   �� 5`      ��6   �� *RemoveCommand+��5a      �� *SelectedAudit+.��6@   Remove File6M   5c      �� 5f   
   #��[  6   $��0,70,0,0q�5g      $��Disabled=�5h      $��1��5i      ��6"   �� *SelectedAudit.CustomItems+	 5j      �� *SelectedAudit+��5m      �� 5n      .R�5p      � 5r      5t      �� 5x   
   $��1��6*   $
��Left=�6:   $��
212,26,0,0q�6U   $	��Top=�6i   $	��170��6�   $��21��6�   ��6   �� *Search+6�   �� 5y   
   $��Search��64   $��1��6E   $
��Left=�6U   $��
400,26,0,0q�6p   $	��Top=�6�   $��68��6�   $��21��6�   ��6   �� *SearchCommand+6�   �� 5z   
   $��1��6,   $
��Left=�6<   $��
542,26,0,0q�6W   $	��Top=�6k   $	��168��6�   ��6   �� *
SaveName+6�   �� 5{   
   $��Save As��62   $��1��6D   $
��Left=�6T   $��
734,27,0,0q�6o   $	��Top=�6�   $��59��6�   $��20��6�   ��6   �� *SaveCommand+6�   �� 5}   
   
 d� IsThreeState$
 False��6   $��Select All Policies��6(   $��1��6F   $
��Left=�6V   $��	26,30,0,0q�6q   $	��Top=�6�   $��29��6�   $	��129��6�    5          WpfAudit.CheckBoxConverter 
 cbConv��%   &	         5�      6*   5�       5�      `� 5�       `� 	Converter# [ 6   .1�5�      �� $ CheckPolicies�6   5�      ��5�      �� 5�      " ��6    �� Triggers 5�      P� 5�       P� Property �@d�	IsChecked P� Value$
 trueY�67   . �5�      ��  � ��
 � � �6;   5�      P� 5�       �@d�	IsChecked$ falseY�67   . �5�      ��  � ��
 �  ��6;   5�      5�      5�      5�      5�      