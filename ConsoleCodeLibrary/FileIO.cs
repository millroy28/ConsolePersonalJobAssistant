using System.Collections.Generic;
using System.IO;



namespace ConsoleCodeLibrary
{
    class FileIO
    {
        public static List<string> GetFile(string directory, string fileName)
        {
            StreamReader reader = new StreamReader($"../../../{directory}/{fileName}.txt");
            string line = reader.ReadLine();
            List<string> fileContents = new List<string>();
            do
            {
                fileContents.Add(line);
                line = reader.ReadLine();
            } while (line != null);
            reader.Close();
            return fileContents;
        }
        public static List<string> GetFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            List<string> fileContents = new List<string>();
            do
            {
                fileContents.Add(line);
                line = reader.ReadLine();
            } while (line != null);
            reader.Close();
            return fileContents;
        }


        public static string GetFirstLine(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            reader.Close();
            return line;
        }

        public static string[] GetFileList(string directory)
        {
            string[] fileList = Directory.GetFiles($"../../../{directory}/");
            return fileList;
        }

        public static void CategoryDirectoryCheck(string category)
        {
            if (!Directory.Exists($"../../../{category}"))
            {
                CreateCategoryDirectory(category);
            }
        }

        public static void CreateCategoryDirectory(string category)
        {
            Directory.CreateDirectory($"../../../{category}");
            File.Copy("../../../config/COLORDEF.txt", $"../../../config/{category}.txt");
        }
    }
}
