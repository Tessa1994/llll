using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAXLuckydraw
{
    public partial class MainForm : Form
    {
        private ControlFrom controlForm = new ControlFrom();
        public MainForm()
        {
            InitializeComponent();
            controlForm.Main = this;
            controlForm.Show();
        
            //MessageBox.Show(Screen.AllScreens[1].WorkingArea.ToString());
        }


        /**
         Helper method to identify slot position.
             */
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            controlForm.setStatus("Mouse Position: x - " + e.X + " y -" + e.Y);
        }
    }
}
