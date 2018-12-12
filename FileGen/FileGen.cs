using System;
using System.IO;
using System.Linq;
using System.Text;
using Markov;

namespace FileGen
{
    public sealed class FileGen
    {
        static void Main(string[] args)
        {
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = new DirectoryInfo(docs);
            var chain = new MarkovChain<string>(3);
            var nameChain = new MarkovChain<char>(3);

            foreach (var file in dir.EnumerateFiles())
            {
                chain.Add(File.ReadAllText(file.FullName).Split(' '));
                nameChain.Add(file.Name);
            }

            var rand = new Random();

            Directory.CreateDirectory(docs + "\\ImportantFiles");

            for (int i = 0; i < 10; i++)
            {

                var filePath = (docs + "\\ImportantFiles\\" + new String(nameChain.Chain(rand).ToArray()));

                Console.WriteLine(filePath);

                using (FileStream fs = File.Create(filePath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(chain.Chain(rand).Aggregate((a, b) => a + " " + b));
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
