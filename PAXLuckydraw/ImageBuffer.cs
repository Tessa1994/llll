using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAXLuckydraw
{

    /**
     * This Singleton class cache all images in memory.
     */
    public class ImageBuffer
    {
        public enum ImageType { VendorLogo, Customer, PricePicture };
        
        private static String configfile= "C:\\Users\\Cathy\\Documents\\Visual Studio 2017\\Projects\\Registration\\Registration\\Photo\\config.csv";  //config file for image locations
        private static Random r = new Random(DateTime.Now.Millisecond);
        private Image[] buffer = new Image[1000];
        private List<Prize> prizes = new List<Prize>();
        private static ImageBuffer _Instance = new ImageBuffer();

        public static ImageBuffer Instance{get{return _Instance;}}

        private void initialize()
        {
            for (int i = 0; i < 1000; i++)
            {
                buffer[i] = createImage();
            }

            if(prizes.Count == 0)
            {
                String[] lines = File.ReadAllText(@configfile).Split(new String[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
                foreach(String line in lines)
                {
                    Prize p = new Prize();
                    String[] values = line.Split(',');
                    p.id = int.Parse(values[0]);
                    p.PrizePicture = new Bitmap(Image.FromFile(values[1]),new Size(150,190));
                    if (!"null".Equals(values[2])) p.VendorLogo = ResizeImage(Image.FromFile(values[2]), 150,190);
                    else p.VendorLogo = new Bitmap(150, 190);

                    prizes.Add(p);
                }
            }


        }


        /**
         * 
         * Helper class to resize source image to required resolution to target slot size.
         * */
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private ImageBuffer()
        {
            initialize();
            
        }

        public Prize getPrize(int i)
        {
            foreach(Prize p in prizes)
            {
                if (p.id == i) return p;
            }return null;
        }

        /**
         Get the specified image at location i in the image buffer.
             */
        public Image get(ImageType type, int i){
            if(type == ImageType.PricePicture || type==ImageType.VendorLogo)
            {
                foreach(Prize p in prizes)
                {
                    if(p.id == i)
                    {
                        if (ImageType.PricePicture == type) return p.PrizePicture;
                        else if (ImageType.VendorLogo == type) return p.VendorLogo;
                    }
                }
            }
            return buffer[i];
        }

        /**
         Get the next random image for specific type.
             */
        public Image getNext(ImageType type){
            if (type == ImageType.PricePicture || type == ImageType.VendorLogo)
            {
                int i = r.Next(0, prizes.Count);
                        if (ImageType.PricePicture == type) return prizes[i] .PrizePicture;
                        else if (ImageType.VendorLogo == type) return prizes[i].VendorLogo;
                    
                
            }

            return buffer[r.Next(0,1000)];
        }

        /**
         * 
         * Create image with random color. This method is for test/demo only.
         * */
        private Image createImage()
        {
            Bitmap bitmap = new Bitmap(150, 190);
            Color color = Color.FromArgb(255, r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(new Pen(color), 0, 0, 150, 190);
                g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, 150, 190));
            }

            return bitmap;
        }
    
    }
}
