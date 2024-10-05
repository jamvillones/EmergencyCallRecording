using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.Utilities {
    internal static class ImageHelper {
        public static bool ChooseImage(out ImageSource? source) {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension             
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|" +
                         "JPEG Files (*.jpeg)|*.jpeg";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {
                // Open document 
                string filename = dlg.FileName;
                //extension = Path.GetExtension(dlg.FileName);
                source = new BitmapImage(new Uri(filename));
                return true;
            }
            source = null;
            return false;
        }

        public static ImageSource? ToImageSource(this string path) {
            if (path is null) return null;
            Uri img = new Uri(@"pack://application:,,," + path);
            return new BitmapImage(img);
        }

        public static byte[]? ToByteArray<T>(this ImageSource? imageSource) where T : BitmapEncoder, new() {
            byte[]? bytes = null;

            if (imageSource != null) {
                var bitmapSource = imageSource as BitmapSource;
                var encoder = new T();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream()) {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }
        public static byte[]? ToByteArray(this ImageSource? imageSource) {
            byte[]? bytes = null;

            if (imageSource != null) {
                var bitmapSource = imageSource as BitmapSource;
                var encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream()) {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static ImageSource? ToImageSource(this byte[]? byteArrayIn) {
            if (byteArrayIn == null)
                return null;

            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            biImg.Freeze();

            return biImg;
        }
    }
}
