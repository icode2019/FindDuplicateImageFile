
using ImageMagick;

namespace ConvertImageFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Declare some variables
            string imagesFolderPath = @"C:\Users\icode\Pictures\Source"; // change path 
            string outputImagesFolderPath = @"C:\Users\icode\Pictures\target"; // change path

            // Set the file type you want to convert from.
            string convertFromFileType = "*.jpg";

            // Set the file type you want to convert to.
            string convertToFileType = ".png";

            // Now get all the file paths from the source folder.
            string[] imageFileList = Directory.GetFiles(imagesFolderPath, convertFromFileType);

            // Check if imageFiles list is empty, if so exist this method.
            if (imageFileList.Length == 0)
            {
                Console.WriteLine("No files in the directory");
                return;
            }

            // Check if target folder exists, if not create it.
            if (!Directory.Exists(outputImagesFolderPath))
            {
                Directory.CreateDirectory(outputImagesFolderPath);
            }

            // Create a dictionary of the supported file types.
            // Here add the image formats you need.
            Dictionary<string, MagickFormat> formatMappings = new Dictionary<string, MagickFormat>
            {
               { ".jpg", MagickFormat.Jpg },
               { ".png", MagickFormat.Png },
               { ".bmp", MagickFormat.Bmp }
            };

            // Loop through each image file in the list
            foreach (string imageFilePath in imageFileList)
            {
                // Wrap the image magik inside using statment.
                using (MagickImage image = new MagickImage(imageFilePath))
                {
                    // Create converted image file path 
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);
                    string newImageTypeFilePath = Path.Combine(
                                            outputImagesFolderPath, 
                                            $"{fileNameWithoutExtension}{convertToFileType}");

                    // Check the dictionary for the selected image type.
                    if (formatMappings.TryGetValue(convertToFileType, out MagickFormat format))
                    {
                        // Convert the image and save the file
                        image.Write(newImageTypeFilePath, format);
                        Console.WriteLine("Processing file: " + newImageTypeFilePath);
                    }
                    else
                    {
                        Console.WriteLine("Unsupported file format: " + convertToFileType);
                    }
                }
            }

            Console.WriteLine("Conversion completed.");
        }
    }
}