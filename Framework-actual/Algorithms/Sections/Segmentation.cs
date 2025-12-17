using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Algorithms.Sections
{
    public class Segmentation
    {
        public static Image<Gray, byte> SobelNonDirectional(Image<Gray, byte> image, double threshold)
        {
            float[,] Sx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            float[,] Sy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            int w = image.Width;
            int h = image.Height;

            Image<Gray, byte> result = image.CopyBlank();

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    float gx = 0, gy = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            byte p = image.Data[y + i, x + j, 0];
                            gx += Sx[i + 1, j + 1] * p;
                            gy += Sy[i + 1, j + 1] * p;
                        }
                    }

                    double mag = Math.Sqrt(gx * gx + gy * gy);
                    result.Data[y, x, 0] = (byte)(mag > threshold ? 255 : 0);
                }
            }
            return result;
        }

        public static int[,] BuildHough(Image<Gray, byte> edges)
        {
            int h = edges.Height;
            int w = edges.Width;
            int rhoMax = (int)Math.Sqrt(h * h + w * w);
            int thetaMax = 271; 

            int[,] H = new int[2 * rhoMax + 1, thetaMax];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (edges.Data[y, x, 0] == 255) 
                    {
                        for (int theta = 0; theta < thetaMax; theta++)
                        {
                            double rad = theta * Math.PI / 180.0;
                            int rho = (int)Math.Round(x * Math.Cos(rad) + y * Math.Sin(rad));

                            rho += rhoMax;

                            if (rho >= 0 && rho < 2 * rhoMax + 1)
                                H[rho, theta]++;
                        }
                    }
                }
            }
            return H;
        }

        public static Image<Gray, byte> DisplayHough(int[,] H)
        {
            int r = H.GetLength(0);
            int t = H.GetLength(1);
            Image<Gray, byte> img = new Image<Gray, byte>(t, r);

            int max = 1;
            foreach (int v in H) if (v > max) max = v;

            for (int i = 0; i < r; i++)
                for (int j = 0; j < t; j++)
                    img.Data[i, j, 0] = (byte)(255.0 * H[i, j] / max);

            return img;
        }

        public static List<(int rhoIdx, int theta, int val)> FindLocalMaxima(int[,] H, int threshold, int neighborhoodSize = 5)
        {
            var maxima = new List<(int, int, int)>();
            int rows = H.GetLength(0);
            int cols = H.GetLength(1);
            int half = neighborhoodSize / 2;

            for (int r = half; r < rows - half; r++)
            {
                for (int t = half; t < cols - half; t++)
                {
                    int currentVal = H[r, t];
                    if (currentVal <= threshold) continue;

                    bool isMax = true;
                    for (int dr = -half; dr <= half; dr++)
                    {
                        for (int dt = -half; dt <= half; dt++)
                        {
                            if (dr == 0 && dt == 0) continue;
                            if (H[r + dr, t + dt] >= currentVal)
                            {
                                isMax = false;
                                break;
                            }
                        }
                        if (!isMax) break;
                    }

                    if (isMax)
                        maxima.Add((r, t, currentVal));
                }
            }
            return maxima;
        }

        public static void DrawLinesFromMaxima(Image<Bgr, byte> image, List<(int rhoIdx, int theta, int val)> maxima, int rhoMaxOffset)
        {
            foreach (var item in maxima)
            {
                double thetaRad = item.theta * Math.PI / 180.0;
                double rho = item.rhoIdx - rhoMaxOffset;

                double a = Math.Cos(thetaRad);
                double b = Math.Sin(thetaRad);

                double x0 = a * rho;
                double y0 = b * rho;

                Point p1 = new Point(
                    (int)(x0 + 3000 * (-b)),
                    (int)(y0 + 3000 * (a))
                );

                Point p2 = new Point(
                    (int)(x0 - 3000 * (-b)),
                    (int)(y0 - 3000 * (a))
                );

                image.Draw(new LineSegment2D(p1, p2), new Bgr(Color.Red), 2);
            }
        }
    }
}