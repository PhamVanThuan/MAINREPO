using System.Collections.Generic;
namespace SAHL.Core.Testing.Fluent
{
    public sealed class FluentTestParameters: IEnumerable<KeyValuePair<string,object>>
    {
        private Dictionary<string, object> parameters;
        public FluentTestParameters()
        {
            this.parameters = new Dictionary<string, object>();
        }
        public void Set<T>(string name, T value)
        {
            this.parameters.Add(name, value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}