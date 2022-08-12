using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        {
            IEnumerable<string> dirs;
            FileStream file;
            string[] pos = { "defenseempty", "prosecutorempty", "witnessempty", "judgestand" };
            Encoding enc = new UTF8Encoding(false);
            try
            {
                dirs = Directory.EnumerateDirectories(Directory.GetCurrentDirectory() + @"\background");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to obtain the background folder: {0}", e.ToString());
                return;
            }
            foreach (string di in dirs)
            {
                try
                {
                    file = new FileStream(di + @"\design.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create a FileStream: {0}", e.ToString());
                    return;
                }
                using (var writer = new StreamWriter(file, enc))
                {
                    if (((FileStream)writer.BaseStream).Length == 0) // design.ini has been created anew
                    {
                        writer.Write("positions = defenseempty, prosecutorempty, witnessempty, judgestand");
                    }
                    else
                    {
                        string str;
                        List<string> out_list = new List<string>();
                        var reader = new StreamReader(file, enc);
                        while ((str = reader.ReadLine()) != null)
                        {
                            if (str.Contains("positions"))
                            {
                                foreach (var p in pos)
                                {
                                    if (!str.Contains(p))
                                    {
                                        str += ", " + p;
                                    }
                                }
                            }
                            out_list.Add(str);
                        }
                        ((FileStream)writer.BaseStream).Position = 0;
                        for (int i = 0; i < out_list.Count - 1; i++)
                        {
                            writer.WriteLine(out_list[i]);
                        }
                        writer.Write(out_list[out_list.Count - 1]);
                    }
                }
            }
        }
    }
}
