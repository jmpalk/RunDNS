using System;
using System.Net;
using System.IO;

namespace RunDNS
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] targetList = null;
            string previousArgument = "";
            string target = "";
            string inputFile = "";


            foreach (string arg in args)
            {
                if (arg == "-l")
                {
                    previousArgument = "-l";
                }
                else if (arg == "-t")
                {
                    previousArgument = "-t";
                }
                else
                {
                    if (previousArgument == "-l")
                    {
                        inputFile = arg;
                        previousArgument = "";
                    }
                    else if (previousArgument == "-t")
                    {
                        target = arg;
                        previousArgument = "";
                    }
                    else
                    {
                        Console.WriteLine("Argument '" + arg + "' is unrecognized.");
                        return;
                    }
                }//end else
            }// end foreach(string arg in args)

            if (inputFile != "")
            {
                if (File.Exists(inputFile))
                {
                    targetList = System.IO.File.ReadAllLines(inputFile);
                }
                else
                {
                    Console.WriteLine("File '" + inputFile + "' does not exist or is not readable. Exiting");
                }
            }

            StreamWriter outputFile = new StreamWriter("rundns.out", append: true);
            IPHostEntry hostInfo = null;
            if (target != "")
            {
                
                try
                {
                    hostInfo = Dns.GetHostEntry(target);
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine(target + ",NOTFOUND");
                    outputFile.Flush();
                    Console.WriteLine("Execution Finished");
                    return;

                }
                foreach (IPAddress address in hostInfo.AddressList)
                {
                    outputFile.WriteLine(target + "," + address.ToString());
                }
                outputFile.Flush();
                Console.WriteLine("Execution Finished");
                return;
            }

            foreach (string host in targetList)
            {
                try
                {
                    hostInfo = Dns.GetHostEntry(host);
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine(host + ",NOTFOUND");
                    outputFile.Flush();
                    continue;
                }
                foreach (IPAddress address in hostInfo.AddressList)
                {
                    outputFile.WriteLine(host + "," + address.ToString());
                }
                outputFile.Flush();
            }//end  foreach (string host in targetList)
            Console.WriteLine("Execution Finished");
        }//end Main(...

    }//end class Program
}//end namespace ShareScanner
