using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LilyPath;
using System.Reflection;

namespace LilyPathDemo
{
    public partial class MainForm : Form
    {
        private Dictionary<string, Action<DrawBatch>> _pages;

        public MainForm ()
        {
            InitializeComponent();
            PopulateList();

            List<string> keys = new List<string>(_pages.Keys);
            keys.Sort();

            foreach (string key in keys)
                listBox1.Items.Add(key);

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

        private void PopulateList ()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            _pages = new Dictionary<string,Action<DrawBatch>>();

            foreach (Module module in assembly.GetModules()) {
                foreach (Type type in module.GetTypes()) {
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static)) {
                        foreach (TestSheetAttribute attr in method.GetCustomAttributes(typeof(TestSheetAttribute), true)) {
                            _pages.Add(attr.Name, Delegate.CreateDelegate(typeof(Action<DrawBatch>), method) as Action<DrawBatch>);
                        }
                    }
                }
            }
        }
    }
}
