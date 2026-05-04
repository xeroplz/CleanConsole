using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleApp
{
    class Startup(
        IConfiguration configuration)
    {
        IConfiguration Configuration { get; } = configuration;

        public void Run(string[] args)
        {
            Console.WriteLine("This program will convert an mp3 to a mabinogi resource file, or do a backwards conversion.");

            if (args.Length < 1)
			{
				Console.WriteLine("Usage: rsvrconv <file_name.mp3 | file_name>");
				Console.WriteLine("Press [Enter] to exit.");
				Console.ReadLine();
				return;
			}

			Console.WriteLine("rsvrconv v1.0.0");
			Console.WriteLine();
			Console.WriteLine("Files: {0}", args.Length);
			Console.WriteLine("Let's begin!");

			int i = 0;
			foreach (var arg in args)
			{
				Console.Write("{0}/{1}\r", i++, args.Length);

				if (!File.Exists(arg))
				{
					Console.WriteLine("Not a valid file: {0}", Path.GetFileName(arg));
					continue;
				}

                bool isMp3 = Path.GetExtension(arg) == ".mp3";

				try
				{
                    if (isMp3) {
                        using (var inf = new FileStream(arg, FileMode.Open))
                        using (var outf = new FileStream(Path.ChangeExtension(arg, null), FileMode.Create))
                        {
                            while (inf.Position < inf.Length)
                            {
                                var ob = inf.ReadByte();
                                var b = (byte)Math.Floor(ob / 2f);
                                if (ob % 2 != 0)
                                    b += 128;

                                outf.WriteByte(b);
                            }
                        }
                    }
                    else
                    {
                        using (var inf = new FileStream(arg, FileMode.Open))
                        using (var outf = new FileStream(Path.ChangeExtension(arg, ".mp3"), FileMode.Create))
                        {
                            int rb;
                            while ((rb = inf.ReadByte()) != -1)
                            {
                                byte b = (byte)rb;
                                byte ob;

                                if (b < 128)
                                {
                                    // original was even
                                    ob = (byte)(b * 2);
                                }
                                else
                                {
                                    // original was odd
                                    ob = (byte)((b - 128) * 2 + 1);
                                }

                                outf.WriteByte(ob);
                            }
                        }

                    }
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error while processing '{0}': {1}", arg, ex.Message);
				}
			}

			Console.WriteLine("Done. Press [Enter] to exit.");
			Console.ReadLine();
        }
    }
}
