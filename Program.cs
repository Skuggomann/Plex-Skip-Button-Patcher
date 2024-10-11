using System;
using System.IO;
using System.Diagnostics;


// SRI Hashes are generated like so: openssl dgst -sha384 -binary "{MainJSFileLocation}" | openssl base64 -A

namespace PlexSkipButtonPatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set default skip times
            int skipBackward = 5;
            int skipForward = 5;

            // Check if there are custom skip times sent as input
            if (args.Length >= 2) {
                skipBackward = int.Parse(args[0]);
                skipForward = int.Parse(args[1]);
            } else if (args.Length >= 1) {
                skipBackward = int.Parse(args[0]);
                skipForward = skipBackward;
            }

            // Find the main-*.js file that contains the code that controls the skip buttons
            string MainJSFileLocation = FindScriptLocation();

            Console.WriteLine($"Patching file: {MainJSFileLocation}");

            File.WriteAllText(MainJSFileLocation, File.ReadAllText(MainJSFileLocation)
                .Replace(",i=10,s=30,", $",i={skipBackward},s={skipForward},")
                .Replace(",a=10,s=30,", $",a={skipBackward},s={skipForward},") // Not sure if this is needed
                .Replace("Skip Back 10 Seconds", $"Skip Backward {skipBackward} Seconds")
                .Replace("Skip Forward 30 Seconds", $"Skip Forward {skipForward} Seconds")
                .Replace("Seek Backward (10 seconds)", $"Seek Backward ({skipBackward} seconds)")   // No idea where this text is
                .Replace("Seek Forward (30 seconds)", $"Seek Forward ({skipForward} seconds)")      // No idea where this text is
            );

            // Find the index.html file that contains the script references with integrity checks
            string IndexFileLocation = FindIndexLocation();
            string MainJSFileName = MainJSFileLocation.Split('\\').Last();

            Console.WriteLine($"Patching file: {IndexFileLocation}");

            File.WriteAllText(IndexFileLocation, File.ReadAllText(IndexFileLocation)
                .Replace($"<script src=\"js/{MainJSFileName}\" integrity=\"", $"<script src=\"js/{MainJSFileName}\" NOthatsabadplex=\"")
            );


            Console.WriteLine($"Skip backward set to {skipBackward} seconds");
            Console.WriteLine($"Skip forward set to {skipForward} seconds");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static string FindScriptLocation()
        {
            return FindFileLocation(@"js\main-*-plex-*.js");
        }

        private static string FindIndexLocation()
        {
            return FindFileLocation(@"index.html");
        }

        private static string FindFileLocation(string filename)
        {
            // List of all folders to search for the plex main-*.js file
            List<string> Folders = new List<string> {
                @"C:\Program Files\Plex\Plex\web-client",
                @"C:\Program Files (x86)\Plex\Plex\web-client",
            };

            foreach (string Folder in Folders)
            {
                if (Directory.Exists(Folder))
                {
                    // Search for the main js file for the Plex web-client
                    var FilesFound = Directory.GetFiles(Folder, filename);

                    if (FilesFound.Length == 1)
                    {
                        return FilesFound[0];
                    }
                    else if (FilesFound.Length >= 2)
                    {
                        throw new FileNotFoundException($"Too many files found, was expecting '1' found '{FilesFound.Length}'");
                    }
                }
            }

            throw new FileNotFoundException();
        }
    }
}
