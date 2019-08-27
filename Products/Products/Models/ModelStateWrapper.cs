using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Products.Models
{
    public class ModelStateWrapper : IValidationDictionary
    {
        private readonly ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        public void AddModelError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public void Remove(string key)
        {
            _modelState.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return _modelState.ContainsKey(key);
        }

        public bool IsValid => _modelState.IsValid;
        public ICollection<string> Keys => _modelState.Keys.ToList();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var kvp in _modelState)
            {
                var key = kvp.Key;
                foreach (var error in _modelState[key].Errors)
                {
                    yield return new KeyValuePair<string, string>(key, error.ErrorMessage);
                }
            }
        }
    }
}
