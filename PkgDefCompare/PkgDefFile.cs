using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PkgDefCompare
{
    internal class PkgDefFile
    {
        private Dictionary<string, Dictionary<string, string>> _sections = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        internal static PkgDefFile Parse(string pkgDefFile)
        {
            var file = new PkgDefFile();

            string section = null;

            foreach (var line in File.ReadAllLines(pkgDefFile))
            {
                if (line.StartsWith(';') || line.Trim().Length == 0)
                {
                    continue;
                }
                else if (line.StartsWith('['))
                {
                    section = line.Trim('[',']');
                }
                else 
                {
                    if (section == null)
                    {
                        throw new InvalidOperationException("Couldn't find a section to start with");
                    }
                    string key = line.Split('=').First().Trim('"');
                    string value = line.Split('=').Last().Trim('"');
                    file.SetValue(section, key, value);
                }
            }

            return file;
        }

        internal string GetValue(string section, string key) => _sections[section][key];

        internal IEnumerable<string> GetKeys(string section) => _sections[section].Keys.Cast<string>();

        internal bool HasKey(string section, string key) => _sections[section].ContainsKey(key);

        internal bool HasSection(string section) => _sections.ContainsKey(section);

        public IEnumerable<string> GetSections() => _sections.Keys.Cast<string>();

        private void SetValue(string section, string key, string value)
        {
            AddSection(section);
            _sections[section][key] = value;
        }

        private void AddSection(string section)
        {
            if (!_sections.ContainsKey(section))
            {
                _sections.Add(section, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }
        }
    }
}