using System.Collections.Generic;

namespace Products.Models
{
    public interface IValidationDictionary
    {
        void AddModelError(string key, string errorMessage);
        void Remove(string key);
        bool IsValid { get; }
        IEnumerator<KeyValuePair<string, string>> GetEnumerator();
        ICollection<string> Keys { get; }
        bool ContainsKey(string key);
    }
}
