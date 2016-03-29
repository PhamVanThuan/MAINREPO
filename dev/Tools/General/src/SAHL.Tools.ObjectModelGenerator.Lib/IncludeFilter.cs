using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectModelGenerator.Lib
{
    public class IncludeFilter
    {

        public IList<string> Includes { get; protected set; }
        public IList<string> Excluded { get; protected set; }

        public IncludeFilter(string file)
        {
            Includes = new List<string>();
            Excluded = new List<string>();
            string fileData = ReadFile(file);
            TokenizeFile(fileData);
        }

        public string ReadFile(string file)
        {
            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return "*";
        }

        public void TokenizeFile(string data)
        {
            using (StringReader reader = new StringReader(data))
            {
                string line;
                
                while ((line = reader.ReadLine()) != null)
                {
                    var lineData = line.Split(" ".ToCharArray(),2);
                    if (lineData.Length == 1)
                    {
                        Includes.Add(lineData[0]);
                    }
                    else
                    {
                        if (lineData[0] == "-")
                        {
                            Excluded.Add(lineData[1]);
                        }
                        else
                        {
                            Includes.Add(lineData[1]);
                        }
                    }
                }
            }
        }

        public IEnumerable<string> Filter(IEnumerable<string> input)
        {
            foreach (var filterItem in this.Excluded)
            {
                if (filterItem.Contains("*"))
                {

                    input = input.Where(x => !x.Contains(filterItem.Replace("*","")));
                }
                else
                {
                    input = input.Where(x => x != filterItem);
                }
            }
            return input;
        }
    }
}
