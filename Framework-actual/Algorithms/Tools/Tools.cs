using Emgu.CV;
using Emgu.CV.Structure;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace Algorithms.Tools
{
    public class Tools
    {
        #region Copy
        public static Image<Gray, byte> Copy(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = inputImage.Clone();
            return result;
        }

        public static Image<Bgr, byte> Copy(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = inputImage.Clone();
            return result;
        }
        #endregion

        #region Invert
        public static Image<Gray, byte> Invert(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (byte)(255 - inputImage.Data[y, x, 0]);
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Invert(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (byte)(255 - inputImage.Data[y, x, 0]);
                    result.Data[y, x, 1] = (byte)(255 - inputImage.Data[y, x, 1]);
                    result.Data[y, x, 2] = (byte)(255 - inputImage.Data[y, x, 2]);
                }
            }
            return result;
        }
        #endregion

        #region Binarizare

        public static Image<Gray, byte> Binar2(Image<Gray, byte> inputImage, int treshold)
        {

            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);


            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    if (inputImage.Data[y, x, 0] > treshold)
                    {
                        result.Data[y, x, 0] = 255;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Mirror
        public static Image<Gray, byte> Mirror(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = inputImage.Data[y,inputImage.Width - 1 - x, 0];
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Mirror(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    int mirroredX = inputImage.Width - 1 - x; 

                    result.Data[y, x, 0] = inputImage.Data[y, mirroredX, 0];
                    result.Data[y, x, 1] = inputImage.Data[y, mirroredX, 1];
                    result.Data[y, x, 2] = inputImage.Data[y, mirroredX, 2];
                }
            }
            return result;
        }
        #endregion

        #region Rotate

        public static Image<Gray, byte> Rotate(Image<Gray, byte> inputImage,bool direction)
        {   
            int newWidth = inputImage.Height;
            int newHeight = inputImage.Width;

            // da frate inertim latimea cu lingimea


            Image<Gray, byte> result = new Image<Gray, byte>(newHeight,newWidth);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    if(direction)
                    {
                        result.Data[x, newWidth - 1 - y, 0] = inputImage.Data[y, x, 0];
                    }
                    else
                    {
                        result.Data[newHeight - 1 - x, y, 0] = inputImage.Data[y, x, 0];
                    }
                }
            }
            return result;
        }




        public static Image<Bgr, byte> Rotate(Image<Bgr, byte> inputImage, bool direction)
        {
            int newWidth = inputImage.Height; 
            int newHeight = inputImage.Width; 

            Image<Bgr, byte> result = new Image<Bgr, byte>(newWidth, newHeight);


            for (int y = 0; y < inputImage.Height; ++y) 
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    if (direction) 
                    {
                        result.Data[x, newWidth - 1 - y, 0] = inputImage.Data[y, x, 0];
                        result.Data[x, newWidth - 1 - y, 1] = inputImage.Data[y, x, 1];
                        result.Data[x, newWidth - 1 - y, 2] = inputImage.Data[y, x, 2];
                    }
                    else
                    {
                        result.Data[newHeight - 1 - x, y, 0] = inputImage.Data[y, x, 0];
                        result.Data[newHeight - 1 - x, y, 1] = inputImage.Data[y, x, 1];
                        result.Data[newHeight - 1 - x, y, 2] = inputImage.Data[y, x, 2];
                    }
                }
            }
            return result;
        }

        #endregion

        #region Crop

        public static Image<Gray, byte> Crop(Image<Gray, byte> inputImage,int StangaSusX,int StangaSusY,int DreaptaJosX, int DreaptaJosY)
        {
            int newWidth = DreaptaJosX - StangaSusX;
            int newHeight = DreaptaJosY - StangaSusY;


            Image<Gray, byte> result = new Image<Gray, byte>(newWidth, newHeight);

            for (int y = StangaSusY; y < DreaptaJosY; ++y)
            {
                for (int x = StangaSusX; x < DreaptaJosX; ++x)
                {
                    result.Data[y - StangaSusY, x - StangaSusX, 0] = inputImage.Data[y, x, 0];

                    //if(y==StangaSusY || y==DreaptaJosY-1|| x==StangaSusX || x==DreaptaJosX-1 )
                    //inputImage.Data[y, x, 0] = 128;
                }
            }

            return result;
        }
        public static Image<Bgr, byte> Crop(Image<Bgr, byte> inputImage, int StangaSusX, int StangaSusY, int DreaptaJosX, int DreaptaJosY)
        {
            int newWidth = DreaptaJosX - StangaSusX;
            int newHeight = DreaptaJosY - StangaSusY;

            Image<Bgr, byte> result = new Image<Bgr, byte>(newWidth, newHeight);

            for (int y = StangaSusY; y < DreaptaJosY; ++y)
            {
                for (int x = StangaSusX; x < DreaptaJosX; ++x)
                {
                    result.Data[y - StangaSusY, x - StangaSusX, 0] = inputImage.Data[y, x, 0];
                    result.Data[y - StangaSusY, x - StangaSusX, 1] = inputImage.Data[y, x, 1];
                    result.Data[y - StangaSusY, x - StangaSusX, 2] = inputImage.Data[y, x, 2];
                }
            }


            return result;
        }

        #endregion

        #region Get Stats
        public static (double Mean, double StdDev) GetStats(Image<Gray, byte> inputImage)
        {
            if (inputImage == null) return (0.0, 0.0);

            int width = inputImage.Width;
            int height = inputImage.Height;
            long totalPixels = (long)width * height;
            if (totalPixels == 0) return (0.0, 0.0);

            double sum = 0.0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    sum += inputImage.Data[y, x, 0];
                }
            }

            double mean = sum / totalPixels;

            double sumOfSquaredDifferences = 0.0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    double pixelValue = inputImage.Data[y, x, 0];
                    double difference = pixelValue - mean;
                    sumOfSquaredDifferences += difference * difference;
                }
            }

            double stdDev = Math.Sqrt(sumOfSquaredDifferences / totalPixels);

            return (mean, stdDev);
        }
        public static (double Mean, double StdDev) GetStats(Image<Bgr, byte> inputImage)
        {
            if (inputImage == null) return (0.0, 0.0);

            int width = inputImage.Width;
            int height = inputImage.Height;
            long totalPixels = (long)width * height;
            if (totalPixels == 0) return (0.0, 0.0);

            double sumOfPixelAverages = 0.0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    double avgPixelValue = (inputImage.Data[y, x, 0] +
                                            inputImage.Data[y, x, 1] +
                                            inputImage.Data[y, x, 2]) / 3.0;

                    sumOfPixelAverages += avgPixelValue;
                }
            }

            double mean = sumOfPixelAverages / totalPixels;

            double sumOfSquaredDifferences = 0.0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    double avgPixelValue = (inputImage.Data[y, x, 0] +
                                            inputImage.Data[y, x, 1] +
                                            inputImage.Data[y, x, 2]) / 3.0;

                    double difference = avgPixelValue - mean;
                    sumOfSquaredDifferences += difference * difference;
                }
            }

            double stdDev = Math.Sqrt(sumOfSquaredDifferences / totalPixels);
            return (mean, stdDev);
        }

        #endregion

        #region Convert color image to grayscale image
        public static Image<Gray, byte> Convert(Image<Bgr, byte> inputImage)
        {
            Image<Gray, byte> result = inputImage.Convert<Gray, byte>();
            return result;
        }
        #endregion
    }
}