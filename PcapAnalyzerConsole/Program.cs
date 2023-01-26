/*
 * Project's plan
 * Get PCAP file
 * Open it, read header
 * For every params (count packets or size) write packets, but before write header
 * 
 */
using System;
using BufferStream;

namespace PcapAnalyzerConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static string PcapFileNameInput = "";
        public static string PcapFileNameOutput = "";
        public static int SplitPcapCount = 1;
        public static UInt64 PcapPacketsCount = 0;
        private static PacketEnumerator InputPcapEnumerator;
        
        

        static void ReadPcapFilename()
        {
            ConsoleWriteWithTimestamp("Enter full path for Pcap filename: ");
            PcapFileNameInput =  Console.ReadLine();
        }

        static bool ValidatePcapFilename()
        {
            try
            {
                InputPcapEnumerator = new PacketEnumerator(PcapFileNameInput);
            }
            catch(Exception ex)
            {
               ConsoleWriteWithTimestamp($"{ex.Source} - {ex.Message}");
               return false;
            }

            return true;

        }

        static void CountPcapPackets()
        {
            while (InputPcapEnumerator.MoveNext())
            {
                PcapPacketsCount++;
            } 
            InputPcapEnumerator.Reset();
        }

        static void ConsoleWriteWithTimestamp(string text)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] - {text}");
        }
        
        static int Main(string[] args)
        {
            ReadPcapFilename();
            if (!ValidatePcapFilename())
                return -1;
            ConsoleWriteWithTimestamp($"Counting packets... Please, wait!");
            CountPcapPackets();
            ConsoleWriteWithTimestamp($"Found {PcapPacketsCount} in the {PcapFileNameInput}");
            ConsoleWriteWithTimestamp("Enter split tome count for PCAP file: ");
            return 0;
        }
    }
}