using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace _3Dimensions.Localization.Editor.Scripts
{
    public static class CsvFileReaderWriter
    {
        /// <summary>
        /// Field delimiter character (default comma).
        /// </summary>
        public static char Delimiter = ',';

        /// <summary>
        /// Reads a CSV file into a jagged array of strings.
        /// Handles quoted fields (with embedded commas, newlines, or quotes).
        /// </summary>
        /// <param name="path">Path to the CSV file.</param>
        /// <param name="includeHeader">Whether to include the first row (header) in the result.</param>
        /// <returns>Jagged array of rows and columns.</returns>
        public static string[][] GetCsvData(string path, bool includeHeader)
        {
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                    "File not found!",
                    $"File could not be found at path: {path}",
                    "OK");
                return null;
            }

            var rows = new List<string[]>();
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                var currentField = new StringBuilder();
                var currentRow = new List<string>();
                bool insideQuotes = false;

                while (sr.Peek() >= 0)
                {
                    char c = (char)sr.Read();

                    if (c == '"')
                    {
                        if (!insideQuotes)
                        {
                            // Opening quote
                            insideQuotes = true;
                        }
                        else if (sr.Peek() == '"')
                        {
                            // Escaped quote
                            currentField.Append('"');
                            sr.Read();
                        }
                        else
                        {
                            // Closing quote
                            insideQuotes = false;
                        }
                    }
                    else if (c == Delimiter && !insideQuotes)
                    {
                        // End of field
                        currentRow.Add(currentField.ToString());
                        currentField.Clear();
                    }
                    else if ((c == '\n' || c == '\r') && !insideQuotes)
                    {
                        // End of line
                        if (c == '\r' && sr.Peek() == '\n')
                            sr.Read(); // consume '\n' after '\r'

                        currentRow.Add(currentField.ToString());
                        currentField.Clear();

                        rows.Add(currentRow.ToArray());
                        currentRow.Clear();
                    }
                    else
                    {
                        // All other characters (including newlines inside quotes)
                        currentField.Append(c);
                    }
                }

                // Flush last field/row if file doesn't end with newline
                if (insideQuotes)
                    throw new InvalidDataException("Unmatched quote in CSV file.");

                if (currentField.Length > 0 || currentRow.Count > 0)
                {
                    currentRow.Add(currentField.ToString());
                    rows.Add(currentRow.ToArray());
                }
            }

            if (rows.Count == 0)
                return Array.Empty<string[]>();

            // Remove header row if requested
            if (!includeHeader && rows.Count > 1)
                rows = rows.Skip(1).ToList();

            return rows.ToArray();
        }

        /// <summary>
        /// Reads a CSV file and returns it as a 2D list of strings.
        /// </summary>
        public static List<List<string>> ReadCsv(string filePath)
        {
            var raw = GetCsvData(filePath, includeHeader: false);
            if (raw == null)
                return new List<List<string>>();

            return raw
                .Select(row => new List<string>(row))
                .ToList();
        }

        /// <summary>
        /// Reads a CSV file using a custom delimiter.
        /// </summary>
        public static List<List<string>> ReadCsv(string filePath, char delimiter)
        {
            Delimiter = delimiter;
            return ReadCsv(filePath);
        }

        /// <summary>
        /// Writes a 2D list of strings to a CSV file,
        /// escaping fields that contain commas, quotes, or newlines.
        /// </summary>
        public static void WriteCsv(List<List<string>> data, string filePath)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("The data to write is invalid or empty.");

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var row in data)
                {
                    var escaped = row.Select(EscapeField);
                    writer.WriteLine(string.Join(Delimiter.ToString(), escaped));
                }
            }
        }

        /// <summary>
        /// Escapes a single CSV field by wrapping in quotes if needed
        /// and doubling internal quotes.
        /// </summary>
        private static string EscapeField(string field)
        {
            bool needsQuotes =
                field.Contains(Delimiter)
                || field.Contains('"')
                || field.Contains('\n')
                || field.Contains('\r');

            if (!needsQuotes)
                return field;

            var escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
    }
}
