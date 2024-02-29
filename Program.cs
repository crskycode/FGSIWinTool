using System;
using System.IO;

namespace FGSIWinTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("FGSIWinTool v1.0");
                Console.WriteLine("  created by Crsky");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  Export text      : FGSIWinTool -e [file|folder]");
                Console.WriteLine("  Rebuild script   : FGSIWinTool -b [file|folder]");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var mode = args[0];
            var path = Path.GetFullPath(args[1]);

            switch (mode)
            {
                case "-e":
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var filePath in Directory.EnumerateFiles(path, "*.script"))
                        {
                            try
                            {
                                Export(filePath);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }

                        foreach (var filePath in Directory.EnumerateFiles(path, "*.lib"))
                        {
                            try
                            {
                                Export(filePath);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    else
                    {
                        Export(path);
                    }

                    break;
                }
                case "-b":
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var filePath in Directory.EnumerateFiles(path, "*.script"))
                        {
                            try
                            {
                                Rebuild(filePath);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }

                        foreach (var filePath in Directory.EnumerateFiles(path, "*.lib"))
                        {
                            try
                            {
                                Rebuild(filePath);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    else
                    {
                        Export(path);
                    }

                    break;
                }
            }
        }

        static void Export(string path)
        {
            Console.WriteLine("Processing {0}", path);

            var txtPath = Path.ChangeExtension(path, ".txt");

            var image = new ScriptFile();
            image.Load(path);
            image.ExportText(txtPath);
        }

        static void Rebuild(string path)
        {
            Console.WriteLine("Processing {0}", path);

            var txtPath = Path.ChangeExtension(path, ".txt");

            var ditPath = Path.GetDirectoryName(path);
            var outDir = Path.Combine(ditPath!, "rebuild");
            var outName = Path.GetFileName(path);
            var newPath = Path.Combine(outDir!, outName);

            Directory.CreateDirectory(outDir);

            var image = new ScriptFile();
            image.Load(path);
            image.ImportText(txtPath);
            image.Save(newPath);
        }
    }
}
