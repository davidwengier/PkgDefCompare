using System;

namespace PkgDefCompare
{
    internal class Program
    {
        public static void Main(string leftFile, string rightFile)
        {
            Console.WriteLine("Comparing " + leftFile + " and " + rightFile);

            var left = PkgDefFile.Parse(leftFile);
            var right = PkgDefFile.Parse(rightFile);

            foreach (var section in left.GetSections())
            {
                if (!right.HasSection(section))
                {
                    Console.WriteLine("Missing section: " + section);
                    continue;
                }

                foreach (var key in left.GetKeys(section))
                {
                    if (!right.HasKey(section, key))
                    {
                        Console.WriteLine("Missing key: " + section + ", " + key);
                        continue;
                    }


                }
            }
        }
    }
}
