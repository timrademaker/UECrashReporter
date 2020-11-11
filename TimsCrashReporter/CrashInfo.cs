using System;
using System.Linq;
using System.IO;

namespace TimsCrashReporter
{
    class CrashInfo
    {
        public string m_LogContent = String.Empty;
        public string m_XmlContent = String.Empty;
        public byte[] m_MiniDump = new byte[0];


        static string s_AppName { get; set; }

        public static CrashInfo GetCrashInfo()
        {
            // Check if crash data can be found
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += "\\" + s_AppName + "\\Saved\\Crashes";

            DirectoryInfo crashDir = new DirectoryInfo(path);
            if (!crashDir.Exists)
            {
                return null;
            }

            var info = new CrashInfo();

            crashDir.Refresh();

            // Get the newest folder in the crash directory
            var dir = crashDir.GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();
            dir.Refresh();

            FileInfo[] fileBuffer;

            // Get the log file
            fileBuffer = dir.GetFiles("*.log");
            if(fileBuffer.Length > 0)
            {
                FileInfo logFile = fileBuffer.First();
                info.m_LogContent = File.ReadAllText(logFile.FullName);
            }


            // Get the xml log file
            fileBuffer = dir.GetFiles("*.runtime-xml");
            if (fileBuffer.Length > 0)
            {
                FileInfo xmlFile = fileBuffer.First();
                info.m_XmlContent = File.ReadAllText(xmlFile.FullName);
            }

            // Get the minidump
            fileBuffer = dir.GetFiles("*.dmp");
            if (fileBuffer.Length > 0)
            {
                FileInfo dumpFile = fileBuffer.First();
                info.m_MiniDump = File.ReadAllBytes(dumpFile.FullName);
                
                // Compress dump
                info.m_MiniDump = Compressor.Compress(info.m_MiniDump);
            }

            return info;
        }
    }
}
