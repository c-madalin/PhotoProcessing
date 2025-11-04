using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using System.Security.Cryptography;


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
        public static Image<Gray, byte> ApplyFilter(Image<Gray, byte> initialImage, double[,] filter)
        {
            int height = filter.GetLength(0);
            int width = filter.GetLength(1);
            int half_h = height / 2;
            int half_w = width / 2;

            Image<Gray, byte> resultImage = new Image<Gray, byte>(initialImage.Width, initialImage.Height);
            Image<Gray, byte> temp = ExpandImage(initialImage, half_w, half_h);

            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    double kernelValue = 0.0;

                    for (int offY = -half_h; offY <= half_h; offY++)
                    {
                        for (int offX = -half_w; offX <= half_w; offX++)
                        {
                            kernelValue += filter[offY + half_h, offX + half_w] *
                                           temp.Data[y + offY + half_h, x + offX + half_w, 0];
                        }
                    }

                    resultImage.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, (int)kernelValue));
                }
            }
            return resultImage;
        }
        
    

        public static double[,] GaussMask(double sigma_x, double sigma_y)
        {
            int width = (int)Math.Ceiling(4 * sigma_x);
            int height = (int)Math.Ceiling(4 * sigma_y);
            if (width % 2 == 0) width++;
            if (width < 1) width = 1;

            if (height % 2 == 0) height++;
            if (height < 1) height = 1;

            double[,] mask = new double[height, width];

            int half_w = width / 2;
            int half_h = height / 2;

            double twoSigmaXSq = 2.0 * sigma_x * sigma_x;
            double twoSigmaYSq = 2.0 * sigma_y * sigma_y;
            double denom = 2.0 * Math.PI * sigma_x * sigma_y;

            double sum = 0.0;

            for (int iy = -half_h; iy <= half_h; iy++)
            {
                for (int ix = -half_w; ix <= half_w; ix++)
                {
                    double exponent = -((ix * ix) / twoSigmaXSq + (iy * iy) / twoSigmaYSq);
                    double value = (1.0 / denom) * Math.Exp(exponent);
                    mask[iy + half_h, ix + half_w] = value;
                    sum += value;
                }
            }

           
            if (sum != 0.0)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        mask[y, x] /= sum;
                    }
                }
            }


            return mask;
        }

        public static Image<Gray,Byte> GaussFilter(Image<Gray,byte> initialImage,double sigma_x, double sigma_y)
        {
    
            if (initialImage == null) return null;
            if (sigma_x <= 0 && sigma_y <= 0) return initialImage.Clone();

            double[,] gaussMask = GaussMask(sigma_x, sigma_y);
            return ApplyFilter(initialImage, gaussMask);


        }
    }
}


