using Algorithms.Utilities;
using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace Algorithms.Sections
{
    public class MorphologicalOperations
    {
        public static Image<Gray, byte> Dilation(Image<Gray, byte> grayInitialImage, int height, int width, int threshold, bool option)
        {
            Image<Gray, byte> binaryImage = Utils.Binarization(grayInitialImage, threshold, option);

            Image<Gray, byte> result = new Image<Gray, byte>(grayInitialImage.Size);

            float[,] structuringElement = new float[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    structuringElement[j, i] = 1;
                }
            }

            int offsetY = height / 2;
            int offsetX = width / 2;

            for (int y = 0; y < binaryImage.Height; y++)
            {
                for (int x = 0; x < binaryImage.Width; x++)
                {
                    bool shouldDilate = false;
                    for (int j = -offsetY; j <= offsetY; j++)
                    {
                        for (int i = -offsetX; i <= offsetX; i++)
                        {
                            int neighborY = y + j;
                            int neighborX = x + i;
                            if (neighborY >= 0 && neighborY < binaryImage.Height &&
                                neighborX >= 0 && neighborX < binaryImage.Width)
                            {
                                if (structuringElement[j + offsetY, i + offsetX] == 1 &&
                                    binaryImage.Data[neighborY, neighborX, 0] == 255)
                                {
                                    shouldDilate = true;
                                    break;
                                }
                            }
                        }
                        if (shouldDilate)
                            break;
                    }
                    result.Data[y, x, 0] = shouldDilate ? (byte)255 : (byte)0;
                }
            }
            return result;

        }
        public static Image<Gray, byte> Erosion(Image<Gray, byte> grayInitialImage, int height, int width, int threshold, bool option)
        {
            Image<Gray, byte> binaryImage = Utils.Binarization(grayInitialImage, threshold, option);
            Image<Gray, byte> result = new Image<Gray, byte>(grayInitialImage.Size);

            float[,] structuringElement = new float[height, width];
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    structuringElement[j, i] = 1;

            int offsetY = height / 2;
            int offsetX = width / 2;

            for (int y = 0; y < binaryImage.Height; y++)
            {
                for (int x = 0; x < binaryImage.Width; x++)
                {
                    bool shouldKeep = true; 

                    for (int j = -offsetY; j <= offsetY; j++)
                    {
                        for (int i = -offsetX; i <= offsetX; i++)
                        {
                            int neighborY = y + j;
                            int neighborX = x + i;

                            if (structuringElement[j + offsetY, i + offsetX] == 1)
                            {
                                if (neighborY >= 0 && neighborY < binaryImage.Height &&
                                    neighborX >= 0 && neighborX < binaryImage.Width)
                                {
                                    if (binaryImage.Data[neighborY, neighborX, 0] != 255)
                                    {
                                        shouldKeep = false;
                                        break;
                                    }
                                }
                                else
                                {
                                   
                                    shouldKeep = false;
                                    break;
                                }
                            }
                        }
                        if (!shouldKeep) break;
                    }
                    result.Data[y, x, 0] = shouldKeep ? (byte)255 : (byte)0;
                }
            }
            return result;
        }

        public static Image<Gray, byte> Opening(Image<Gray, byte> image, int height, int width, int threshold, bool option)
        {
            Image<Gray, byte> eroded = Erosion(image, height, width, threshold, option);

        
            return Dilation(eroded, height, width, 127, true);
        }

        public static Image<Gray, byte> Closing(Image<Gray, byte> image, int height, int width, int threshold, bool option)
        {
            Image<Gray, byte> dilated = Dilation(image, height, width, threshold, option);

            return Erosion(dilated, height, width, 127, true);
        }


        private static double GetNormL2(Bgr pixel)
        {
            return Math.Sqrt(Math.Pow(pixel.Blue, 2) + Math.Pow(pixel.Green, 2) + Math.Pow(pixel.Red, 2));
        }
        public static Image<Bgr, byte> ColorDilation(Image<Bgr, byte> inputImage, int height, int width)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);
            int offsetY = height / 2;
            int offsetX = width / 2;

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    double maxNorm = -1;
                    Bgr maxPixel = inputImage[y, x];

                    for (int j = -offsetY; j <= offsetY; j++)
                    {
                        for (int i = -offsetX; i <= offsetX; i++)
                        {
                            int neighborY = y + j;
                            int neighborX = x + i;

                            if (neighborY >= 0 && neighborY < inputImage.Height &&
                                neighborX >= 0 && neighborX < inputImage.Width)
                            {
                                Bgr currentPixel = inputImage[neighborY, neighborX];
                                double currentNorm = GetNormL2(currentPixel);

                                if (currentNorm > maxNorm)
                                {
                                    maxNorm = currentNorm;
                                    maxPixel = currentPixel;
                                }
                            }
                        }
                    }
                    result[y, x] = maxPixel;
                }
            }
            return result;
        }

        public static Image<Bgr, byte> ColorErosion(Image<Bgr, byte> inputImage, int height, int width)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);
            int offsetY = height / 2;
            int offsetX = width / 2;

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    double minNorm = double.MaxValue;
                    Bgr minPixel = inputImage[y, x];

                    for (int j = -offsetY; j <= offsetY; j++)
                    {
                        for (int i = -offsetX; i <= offsetX; i++)
                        {
                            int neighborY = y + j;
                            int neighborX = x + i;

                            if (neighborY >= 0 && neighborY < inputImage.Height &&
                                neighborX >= 0 && neighborX < inputImage.Width)
                            {
                                Bgr currentPixel = inputImage[neighborY, neighborX];
                                double currentNorm = GetNormL2(currentPixel);

                                if (currentNorm < minNorm)
                                {
                                    minNorm = currentNorm;
                                    minPixel = currentPixel;
                                }
                            }
                        }
                    }
                    result[y, x] = minPixel;
                }
            }
            return result;
        }

        public static Image<Bgr, byte> ColorOpening(Image<Bgr, byte> colorInitialImage, int height, int width)
        {
            Image<Bgr, byte> eroded = ColorErosion(colorInitialImage, height, width);
            return ColorDilation(eroded, height, width);
        }

        public static Image<Bgr, byte> ColorClosing(Image<Bgr, byte> colorInitialImage, int height, int width)
        {
            Image<Bgr, byte> dilated = ColorDilation(colorInitialImage, height, width);
            return ColorErosion(dilated, height, width);
        }
    }
}
