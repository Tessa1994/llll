using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAXLuckydraw
{
    public partial class ControlFrom : Form
    {
        
        private static int numofPrize = 12;     //number of prize default 12
        private static int buttonPerRow = 3;    //default 4
        private List<Button> buttons = new List<Button>();
        private Slot[][] slots;

        private Point[][] coords;

        private static int[] Xcoords = {554,751,943,1129,1321};     //coordanation of X
        private static int[] Ycoords = { 120, 462, 776 };
        //private static int width = 150;
        //private static int height = 190;

        public MainForm Main { get; set; }  //reference back to main form



        public ControlFrom()
        {
            InitializeComponent();
            if (Screen.AllScreens.Length > 1)       //Auto position the control form on Screen 2
            {
                Screen screen = Screen.AllScreens[0];
                this.Left = screen.Bounds.Left;
                this.Top = screen.Bounds.Top;
                this.StartPosition = FormStartPosition.Manual;
                //  this.CenterToScreen();
            }

            //initialize buttons
            tableLayoutPanel3.ColumnCount = buttonPerRow;
            tableLayoutPanel3.RowCount = numofPrize / buttonPerRow;


            for (int i = 0; i < numofPrize; i++) 
            {
                Button btn = new Button()
                {
                    Size = new Size(200,100),
                    Text = "Draw Prize " + (i + 1)
                };
                
                btn.Size = new Size(200, 100);
                btn.Text = "Draw Prize "+(i+1);
                btn.Click += Draw_Btn_Click;
                btn.Tag = (int)(i + 1);
                tableLayoutPanel3.Controls.Add(btn);
                buttons.Add(btn);

            }

            //initialize slots
            slots = new Slot[3][];
            coords = new Point[3][];
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i] = new Slot[5];
                coords[i] = new Point[5];
                for(int j = 0; j < 5; j++)
                {
                    if (i == 0)
                    {
                        slots[i][j] = new Slot(ImageBuffer.ImageType.VendorLogo);
                    }
                    else if (i == 1)
                    {
                        slots[i][j] = new Slot(ImageBuffer.ImageType.Customer);
                    }
                    else if (i == 2)
                    {
                        slots[i][j] = new Slot(ImageBuffer.ImageType.PricePicture);
                    }
                    coords[i][j] = new Point(Xcoords[j], Ycoords[i]);
                    slots[i][j].Location = coords[i][j];
                }
            }

        }

        Random r = new Random(DateTime.Now.Millisecond);
        private int currentDrawing = 0;
        private void Draw_Btn_Click(object sender, EventArgs e)
        {
            
            ((Button)sender).Enabled = false;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!Main.Controls.Contains(slots[i][j])) Main.Controls.Add(slots[i][j]);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    slots[i][j].begin();
                }
            }
            currentDrawing = (int)((Button)sender).Tag;
            Thread t = new Thread(new ThreadStart(runningDraw));
            t.Start();
            }

        /**
         * Draw animation
         * */
        private void runningDraw()
        {
            Thread.Sleep(5000);
            stopRow(0,currentDrawing);
            Thread.Sleep(5000);
            stopRow(2,currentDrawing);
            Thread.Sleep(5000);
            stopRow(1,0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /**
            Reset all disabled buttons to enabled.
        */
        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach(Button btn in buttons)
            {
                btn.Enabled = true;
            }
                
        }

        /**
         * Stop row with random delay.
         * */
        void stopRow(int row,int value)
        {
            if(row >=0 && row <= 2)
            {
                int v;
                if(value == 0)v= r.Next(0, 1000);
                else
                {
                    v = value;
                }
                for (int j = 0; j < 5; j++)
                {
                    Thread.Sleep(100);
                    slots[row][j].stop(v);
                }
            }

        }

        public void setStatus(String text)
        {
            label1.Text = text;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
