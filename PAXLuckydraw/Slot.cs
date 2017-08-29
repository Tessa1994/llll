using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PAXLuckydraw
{

    /**
     * 
     * This user control represents a slot on the drawing board.
     * */
    public partial class Slot : UserControl
    {
        private static Random r = new Random(DateTime.Now.Millisecond);
        private static ImageBuffer buff = ImageBuffer.Instance;
        private int step = 20;      //Step value dertemines the smoothiness of animation, requires more graphic processing power for lower values
        private int position = 0;
        private Image currentImage;
        private Image nextImage;
        private ImageBuffer.ImageType type; //0: vendor logo 1: customer face 2: prize picture
        private static Object locker = new Object();
        private Boolean isStopped = false;
        private int stopIndex =0;
        private int countDown = 190;

        /**
         * Initialize a slot with type. 
         * */
        public Slot(ImageBuffer.ImageType t)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(255, r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            type = t;
        }
        Timer timer = new Timer();

        public void begin()
        {
            position = 0;
          
            currentImage = buff.getNext(type);
            nextImage = buff.getNext(type);
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            timer.Start();
            isStopped = false;
            countDown = 190;
        }

        

        private void timer_Tick(object sender, EventArgs e)
        {
            lock (locker)
            {

            
            if (isStopped)
            {
                countDown -= step;
                if (countDown <= 0 && position == 0)
                {
                 //   currentImage = buff.get(type, stopIndex);
                 //   nextImage = buff.get(type, stopIndex);
                    
                    timer.Stop();
                        if (!this.IsDisposed)
                        {
                            using (Graphics g = this.CreateGraphics())
                            {
                                g.DrawImage(currentImage, 0, position, new Rectangle(0, 0, 150, 190), GraphicsUnit.Pixel);
                                return;
                            }
                        }
                    }

                }
           
                if (!this.IsDisposed)
                {

                
                using (Graphics g = this.CreateGraphics())
                {

                    g.DrawImage(currentImage, 0, position, new Rectangle(0, 0, 150, 190 - position), GraphicsUnit.Pixel);
                    g.DrawImage(nextImage, 0, 0, new Rectangle(0, 190 - position, 150, position), GraphicsUnit.Pixel);
                }
                }

                if ((position += step) >= 190)
                {
                    position = 0;
                    currentImage = nextImage;
                    if (!isStopped)
                    {
                        nextImage = buff.getNext(type);
                    }
                    else
                    {
                        nextImage = buff.get(type, stopIndex);
                    }
                }
            }


       
         }
 
        public void stop(int i)
        {
            lock (locker)
            { 
            isStopped = true;
            stopIndex = i;
            countDown = (190-position) + 190;
            }
            //timer.Stop();
            //System.Threading.Thread.Sleep(500);

            //lock (locker)
            //{
            //    if (!this.IsDisposed)
            //    {
            //        using (Graphics g = this.CreateGraphics())
            //        {
            //            currentImage = nextImage;
            //            nextImage = buff.get(type, i);
            //            for (int j = 1; j <= 190; j++)
            //            {
            //                g.DrawImage(currentImage, 0, j, new Rectangle(0, 0, 150, 190 - j), GraphicsUnit.Pixel);
            //                g.DrawImage(nextImage, 0, 0, new Rectangle(0, 190 - j, 150, j), GraphicsUnit.Pixel);
            //            }
            //        }
            //    }
            //}
        }

         
    }
}
