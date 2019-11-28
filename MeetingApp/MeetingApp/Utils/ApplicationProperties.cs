using Xamarin.Forms;

namespace MeetingApp.Utils
{
    public class ApplicationProperties
    {
        /// <summary>
        /// ローカルに値を保持する
        /// </summary>
        /// <typeparam name="T">保持するデータの型</typeparam>
        /// <param name="key">保持する際のキー</param>
        /// <param name="value">保持するキーに対する値</param>
        public virtual async void SaveToProperties<T>(string key, T value) where T : class
        {
            if (!string.IsNullOrEmpty(key))
            {
                Application.Current.Properties[key] = value;
                await Application.Current.SavePropertiesAsync();
            }
        }
        /// <summary>
        /// ローカルに保持された値を取り出す
        /// </summary>
        /// <typeparam name="T">保持するデータの型</typeparam>
        /// <param name="key">保持する際に指定したキー</param>
        /// <returns>取り出される値</returns>
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

        /// <summary>
        /// ローカルに保持された値を破棄する
        /// </summary>
        /// <param name="key">破棄する対象のキー</param>
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
