using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks;

namespace Algorithms.Utilities
{
    public class Utils
    {
        #region Compute histogram
        public static int[] ComputeHistogram(Image<Gray, byte> inputImage)
        {
            int[] histogram = new int[256];

            Parallel.For(0, inputImage.Height, (int y) =>
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    ++histogram[inputImage.Data[y, x, 0]];
                }
            });

            return histogram;
        }
        internal static Image<Gray, byte> Binarization(Image<Gray, byte> grayInitialImage, int threshold, bool option)
        {
            Image<Gray, byte> binaryImage = new Image<Gray, byte>(grayInitialImage.Size);
            for (int y = 0; y < grayInitialImage.Height; y++)
            {
                for (int x = 0; x < grayInitialImage.Width; x++)
                {
                    byte pixelValue = grayInitialImage.Data[y, x, 0];
                    if (option)
                    {
                        binaryImage.Data[y, x, 0] = (pixelValue >= threshold) ? (byte)255 : (byte)0;
                    }
                    else
                    {
                        binaryImage.Data[y, x, 0] = (pixelValue <= threshold) ? (byte)255 : (byte)0;
                    }
                }
            }
            return binaryImage;
        }
        #endregion
    }
}