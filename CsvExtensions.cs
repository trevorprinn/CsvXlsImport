#region Licence
/*
The MIT License (MIT)

Copyright (c) 2016 Babbacombe Computers Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvXlsImport {
    static class CsvExtensions {
        public static string ToCsv<T>(this IEnumerable<T> source) {
            using (var m = new MemoryStream())
            using (var sw = new StreamWriter(m)) {
                ToCsv(source, sw);
                sw.Flush();
                m.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(m)) {
                    return sr.ReadToEnd();
                }
            }
        }

        public static void ToCsv<T>(this IEnumerable<T> source, StreamWriter output) {
            bool first = true;
            Regex re = new Regex(@"^\d+(|\.\d+)$");
            foreach (var element in source) {
                Type t = element.GetType();
                if (first) {
                    var titles = t.GetProperties().Select(p => p.Name).ToArray();
                    output.WriteLine(string.Join(",", titles));
                    first = false;
                }
                var values = t.GetProperties().Select(p => {
                    object o = p.GetValue(element, null);
                    string val = o == null ? "" : o.ToString();
                    if (o != null && !re.Match(val).Success) val = "\"" + val.Replace("\"", "\"\"") + "\"";
                    return val;
                }).ToArray();
                output.WriteLine(string.Join(",", values));
            }
            output.Flush();
        }

        public static IEnumerable<string> GetCsvFieldNames(this StreamReader input) {
            return parseCsvRecord(input);
        }

        public static IEnumerable<T> FromCsv<T>(this StreamReader input) {
            // Get the field names from the csv file
            var fnames = parseCsvRecord(input);
            if (fnames == null) yield break;
            var names = fnames.ToList();

            var props = typeof(T).GetProperties().Where(p => p.SetMethod != null);
            var dataProp = props.FirstOrDefault(p => p.GetCustomAttribute(typeof(CsvDataAttribute)) != null);
            var flds = typeof(T).GetFields().Where(f => f.IsPublic);

            string[] values;
            while (true) {
                values = parseCsvRecord(input);
                if (values == null) break;
                var o = (T)Activator.CreateInstance(typeof(T));
                foreach (var prop in props) {
                    if (prop == dataProp) {
                        prop.SetValue(o, fnames.ZipLongest(values, (n, v) => new CsvField { Name = n, Value = v }));
                    }

                    var ix = names.IndexOf(prop.Name);
                    if (ix >= 0 && ix <= values.Length - 1) {
                        if (prop.PropertyType == typeof(string)) {
                            prop.SetValue(o, string.IsNullOrEmpty(values[ix]) ? null : values[ix]);
                        } else if (prop.PropertyType == typeof(DateTime) || (prop.PropertyType == typeof(DateTime?) && !string.IsNullOrEmpty(values[ix]))) {
                            prop.SetValue(o, new DateTimeConverter().ConvertFromString(values[ix]));
                        } else {
                            var conv = TypeDescriptor.GetConverter(prop.PropertyType);
                            if (conv.IsValid(values[ix])) {
                                prop.SetValue(o, conv.ConvertFrom(values[ix]));
                            }
                        }
                    }
                }
                foreach (var fld in flds) {
                    var ix = names.IndexOf(fld.Name);
                    if (ix >= 0 && ix <= values.Length - 1) {
                        if (fld.FieldType == typeof(string)) {
                            fld.SetValue(o, string.IsNullOrEmpty(values[ix]) ? null : values[ix]);
                        } else if (fld.FieldType == typeof(DateTime) || (fld.FieldType == typeof(DateTime?) && !string.IsNullOrEmpty(values[ix]))) {
                            fld.SetValue(o, new DateTimeConverter().ConvertFromString(values[ix]));
                        } else {
                            var conv = TypeDescriptor.GetConverter(fld.FieldType);
                            if (conv.IsValid(values[ix])) {
                                fld.SetValue(o, conv.ConvertFrom(values[ix]));
                            }
                        }
                    }
                }
                yield return o;
            }
        }

        public static int CountCsv(this StreamReader input) {
            int count = -1;
            while (parseCsvRecord(input) != null) count++;
            return count;
        }

        private static Regex _csvParser = new Regex("(?:^|,)(\\\"(?:[^\\\"]+|\\\"\\\")*\\\"|[^,]*)");

        private static string[] parseCsvRecord(StreamReader input) {
            string rowData;
            do {
                rowData = getCsvRowData(input);
                if (rowData == null) return null;
            } while (string.IsNullOrWhiteSpace(rowData));


            List<string> fields = new List<string>();
            foreach (Match match in _csvParser.Matches(rowData)) {
                StringBuilder field = new StringBuilder(match.Groups[1].Value.Trim());
                if (field.ToString().StartsWith("\"") && field.ToString().EndsWith("\"")) {
                    field.Remove(0, 1);
                    field.Remove(field.Length - 1, 1);
                }
                field.Replace("\"\"", "\"");
                fields.Add(field.ToString());
            }
            return fields.ToArray();
        }

        private static Regex _quoteCounter = new Regex("\\\"");

        private static string getCsvRowData(StreamReader input) {
            int quoteCount = 0;
            StringBuilder s = new StringBuilder();
            string line = input.ReadLine();
            while (line != null) {
                quoteCount += _quoteCounter.Matches(line).Count;
                if (s.Length > 0) s.Append("\r\n");
                s.Append(line);
                if (quoteCount % 2 == 0) break;
                // We have an odd number of quotes, so there must be more.
                line = input.ReadLine();
            }
            if (line == null) return null;
            return s.ToString();
        }
    }

    // When loading CSV into a class, a property with this attribute will receive the fields.
    // It must be an IEnumerable<CsvField>.
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvDataAttribute : Attribute { }

    public struct CsvField {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
