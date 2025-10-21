﻿using Emgu.CV;
using Emgu.CV.Structure;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
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
                    else
                    {
                        result.Data[y, x, 0] = 0;
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
                    result.Data[y, x, 0] = inputImage.Data[y, inputImage.Width - 1 - x, 0];
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

        public static Image<Gray, byte> Rotate(Image<Gray, byte> inputImage, bool direction)
        {
            int newWidth = inputImage.Height;
            int newHeight = inputImage.Width;

            // da frate inertim latimea cu lingimea


            Image<Gray, byte> result = new Image<Gray, byte>(newHeight, newWidth);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    if (direction)
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

        public static Image<Gray, byte> Crop(Image<Gray, byte> inputImage, int StangaSusX, int StangaSusY, int DreaptaJosX, int DreaptaJosY)
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

        #region Contrast and Brightness
        public static Image<Gray, byte> ContrastAndBrisghtness(Image<Gray, byte> inputImage, double alpha, double beta)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            byte[] LUT = new byte[256];
            for (int i = 0; i <= 255; i++)
            {
                //LUT[i] = (byte)(alpha * i + beta);
                LUT[i] = (byte)(Math.Min(255, Math.Max(0, alpha * i + beta)) + 0.5f);

            }

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = LUT[inputImage.Data[y, x, 0]];
                }
            }
            return result;
        }

        public static Image<Bgr, byte> ContrastAndBrisghtness(Image<Bgr, byte> inputImage, double alpha, double beta)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    double newBlue = alpha * inputImage.Data[y, x, 0] + beta;
                    result.Data[y, x, 0] = (byte)Math.Min(255, Math.Max(0, newBlue));

                    double newGreen = alpha * inputImage.Data[y, x, 1] + beta;
                    result.Data[y, x, 1] = (byte)Math.Min(255, Math.Max(0, newGreen));

                    double newRed = alpha * inputImage.Data[y, x, 2] + beta;
                    result.Data[y, x, 2] = (byte)Math.Min(255, Math.Max(0, newRed));
                }
            }
            return result;
        }

        #endregion

        #region Gamma
        public static Image<Gray, byte> Gamma(Image<Gray, byte> inputImage, double gamma)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            byte[] LUT = new byte[256];

            double c = 255.0 / Math.Pow(255, gamma);

            for (int i = 0; i <= 255; i++)
            {
                LUT[i] = (byte)(c * Math.Pow(i, gamma));

            }

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (byte)(LUT[inputImage.Data[y, x, 0]]);
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Gamma(Image<Bgr, byte> inputImage, double gamma)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            byte[] LUT = new byte[256];
            double c = 255.0 / Math.Pow(255, gamma);

            for (int i = 0; i <= 255; i++)
            {
                LUT[i] = (byte)(c * Math.Pow(i, gamma));
            }

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    for (int channel = 0; channel < 3; channel++)
                    {
                        result.Data[y, x, channel] = LUT[inputImage.Data[y, x, channel]];
                    }
                }
            }

            return result;
        }
        #endregion

        #region Convert color image to grayscale image
        public static Image<Gray, byte> Convert(Image<Bgr, byte> inputImage)
        {
            Image<Gray, byte> result = inputImage.Convert<Gray, byte>();
            return result;
        }
        #endregion


        #region MinErr
        public static Image<Gray, byte> MinErr(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            List<Double>histograma= Histogram2(inputImage);

            int Threshold = 0;

            int minError = int.MaxValue;

            double P1 = 0.0;
            double P2 = 0.0;
            double mu1 = 0.0; // suma k * p(k) de la 0 la t-1
            double mu2 = 0.0; // suma k * p(k) de la t la 255

            double total_mu_sum = 0.0;

            for (int k = 0; k < 256; k++)
            {
                total_mu_sum += k * histograma[k];
            }

            for (int t=1;t<254;t++)
            {
                for (int k = 0; k < 256; k++)
                {
                    total_mu_sum += k * histograma[k];
                }
                
                P1 += histograma[t - 1];
                mu1 += (t - 1) * histograma[t - 1];

      
                P2 = 1 - P1;
                mu2 = total_mu_sum - mu1;

                double sigma1 = 0.0;
                for (int k = 0; k <= t - 1; k++)
                {
                    sigma1 += (k - mu1)* (k - mu1) * histograma[k];
                }
                double s1 = sigma1 / P1;


                double sigma2 = 0.0;
                for (int k = t; k < 256; k++)
                {
                    sigma2 += (k - mu2)* (k - mu2) * histograma[k];
                }
                double s2 = sigma2 / P2;


                double currentError = double.MaxValue;
                if (s1 > 0 && s2 > 0)
                {
                    currentError = P1 * Math.Log(s1) + P2 * Math.Log(s2);
                }
                if (currentError < minError)
                {
                    minError = (int)currentError;
                    Threshold = t;
                }


            }



            result = Binar2(inputImage, Threshold);
            return result;
        }

        #endregion


        #region Histogram

        public static List<double> Histogram2(Image<Gray, byte> inputImage)
        {

            //hiistograma normalizata
            List<double> histogram = new List<double>(new double[256]);

            if (inputImage == null)
                return histogram;

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    byte pixelValue = inputImage.Data[y, x, 0];
                    histogram[pixelValue]++;
                }
            }

            double totalPixels = inputImage.Width * inputImage.Height;
            if (totalPixels > 0)
            {
                for (int i = 0; i < histogram.Count; i++)
                {
                    histogram[i] /= totalPixels;
                }
            }

            return histogram;
        }

        public static List<double> Histogram1(Image<Gray, byte> inputImage)
        {

            //hiistograma nenormalizata
            List<double> histogram = new List<double>(new double[256]);

            if (inputImage == null)
                return histogram;

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    byte pixelValue = inputImage.Data[y, x, 0];
                    histogram[pixelValue]++;
                }
            }

            return histogram;
        }


        public static List<double> Histogram(Image<Bgr, byte> inputImage)
        {
            List<double> histogram = new List<double>(new double[256]);
            


            return histogram;
        }

        #endregion


    }
}

