using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeetingApp
{
    class RestService
    {
        HttpClient _client;
        CheckString _checkString;

        public RestService()
        {
            _client = new HttpClient();
            _checkString = new CheckString();
        }

        //��c����P��Ŏ擾����API�R�[��
        public async Task<MeetingData> GetMeetingDataAsync(string uri)
        {
            MeetingData meetingData = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    meetingData = JsonConvert.DeserializeObject<MeetingData>(content);
                    Console.WriteLine(meetingData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return meetingData;
        }

        //��c����S���擾����API�R�[��
        public async Task<List<MeetingData>> GetMeetingsDataAsync(string uri)
        {
            List<MeetingData> meetingDatas = new List<MeetingData>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    meetingDatas = JsonConvert.DeserializeObject<List<MeetingData>>(content);
                    Console.WriteLine(meetingDatas);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return meetingDatas;
        }

        //���[�U�[����V�K�o�^����API�̃R�[��
        public async Task<SignUpParam> SignUpUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var signUpParam = new SignUpParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //���͂��ꂽuserId���󂩂ǂ����`�F�b�N
                if (string.IsNullOrEmpty(userId))
                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    signUpParam.HasError = true;
                    signUpParam.BlankUserId = true;
                    return signUpParam;
                }

                //���͂��ꂽpassword���󂩂ǂ����`�F�b�N
                if (string.IsNullOrEmpty(password))
                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    signUpParam.HasError = true;
                    signUpParam.BlankPassword = true;
                    return signUpParam;
                }

                //�e�[�u������userId�����݂��邩�ǂ����`�F�b�N
                if (await CheckUserDataAsync(uri, userId))

                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    signUpParam.HasError = true;
                    signUpParam.UserExists = true;
                    return signUpParam;
                }

                //���͂��ꂽ�p�X���[�h���w�蕶�����𖞂����Ă��邩�ǂ����`�F�b�N
                if (password.Length < 6)

                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    signUpParam.HasError = true;
                    signUpParam.ShortPassword = true;
                    return signUpParam;
                }

                //���͂��ꂽuserId�����p�p�����݂̂ō\������Ă��邩�`�F�b�N
                if (_checkString.isAlphaNumericPlusAlphaOnly(userId))
                {
                    signUpParam.HasError = true;
                    signUpParam.UnSpecifiedUserId = true;
                    return signUpParam;
                }

                if (_checkString.isAlphaNumericPlusAlphaOnly(password))
                {
                    signUpParam.HasError = true;
                    signUpParam.UnSpecifiedPassword = true;
                    return signUpParam;
                }

                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    signUpParam.IsSuccessed = response.IsSuccessStatusCode;
                    return signUpParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return signUpParam;


        }

        //���O�C��API�̃R�[��
        public async Task<LoginParam> LoginUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var LoginParam = new LoginParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //���[�U�[�����݂��Ȃ��ꍇ�iuserId,Password��Users�e�[�u���ɑ��݂��Ȃ��j


                //���͂��ꂽuserId���󂩂ǂ����`�F�b�N
                if (string.IsNullOrEmpty(userId))
                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    LoginParam.HasError = true;
                    LoginParam.BlankUserId = true;
                    return LoginParam;
                }

                //���͂��ꂽpassword���󂩂ǂ����`�F�b�N
                if (string.IsNullOrEmpty(password))
                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    LoginParam.HasError = true;
                    LoginParam.BlankPassword = true;
                    return LoginParam;
                }

                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    LoginParam.IsSuccessed = response.IsSuccessStatusCode;
                    return LoginParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return LoginParam;


        }


        //��c����P��Ŏ擾����API�R�[��
        public async Task<Boolean> CheckUserDataAsync(string uri, string userId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + "?userId=" + userId);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content == "[null]") { return false; }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return true; ;
        }

    }
}
