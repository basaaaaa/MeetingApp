using Xamarin.Forms;

namespace MeetingApp.Utils
{
    public class ApplicationProperties
    {
        /// <summary>
        /// ���[�J���ɒl��ێ�����
        /// </summary>
        /// <typeparam name="T">�ێ�����f�[�^�̌^</typeparam>
        /// <param name="key">�ێ�����ۂ̃L�[</param>
        /// <param name="value">�ێ�����L�[�ɑ΂���l</param>
        public virtual async void SaveToProperties<T>(string key, T value) where T : class
        {
            if (!string.IsNullOrEmpty(key))
            {
                Application.Current.Properties[key] = value;
                await Application.Current.SavePropertiesAsync();
            }
        }
        /// <summary>
        /// ���[�J���ɕێ����ꂽ�l�����o��
        /// </summary>
        /// <typeparam name="T">�ێ�����f�[�^�̌^</typeparam>
        /// <param name="key">�ێ�����ۂɎw�肵���L�[</param>
        /// <returns>���o�����l</returns>
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
        /// ���[�J���ɕێ����ꂽ�l��j������
        /// </summary>
        /// <param name="key">�j������Ώۂ̃L�[</param>
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
