using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

public namespace SpecialClasses
{
    public class FileWorker
    {
        private string path;

        public FileWorker(string path)
        {
            this.path = path;
            try
            {
                fileWorker = new FileWorker(path);
            }
            catch (FileNotFoundException ex)
            {
                File.AppendText(@"C:\ProgramFiles\Errors.txt").Write("\n" + ex.Message);
            }
        }

        public List<string> readFromFile()
        {
            try
            {
                vs.AddRange(File.ReadAllLines(path));
            }
            catch(FileNotFoundException ex)
            {
                File.AppendText(@"C:\ProgramFiles\Errors.txt").Write("\n" + ex.Message);
            }
            return vs;
        }

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
