using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Core.SpecialClasses
{
    public class FileWorker
    {
        private string path;

        public static List<string> ReadFromFile(string fileName)
        {
            try
            {
                List<string> returnsString = new List<string>();
                returnsString.AddRange(File.ReadAllLines(fileName));
                return returnsString;
            }
            catch (FileNotFoundException ex)
            {
                File.AppendText(@"C:\ProgramFiles\Errors.txt").Write("\n" + ex.Message);
                MessageBox.Show(ex.Message);
                return new List<string>();
            }
        }

        public static bool WriteToFile(string path, string[] array)
        {
            try
            {
                File.WriteAllLines(path, array);
                return true;
            }
            catch(FileNotFoundException)
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                fs.Close();
                File.WriteAllLines(path, array);
                return true;
            }
            catch (IOException ex)
            {
                File.AppendText(@"C:\ProgramFiles\Errors.txt").Write("\n" + ex.Message);
                return false;
            }
        }

        public static bool WriteToFile(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content + "\n");
                return true;
            }
            catch(IOException ex)
            {
                File.AppendText(@"C:\ProgramFiles\Errors.txt").Write("\n" + ex.Message);
                return false;
            }
        }

        //public static bool WriteToFile(string path, List<string> list)
        //{

        //}
    }
}
