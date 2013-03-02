using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LilyPathDemo
{
    public partial class MainForm : Form
    {
        private Dictionary<string, Action> _pages;

        public MainForm ()
        {
            InitializeComponent();

            _pages = new Dictionary<string, Action>() {
                { "Outlined Shapes", drawingControl1.DrawOutlineShapes },
                { "Pen Alignment", drawingControl1.DrawLineAlignment },
                { "Primitive Shapes", drawingControl1.DrawPrimitiveShapes },
            };

            foreach (var item in _pages)
                listBox1.Items.Add(item.Key);

            listBox1.SelectedValueChanged += ListBoxSelectedValueChanged;
            listBox1.SelectedItem = "Primitive Shapes";
        }

        private void ListBoxSelectedValueChanged (object sender, EventArgs e)
        {
            string key = (string)listBox1.SelectedItem;
            if (_pages.ContainsKey(key)) {
                drawingControl1.DrawAction = _pages[key];
            }
        }
    }
}
