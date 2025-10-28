using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System;


namespace Algorithms.Sections
{
    public class Filters
    {
        public static Image<Gray, byte> ExpandImage(Image<Gray,byte> initialImage, int width, int height)
        {
            Image<Gray, byte> expandedImage = new Image<Gray, byte>(initialImage.Width + width * 2, initialImage.Height + height * 2);
            for(int y=height; y< expandedImage.Height - height; y++)
            {
                for(int x=width; x< expandedImage.Width - width; x++)
                {   
                    int coord_y=Math.Min(initialImage.Height - 1, Math.Max(0, y - height));
                    int coord_x=Math.Min(initialImage.Width - 1, Math.Max(0, x - width));

                    expandedImage.Data[y, x, 0] = initialImage.Data[coord_y, coord_x, 0];
                }
            }
            return expandedImage;
        }
        public static Image<Gray, byte> ApplyFilter(Image<Gray, byte> initialImage, int filterWidth, int filterHeight)
        {
            
            return ExpandImage(initialImage, filterWidth / 2, filterHeight / 2);
        }
    }
}