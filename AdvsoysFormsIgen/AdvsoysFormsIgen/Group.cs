using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdvsoysFormsIgen
{
    public class Group<TKey, TValue> : IEnumerable<TValue>
    {
        private readonly TKey key;
        private readonly IEnumerable<TValue> values;

        public Group(TKey key, IEnumerable<TValue> values)
        {
            this.key = key;
            this.values = values ?? Enumerable.Empty<TValue>();
        }

        public TKey Key
        {
            get { return key; }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}