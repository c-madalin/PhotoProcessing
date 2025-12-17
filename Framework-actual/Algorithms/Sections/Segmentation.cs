using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing; // Necesar pentru Point, dacă folosim structuri grafice, sau doar Math

namespace Algorithms.Sections
{
    public class Segmentation
    {
        // Funcția principală apelată din ViewModel
        public static Image<Gray, byte> Hough(Image<Gray, byte> inputColor, int threshold)
        {
            // 1. Conversie Bgr -> Gray
            Image<Gray, byte> grayImage = inputColor.Convert<Gray, byte>();
            int width = grayImage.Width;
            int height = grayImage.Height;

            // 2. Detectarea Marginilor (Sobel Manual pentru a obține o imagine binară de contur)
            // Folosim o matrice bool pentru viteză internă, sau putem face un Image<Gray, byte> temporar
            bool[,] edges = new bool[height, width];
            int edgeThreshold = 100; // Prag pentru Sobel

            // Kernel-uri Sobel
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int sumX = 0;
                    int sumY = 0;

                    // Aplicăm Kernel 3x3
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            // Accesăm pixelii imaginii Gray
                            int val = grayImage.Data[y + i, x + j, 0];
                            sumX += val * gx[i + 1, j + 1];
                            sumY += val * gy[i + 1, j + 1];
                        }
                    }

                    int magnitude = (int)Math.Sqrt(sumX * sumX + sumY * sumY);
                    if (magnitude > edgeThreshold)
                    {
                        edges[y, x] = true;
                    }
                }
            }

            // 3. Transformata Hough (Acumulator)
            int maxRho = (int)Math.Sqrt(width * width + height * height);
            int rhoDim = 2 * maxRho;
            int thetaDim = 180;
            int[,] accumulator = new int[rhoDim, thetaDim];

            // Tabele de sin/cos precalculate
            double[] sinTable = new double[thetaDim];
            double[] cosTable = new double[thetaDim];
            for (int t = 0; t < thetaDim; t++)
            {
                double rad = (t * Math.PI) / 180.0;
                sinTable[t] = Math.Sin(rad);
                cosTable[t] = Math.Cos(rad);
            }

            // Votarea în acumulator
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (edges[y, x]) // Dacă este punct de margine
                    {
                        for (int t = 0; t < thetaDim; t++)
                        {
                            int rho = (int)(x * cosTable[t] + y * sinTable[t]);
                            accumulator[rho + maxRho, t]++;
                        }
                    }
                }
            }

            // 4. Desenarea Liniilor
            // Creăm rezultatul pornind de la imaginea grayscale originală
            Image<Gray, byte> result = grayImage.Copy();

            // Prag minim de siguranță
            if (threshold < 10) threshold = 50;

            for (int r = 0; r < rhoDim; r++)
            {
                for (int t = 0; t < thetaDim; t++)
                {
                    if (accumulator[r, t] >= threshold)
                    {
                        // Parametrii liniei
                        int rho = r - maxRho;
                        double cos = cosTable[t];
                        double sin = sinTable[t];

                        // Calculăm coordonate pentru desenare
                        int x0 = (int)(cos * rho);
                        int y0 = (int)(sin * rho);

                        // Extindem linia mult în afara ecranului pentru a părea infinită
                        int hugeLen = Math.Max(width, height) * 2;

                        int x1 = (int)(x0 + hugeLen * (-sin));
                        int y1 = (int)(y0 + hugeLen * (cos));
                        int x2 = (int)(x0 - hugeLen * (-sin));
                        int y2 = (int)(y0 - hugeLen * (cos));

                        // Desenăm linia folosind o funcție helper (stil manual pe Data array)
                        DrawLineOnData(result, x1, y1, x2, y2);
                    }
                }
            }

            return result;
        }

        // Helper pentru desenare linie direct pe matricea de pixeli (Bresenham)
        // 
        private static void DrawLineOnData(Image<Gray, byte> img, int x0, int y0, int x1, int y1)
        {
            int width = img.Width;
            int height = img.Height;

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                // Verificăm limitele imaginii (Clipping)
                if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                {
                    // Setăm pixelul alb (255)
                    img.Data[y0, x0, 0] = 255;
                }

                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }
    }
}