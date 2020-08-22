using System;
using System.IO;
using System.Text;

namespace antCompress
{
    public class FileWrite
    {
        public FileWrite()
        {
        }

        public void WriteBytes(Byte[] content,string path)
        {
            try
            {
                for (int i = 0; i < content.Length; i++)
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Append))
                    {
                        fileStream.WriteByte(content[i]);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public void AppendString(string content, string path)
        {
            try
            {
                // File.WriteAllBytes(path, content);
                File.AppendAllText(path,content);
              
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public void AppendLines(string[] content, string path)
        {
            try
            {
                // File.WriteAllBytes(path, content);
                File.AppendAllLines(path, content);

            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public void Write(string content, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    writer.WriteLine(content);
                }
                // File.WriteAllBytes(path, content);
              //  File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void WriteLines(string[] content, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    foreach (string line in content)
                    {
                        writer.WriteLine(line);
                    }
                }
                // File.WriteAllBytes(path, content);
                //  File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void WriteFile(string content, string path)
        {
            try
            {

                using FileStream fs = File.OpenWrite(path);

                byte[] bytes = Encoding.UTF8.GetBytes(content);

                fs.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                throw e;
            }

        }




        public string Read(string path)
        {
            try
            {
                return File.ReadAllText(path);

            }
            catch (Exception e)
            {
                throw e;
               
            }

        }

        public byte[] ReadBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);

            }
            catch (Exception e)
            {
                throw e;

            }

        }



        public string[] ReadLines(string path)
        {
            try
            {
                return File.ReadAllLines(path);

            }
            catch (Exception e)
            {
                throw e;

            }

        }

    }
}
