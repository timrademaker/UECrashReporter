using System;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace TimsCrashReporter
{
    class CrashInfo
    {
        public string m_LogContent = string.Empty;
        public string m_XmlContent = string.Empty;
        public byte[] m_MiniDump = new byte[0];


        public static string s_AppName { get; set; } = string.Empty;
        public static string s_CrashReportLocation { get; set; } = string.Empty;

        public static CrashInfo GetCrashInfo(bool a_IncludeLog)
        {
            // Check if crash data can be found
            DirectoryInfo crashDir = null;
            if(s_CrashReportLocation != string.Empty)
            {
                crashDir = new DirectoryInfo(s_CrashReportLocation);
                crashDir.Refresh();
            }

            var dir = crashDir;

            if (crashDir != null && !crashDir.Exists)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                path += "\\" + s_AppName + "\\Saved\\Crashes";
                crashDir = new DirectoryInfo(path);
                
                if (!crashDir.Exists)
                {
                    return null;
                }
                else
                {
                    // Get the newest folder in the crash directory
                    dir = crashDir.GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();
                }
            }

            var info = new CrashInfo();
            
            dir.Refresh();

            FileInfo[] fileBuffer;

            if(a_IncludeLog)
            {
                // Get the log file
                fileBuffer = dir.GetFiles("*.log");
                if (fileBuffer.Length > 0)
                {
                    FileInfo logFile = fileBuffer.First();
                    info.m_LogContent = File.ReadAllText(logFile.FullName);
                }
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
            }

            return info;
        }

        public byte[] ZipCrashInfo()
        {
            using(var memoryStream = new MemoryStream())
            {
                using(var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // Add log content
                    if(m_LogContent != string.Empty)
                    {
                        var logFile = archive.CreateEntry("Crash.log");

                        using (var streamWriter = new StreamWriter(logFile.Open()))
                        {
                            streamWriter.Write(m_LogContent);
                        }
                    }

                    // Add xml content
                    if (m_XmlContent != string.Empty)
                    {
                        var xmlFile = archive.CreateEntry("Crash.xml");

                        using (var streamWriter = new StreamWriter(xmlFile.Open()))
                        {
                            streamWriter.Write(m_XmlContent);
                        }
                    }

                    // Add minidump
                    {
                        var dumpFile = archive.CreateEntry("Crash.dmp");

                        using (var binaryWriter = new BinaryWriter(dumpFile.Open()))
                        {
                            binaryWriter.Write(m_MiniDump);
                        }
                    }

                }

                return memoryStream.ToArray();
            }
        }
    }
}
