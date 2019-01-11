using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MouseLocation
{
    public partial class Form1 : Form
    {
        private MouseHook hook;
        private int originLocationX;
        private int originLocationY;
        private int nowLocationX;
        private int nowLocationY;
        private bool isHotkeyPressed = false;

        public Form1()
        {
            InitializeComponent();
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Shift, Keys.S);
            hook = new MouseHook();
            hook.SetHook();
            hook.MouseMoveEvent += Hook_MouseMoveEvent;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hook.UnHook();
            HotKey.UnregisterHotKey(Handle, 100);
        }

        private void Hook_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            nowLocationX = e.Location.X;
            nowLocationY = e.Location.Y;

            this.label_NowX.Text = e.Location.X.ToString();
            this.label_NowY.Text = e.Location.Y.ToString();
            
            if(!isHotkeyPressed)
            {
                originLocationX = nowLocationX;
                originLocationY = nowLocationY;
                this.label_OriX.Text = e.Location.X.ToString();
                this.label_OriY.Text = e.Location.Y.ToString();
            }
            this.label_DistanceX.Text = ((int)(nowLocationX - originLocationX)).ToString();
            this.label_DistanceY.Text = ((int)(nowLocationY - originLocationY)).ToString();
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch(m.Msg)
            {
                case WM_HOTKEY:
                    switch(m.WParam.ToInt32())
                    {
                        case 100:
                            this.HotKeyPressed();
                            break;
                    }
                break;
            }
            base.WndProc(ref m);
        }
        private void HotKeyPressed()
        {
            if(isHotkeyPressed)
            {
                isHotkeyPressed = false;
                this.label_OriX.BackColor = Color.White;
                this.label_OriY.BackColor = Color.White;
            }
            else
            {
                this.label_OriX.BackColor = Color.DarkGray;
                this.label_OriY.BackColor = Color.DarkGray;
                isHotkeyPressed = true;
                originLocationX = nowLocationX;
                originLocationY = nowLocationY;
            }
        }
    }
}
