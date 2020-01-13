﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace AppendWindowChromeContextMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //A window receives this message when the user chooses a command from the Window menu, or when the user chooses the maximize button, minimize button, restore button, or close button.
        public const Int32 WM_SYSCOMMAND = 0x112;

        //Draws a horizontal dividing line.This flag is used only in a drop-down menu, submenu, or shortcut menu.The line cannot be grayed, disabled, or highlighted.
        public const Int32 MF_SEPARATOR = 0x800;

        //Specifies that an ID is a position index into the menu and not a command ID.
        public const Int32 MF_BYPOSITION = 0x400;

        //Specifies that the menu item is a text string.
        public const Int32 MF_STRING = 0x0;

        //Menu Ids for our custom menu items
        public const Int32 _ItemOneMenuId = 1000;
        public const Int32 _ItemTwoMenuID = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowhandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);

            //Get the handle for the system menu
            IntPtr systemMenuHandle = GetSystemMenu(windowhandle, false);

            //Insert our custom menu items
            InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty); //Add a menu seperator
            InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, _ItemOneMenuId, "Item 1"); //Add a setting menu item
            InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, _ItemTwoMenuID, "Item 2"); //add an About menu item

            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Check if the SystemCommand message has been executed
            if (msg == WM_SYSCOMMAND)
            {
                //check which menu item was clicked
                switch (wParam.ToInt32())
                    {
                        case _ItemOneMenuId:
                            MessageBox.Show("Item 1 was clicked");
                            handled = true;
                            break;
                        case _ItemTwoMenuID:
                            MessageBox.Show("Item 2 was clicked");
                            handled = true;
                            break;
                    }
            }

            return IntPtr.Zero;
        }
    }
}
