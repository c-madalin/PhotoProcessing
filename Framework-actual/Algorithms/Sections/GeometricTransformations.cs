using Algorithms.Utilities;
using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace Algorithms.Sections
{
    public class GeometricTransformations
    {
  
        public static double LiniarInterpolate(double x, double f0, double f1)
        {
            double fraction = x - Math.Floor(x);
            return fraction * f1 + (1 - fraction) * f0;
        }

        public static double BilinearInterpolate(double x, double y, byte[] f)
        {
            double p0 = LiniarInterpolate(x, f[0], f[1]);
            double p1 = LiniarInterpolate(x, f[2], f[3]);
            return LiniarInterpolate(y, p0, p1);
        }

        public static double CubicInterpolate(double x, double fm1, double f0, double f1, double f2)
        {
            double fraction = x - Math.Floor(x);
            double fraction2 = fraction * fraction;
            double fraction3 = fraction2 * fraction;

            double a = -fraction3 + 2 * fraction2 - fraction;
            double b = 3 * fraction3 - 5 * fraction2 + 2;
            double c = -3 * fraction3 + 4 * fraction2 + fraction;
            double d = fraction3 - fraction2;

            return 0.5 * (a * fm1 + b * f0 + c * f1 + d * f2);
        }

        public static double BicubicInterpolate(double x, double y, double[,] f)
        {
            double[] p = new double[4];
            for (int i = 0; i < 4; i++)
            {
                p[i] = CubicInterpolate(x, f[i, 0], f[i, 1], f[i, 2], f[i, 3]);
            }
            return CubicInterpolate(y, p[0], p[1], p[2], p[3]);
        }

        public static Image<Gray, byte> InterpolateBilin(Image<Gray, byte> image, double sx, double sy)
        {
            int newWidth = (int)(image.Width * sx);
            int newHeight = (int)(image.Height * sy);
            var result = new Image<Gray, byte>(newWidth, newHeight);

            byte[] f = new byte[4];

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    double xc = x / sx;
                    double yc = y / sy;

                    int x0 = (int)xc;
                    int y0 = (int)yc;

                    if (x0 >= 0 && x0 < image.Width - 1 && y0 >= 0 && y0 < image.Height - 1)
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            for (int j = 0; j <= 1; j++)
                            {
                                f[i * 2 + j] = image.Data[y0 + i, x0 + j, 0];
                            }
                        }
                        double val = BilinearInterpolate(xc, yc, f);
                        result.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, val + 0.5));
                    }
                }
            }
            return result;
        }

        public static Image<Bgr, byte> InterpolateBilin(Image<Bgr, byte> image, double sx, double sy)
        {
            int newWidth = (int)(image.Width * sx);
            int newHeight = (int)(image.Height * sy);
            var result = new Image<Bgr, byte>(newWidth, newHeight);

            byte[] f = new byte[4];

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    double xc = x / sx;
                    double yc = y / sy;

                    int x0 = (int)xc;
                    int y0 = (int)yc;

                    if (x0 >= 0 && x0 < image.Width - 1 && y0 >= 0 && y0 < image.Height - 1)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            for (int i = 0; i <= 1; i++)
                            {
                                for (int j = 0; j <= 1; j++)
                                {
                                    f[i * 2 + j] = image.Data[y0 + i, x0 + j, c];
                                }
                            }
                            double val = BilinearInterpolate(xc, yc, f);
                            result.Data[y, x, c] = (byte)Math.Max(0, Math.Min(255, val + 0.5));
                        }
                    }
                }
            }
            return result;
        }

    
        public static Image<Gray, byte> InterpolateBicubic(Image<Gray, byte> image, double sx, double sy)
        {
            int newWidth = (int)(image.Width * sx);
            int newHeight = (int)(image.Height * sy);
            var result = new Image<Gray, byte>(newWidth, newHeight);

            double[,] f = new double[4, 4];

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    double xc = x / sx;
                    double yc = y / sy;

                    int x0 = (int)xc;
                    int y0 = (int)yc;

                    if (x0 >= 1 && x0 < image.Width - 2 && y0 >= 1 && y0 < image.Height - 2)
                    {
                        for (int i = -1; i <= 2; i++)
                        {
                            for (int j = -1; j <= 2; j++)
                            {
                                f[i + 1, j + 1] = image.Data[y0 + i, x0 + j, 0];
                            }
                        }

                        double val = BicubicInterpolate(xc, yc, f);
                        result.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, val + 0.5));
                    }
                }
            }
            return result;
        }

        public static Image<Bgr, byte> InterpolateBicubic(Image<Bgr, byte> image, double sx, double sy)
        {
            int newWidth = (int)(image.Width * sx);
            int newHeight = (int)(image.Height * sy);
            var result = new Image<Bgr, byte>(newWidth, newHeight);

            double[,] f = new double[4, 4];

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    double xc = x / sx;
                    double yc = y / sy;

                    int x0 = (int)xc;
                    int y0 = (int)yc;

                    if (x0 >= 1 && x0 < image.Width - 2 && y0 >= 1 && y0 < image.Height - 2)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            for (int i = -1; i <= 2; i++)
                            {
                                for (int j = -1; j <= 2; j++)
                                {
                                    f[i + 1, j + 1] = image.Data[y0 + i, x0 + j, c];
                                }
                            }

                            double val = BicubicInterpolate(xc, yc, f);
                            result.Data[y, x, c] = (byte)Math.Max(0, Math.Min(255, val + 0.5));
                        }
                    }
                }
            }
            return result;
        }
    }
}