using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;


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
        public static Image<Gray, int> ExpandImageInt(Image<Gray, int> initialImage, int width, int height)
        {
            Image<Gray, int> expandedImage = new Image<Gray, int>(initialImage.Width + width * 2, initialImage.Height + height * 2);
            for (int y = height; y < expandedImage.Height - height; y++)
            {
                for (int x = width; x < expandedImage.Width - width; x++)
                {
                    int coord_y = Math.Min(initialImage.Height - 1, Math.Max(0, y - height));
                    int coord_x = Math.Min(initialImage.Width - 1, Math.Max(0, x - width));

                    expandedImage.Data[y, x, 0] = initialImage.Data[coord_y, coord_x, 0];
                }
            }
            return expandedImage;
        }
        public static Image<Gray,int> ApplyFilterInt(Image<Gray, int> initialImage, double[,] filter)
        {
            int height = filter.GetLength(0);
            int width = filter.GetLength(1);
            int half_h = height / 2;
            int half_w = width / 2;

            Image<Gray, int> resultImage = new Image<Gray, int>(initialImage.Width, initialImage.Height);
            Image<Gray, int> temp = ExpandImageInt(initialImage, half_w, half_h);

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
                    resultImage.Data[y, x, 0] = (int)kernelValue;

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
       

        public static Image<Gray, byte> LaplaceFilter(Image<Gray, byte> initialImage)
        {
            if (initialImage == null) return null;

            double sigma = 1.0;

            double[,] laplaceMask = new double[,]
           {
                {0,-1,0 },
                {-1,4,-1 },
                {0,-1,0 }
           };

            Image<Gray, byte> low = GaussFilter(initialImage, sigma, sigma);

            Image<Gray,int> lowInt = new Image<Gray, int>(low.Width, low.Height);
            for (int y = 0; y < low.Height; y++)
            {
                for (int x = 0; x < low.Width; x++)
                {
                    lowInt.Data[y, x, 0] = (int)low.Data[y, x, 0];
                }
            }

            Image<Gray,int> lap = ApplyFilterInt(lowInt, laplaceMask);

            Image<Gray, byte> result = new Image<Gray, byte>(lap.Width, lap.Height);
            for (int y = 0; y < lap.Height; y++)
            {
                for (int x = 0; x < lap.Width; x++)
                {
                    double v = lap.Data[y, x, 0] + 128.0;
                    int iv = (int)Math.Round(v);
                    iv = Math.Max(0, Math.Min(255, iv));
                    result.Data[y, x, 0] = (byte)iv;
                }
            }

            return result;
        }
        public static Image<Gray, int> LaplaceFilter2(Image<Gray, byte> initialImage)
        {
            if (initialImage == null) return null;

            double sigma = 1.0;

            double[,] laplaceMask = new double[,]
           {
                {0,-1,0 },
                {-1,4,-1 },
                {0,-1,0 }
           };

            Image<Gray, byte> low = GaussFilter(initialImage, sigma, sigma);

            Image<Gray, int> lowInt = new Image<Gray, int>(low.Width, low.Height);
            for (int y = 0; y < low.Height; y++)
            {
                for (int x = 0; x < low.Width; x++)
                {
                    lowInt.Data[y, x, 0] = (int)low.Data[y, x, 0];
                }
            }

            Image<Gray, int> lap = ApplyFilterInt(lowInt, laplaceMask);

            Image<Gray, int> result = new Image<Gray, int>(lap.Width, lap.Height);
            for (int y = 0; y < lap.Height; y++)
            {
                for (int x = 0; x < lap.Width; x++)
                {
                    double v = lap.Data[y, x, 0];
                    int iv = (int)Math.Round(v);
                   // iv = Math.Max(0, Math.Min(255, iv));
                    result.Data[y, x, 0] = (int)iv;
                }
            }

            return result;
        }

        public static Image<Gray, byte> ZeroCrossingFilter(Image<Gray, byte> initialImage)
        {
            if (initialImage == null) return null;

            Image<Gray, int> temp = LaplaceFilter2(initialImage);

            int T = 10;

            Image<Gray, byte> result = new Image<Gray,byte>(initialImage.Width,initialImage.Height);

            for (int y = 2; y < temp.Height-1; y++)
            {
                for (int x = 2; x < temp.Width-1; x++)
                {
                    if ((temp.Data[y, x - 1, 0] >= T && temp.Data[y + 1, x, 0] <= -T) ||
                        (temp.Data[y - 1, x - 1, 0] >= T && temp.Data[y + 1, x + 1, 0] <= -T)
                            || (temp.Data[y - 1, x, 0] >= T && temp.Data[y, x + 1, 0] <= -T))
                        result.Data[y, x, 0] = (byte)255;
                    else if ((temp.Data[y, x - 1, 0] <= -T && temp.Data[y + 1, x, 0] >= T) ||
                        (temp.Data[y - 1, x - 1, 0] <= -T && temp.Data[y + 1, x + 1, 0] >= T)
                            || (temp.Data[y - 1, x, 0] <= -T && temp.Data[y, x + 1, 0] >= T))
                        result.Data[y, x, 0] = (byte)255;
                    else if ((result.Data[y, x - 1, 0] <= -T && result.Data[y - 1, x, 0] >= T) || (result.Data[y + 1, x - 1, 0] <= -T && result.Data[y - 1, x + 1, 0] >= T) ||
                        (result.Data[y + 1, x, 0] <= -T && result.Data[y, x + 1, 0] >= T))
                        result.Data[y, x, 0] = (byte)255;
                    else if ((result.Data[y, x - 1, 0] >= T && result.Data[y - 1, x, 0] <= -T) || (result.Data[y + 1, x - 1, 0] >= T && result.Data[y - 1, x + 1, 0] <= -T) ||
                        (result.Data[y + 1, x, 0] >= T && result.Data[y, x + 1, 0] <= -T))
                        result.Data[y, x, 0] = (byte)255;

                }
            }
            return result;

        }
        

    }
}


