using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Snowflake
{
    public class ScriptDictionary : DynamicObject, IDictionary<object, object>
    {
        Dictionary<object, object> data;

        public ICollection<object> Keys
        {
            get { return this.data.Keys; }
        }

        public ICollection<object> Values
        {
            get { return this.data.Values; }
        }

        public object this[object key]
        {
            get { return this.data[key]; }
            set { this.data[key] = value; }
        }

        public int Count
        {
            get { return this.data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ScriptDictionary()
        {
            this.data = new Dictionary<object, object>();
        }

        public void Add(object key, object value)
        {
            this.data.Add(key, value);
        }

        public bool ContainsKey(object key)
        {
            return this.data.ContainsKey(key);
        }
                
        public bool Remove(object key)
        {
            return this.data.Remove(key);
        }

        public bool TryGetValue(object key, out object value)
        {
            return this.data.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<object, object>>.Add(KeyValuePair<object, object> item)
        {
            ((IDictionary<object, object>)this.data).Add(item);
        }

        public void Clear()
        {
            this.data.Clear();
        }

        bool ICollection<KeyValuePair<object, object>>.Contains(KeyValuePair<object, object> item)
        {
            return ((IDictionary<object, object>)this.data).Contains(item);
        }

        void ICollection<KeyValuePair<object, object>>.CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            ((IDictionary<object, object>)this.data).CopyTo(array, arrayIndex);
        }
             
        bool ICollection<KeyValuePair<object, object>>.Remove(KeyValuePair<object, object> item)
        {
            return ((IDictionary<object, object>)this.data).Remove(item);
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ScriptUndefined.Value;

            if (this.data.ContainsKey(binder.Name))
                result = this.data[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.data[binder.Name] = value;
            return true;
        }
    }
}
