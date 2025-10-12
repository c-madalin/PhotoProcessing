using Emgu.CV;
using Emgu.CV.Structure;

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


            Image<Bgr, byte> result = new Image<Bgr, byte>(newHeight,newWidth);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    if(direction)
                    {
                        result.Data[x, newWidth - 1 - y, 0] = inputImage.Data[x, y, 0];
                        result.Data[x, newWidth - 1 - y, 1] = inputImage.Data[x, y, 1];
                        result.Data[x, newWidth - 1 - y, 2] = inputImage.Data[x, y, 2];
                    }
                    else
                    {
                        result.Data[newHeight - 1 - x, y, 0] = inputImage.Data[x, y, 0];
                        result.Data[newHeight - 1 - x, y, 1] = inputImage.Data[x, y, 1];
                        result.Data[newHeight - 1 - x, y, 2] = inputImage.Data[x, y, 2];

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
    }
}