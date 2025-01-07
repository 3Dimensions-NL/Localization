using System;
using System.Collections.Generic;
using System.IO;

namespace _3Dimensions.Localization.Editor.Scripts
{
    public class CsvFileReaderWriter
    {
        /// <summary>
        /// Reads a CSV file and returns it as a 2D list of strings.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A 2D list representing rows and columns of the file.</returns>
        public static List<List<string>> ReadCsv(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified CSV file does not exist.");
            }

            var rows = new List<List<string>>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Handle simple CSV parsing (split by commas; you can extend this for advanced CSV parsing if needed).
                    var columns = line.Split(',');
                    rows.Add(new List<string>(columns));
                }
            }
            return rows;
        }

        /// <summary>
        /// Writes a 2D list of strings to a CSV file.
        /// </summary>
        /// <param name="data">The 2D list representing rows and columns to write to the file.</param>
        /// <param name="filePath">The path to the CSV file.</param>
        public static void WriteCsv(List<List<string>> data, string filePath)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("The data to write is invalid or empty.");
            }

            using (var writer = new StreamWriter(filePath))
            {
                foreach (var row in data)
                {
                    // Handle simple CSV formatting (join by commas; escape additional CSV rules if necessary).
                    writer.WriteLine(string.Join(",", row));
                }
            }
        }
    }
}