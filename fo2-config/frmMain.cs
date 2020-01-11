﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fo2_config
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            PlaceText(this, "Created by ", 10, 7, new Size(58, 13));
            PlaceLink(this, "Rotators", "https://github.com/rotators", 65, 7);
            PlaceText(this, "Version 0.9", 10, 20);

            PlaceLink(this, "fodev.net", "https://fodev.net", this.Width-80, 7).Anchor = (AnchorStyles.Top | AnchorStyles.Right);

            ddraw.Init();
            ddraw.Parse(@"E:\Fallout\fo2\Fallout1in2\ddraw.ini");

            ddraw.redrawCallback = () => DrawSelectedDDrawCategory(DDrawTabControl);

            this.RenderTabContent();
        }

        Font linkFont = new System.Drawing.Font("Microsoft Sans Serif", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        int selectedDDrawTab = -1;
        TabControl DDrawTabControl;

        public LinkLabel PlaceLink(Control parent, string text, string href, int x, int y)
        {
            var ll = new LinkLabel();
            ll.AutoSize = true;
            ll.Text = text;
            ll.Location = new Point(x, y);
            ll.Font = linkFont;
            ll.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start(href);
            ll.Parent = parent;
            return ll;
        }

        public void PlaceText(Control parent, string text, int x, int y, Size size)
        {
            var t = PlaceText(parent, text, x, y);
            t.Size = size;
            t.AutoSize = false;
        }

        public Label PlaceText(Control parent, string text, int x, int y)
        {
            var l = new Label();
            l.AutoSize = true;
            l.Text = text;
            l.Location = new Point(x, y);
            l.Parent = parent;
            return l;
        }

        public void RenderMain()
        {
            var p = tabControl.TabPages[0];
            p.SuspendLayout();
            p.Controls.Clear();
            p.ResumeLayout();
        }

        public void DrawCategory(string category, TabPage p)
        {
            var opts = ddraw.options.Where(x => x.category == category && x.found);
            if (opts.Any())
            {
                p.UseVisualStyleBackColor = true;
                int y = 10;
                foreach (var o in opts)
                {
                    y = o.Render(p, y) + 25;
                }
            }
        }

        private void DrawSelectedDDrawCategory(TabControl tc)
        {
            //tc.SelectedTab.Controls.Clear();
            //tc.SuspendLayout();
            var oldControls = new List<Control>();
            foreach (var o in tc.SelectedTab.Controls)
                oldControls.Add((Control)o);

            DrawCategory(tc.SelectedTab.Text, tc.SelectedTab);

            foreach (var o in oldControls)
                tc.SelectedTab.Controls.Remove(o);
        }

        public void RenderDDraw()
        {
            var p = tabControl.TabPages[1];
            p.AutoScroll = true;
            p.SuspendLayout();
            p.Controls.Clear();

            var xPos = 10;
            var yPos = 10;

            PlaceText(p, $"ddraw.ini for sfall {ddraw.Version} loaded from {ddraw.Path}.", xPos, yPos);

            var tc = new TabControl();
            tc.Height = p.Height - 40;
            tc.Width = p.Width - 30;
            tc.Location = new Point(10, 30);
            tc.Parent = p;
            tc.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

            var categories = new string[] { "Main", "Sound", "Speed", "Graphics", "Interface", "Input", "Misc", "Scripts", "Debugging" };
            foreach(var cat in categories)
                tc.TabPages.Add(cat);

            if(selectedDDrawTab != -1)
                tc.SelectedIndex = selectedDDrawTab;

            tc.SelectedIndexChanged += (sender, e) =>
            {
                this.DrawSelectedDDrawCategory(tc);
                selectedDDrawTab = tc.SelectedIndex;
            };

            DDrawTabControl = tc;


            yPos += 20;


            p.ResumeLayout();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RenderTabContent();
        }

        private void RenderTabContent()
        { 
            var idx = tabControl.SelectedIndex;

            if (idx == 0) RenderMain();
            if (idx == 1) RenderDDraw();
        }

        private void tabMain_Resize(object sender, EventArgs e)
        {
           // this.RenderTabContent();
        }
    }
}
