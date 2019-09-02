using Xamarin.Forms;

namespace MeetingApp.Utils
{
    public class ApplicationProperties
    {
        public virtual async void SaveToProperties<T>(string key, T value) where T : class
        {
            if (!string.IsNullOrEmpty(key))
            {
                Application.Current.Properties[key] = value;
                await Application.Current.SavePropertiesAsync();
            }
        }

        public virtual T GetFromProperties<T>(string key) where T : class
        {
            var result = default(T);
            if (!string.IsNullOrEmpty(key) && Application.Current.Properties.ContainsKey(key))
            {
                var value = Application.Current.Properties[key];
                if (value != null && value is T)
                {
                    result = (T)(object)value;
                }
            }
            return result;
        }

        public virtual void ClearPropertie(string key)
        {
            if (!string.IsNullOrEmpty(key) && Application.Current.Properties.ContainsKey(key))
            {
                Application.Current.Properties.Remove(key);
                Application.Current.SavePropertiesAsync();
            }
        }
    }
}
