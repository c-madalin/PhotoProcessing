using Algorithms.Utilities;
using Emgu.CV;
using Emgu.CV.Structure;

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
    }
}