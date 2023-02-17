﻿#pragma checksum "..\..\..\..\..\MVVM\Views\MainView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E7CC088EE1D695909EAB43184DA4C68A69B149A8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Venn.Client.MVVM.ViewModels;
using Venn.Client.MVVM.Views;


namespace Venn.Client.MVVM.Views {
    
    
    /// <summary>
    /// MainView
    /// </summary>
    public partial class MainView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\..\MVVM\Views\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border header;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\MVVM\Views\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnWindowMinimize;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\MVVM\Views\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnWindowMaximize;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\MVVM\Views\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnWindowClose;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Venn.Client;component/mvvm/views/mainview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MVVM\Views\MainView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.header = ((System.Windows.Controls.Border)(target));
            
            #line 26 "..\..\..\..\..\MVVM\Views\MainView.xaml"
            this.header.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnWindowMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\..\MVVM\Views\MainView.xaml"
            this.btnWindowMinimize.Click += new System.Windows.RoutedEventHandler(this.btnWindowMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnWindowMaximize = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\..\..\MVVM\Views\MainView.xaml"
            this.btnWindowMaximize.Click += new System.Windows.RoutedEventHandler(this.btnWindowMaximize_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnWindowClose = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\..\..\MVVM\Views\MainView.xaml"
            this.btnWindowClose.Click += new System.Windows.RoutedEventHandler(this.btnWindowClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

