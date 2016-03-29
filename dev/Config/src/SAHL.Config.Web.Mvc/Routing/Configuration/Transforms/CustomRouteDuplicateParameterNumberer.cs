using System.Text.RegularExpressions;

namespace SAHL.Config.Web.Mvc.Routing.Configuration.Transforms
{
    public class CustomRouteDuplicateParameterNumberer
    {
        public string Number(string source)
        {
            var regex = new Regex(@"{([A-Za-z0-9])*}");
            var count = -1;
            var transformedUrl = regex.Replace(source,
                a =>
                {
                    //we only want to replace duplicated parameters
                    var occurences = Regex.Matches(source, a.Value).Count;
                    if (occurences <= 1)
                    {
                        //not duplicate, ignore
                        return a.Value;
                    }
                    count++;
                    if (count <= 0)
                    {
                        //first parameter, do not number
                        return a.Value;
                    }
                    if (char.IsDigit(a.Value[a.Value.Length - 2]))
                    {
                        //already ends in a number, do not number
                        return a.Value;
                    }
                    //add the parameter count to the end of the parameter name
                    return a.Value.Insert(a.Length - 1, count.ToString());
                }
                );

            return transformedUrl;
        }
    }
}