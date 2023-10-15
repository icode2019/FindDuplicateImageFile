using System.Security.Cryptography;

namespace FindDuplicateImageFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // First we need to declare a variable for the images folder path.
            string imagesFolderPath = @"C:\Users\icode\Pictures\Source"; // change as needed

            // Then we need to get all the file paths from the images folder.
            string[] imageFileList = Directory.GetFiles(imagesFolderPath);

            // Check if the list is empty, if so we exit this method.
            if (imageFileList.Length == 0)
            {
                Console.WriteLine("No files in the directory");
                return;
            }

            // Get the file hashes
            var fileHashes = GetFileHashes(imageFileList);

            // Check for duplicates
            FindDuplicate(fileHashes);
        }


        private static Dictionary<string, List<string>> GetFileHashes(string[] imageFileList)
        {
            var hashToFilePaths = new Dictionary<string, List<string>>();

            // We then loop through each file in the list.
            foreach (var filePath in imageFileList)
            {
                // Here we calculate file hash value for this file
                string fileHash = CalculateFileHash(filePath);

                // Now we check if file hash already exists
                if (!hashToFilePaths.ContainsKey(fileHash))
                {
                    hashToFilePaths[fileHash] = new List<string>();
                }

                // Add file hash to the dictionary
                hashToFilePaths[fileHash].Add(filePath);

                Console.WriteLine("Processing file: " + filePath);
            }

            // Return all the file hashes
            return hashToFilePaths;
        }

        public static string CalculateFileHash(string filePath)
        {
            // We calculate the file hash using MD5 hashing from System.Security.Cryptography.
            using (var md5 = MD5.Create())
            {
                // We then open the image file as a stream.
                using (var stream = File.OpenRead(filePath))
                {
                    // And then calculate the hash value for the image.
                    byte[] hash = md5.ComputeHash(stream);

                    // We then convert this hash value into string.
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        public static void FindDuplicate(Dictionary<string, List<string>> fileHashes)
        {
            // Loop through each hash entry in the dictionary
            foreach (var hashEntry in fileHashes)
            {
                // Check if hasEntry value count is greater than
                if (hashEntry.Value.Count > 1)
                {
                    // Loop through each hash value
                    for (int i = 1; i < hashEntry.Value.Count; i++)
                    {
                        // Get the file path for this hash
                        string duplicateFilePath = hashEntry.Value[i];

                        // We then create a new file using this path adding the text _duplicate
                        string newFilePath = Path.Combine(
                            Path.GetDirectoryName(duplicateFilePath),
                            Path.GetFileNameWithoutExtension(duplicateFilePath)
                            + "_duplicate" + Path.GetExtension(duplicateFilePath)
                        );

                        // Here you can either delete or rename the duplicate
                        File.Move(duplicateFilePath, newFilePath);
                    }
                }
            }
        }
    }
}