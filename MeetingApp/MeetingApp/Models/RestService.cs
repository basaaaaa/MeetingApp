using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public async Task<List<MeetingData>> GetMeetingsDataAsync(string uri, string userId)
        {
            List<MeetingData> meetingDatas = new List<MeetingData>();

            //userId����id���擾
            GetUserParam getUserParam = await GetUserDataAsync(UserConstants.OpenUserEndPoint, userId);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    meetingDatas = JsonConvert.DeserializeObject<List<MeetingData>>(content);
                    Console.WriteLine(meetingDatas);

                    foreach (MeetingData meeting in meetingDatas)
                    {
                        meeting.StartTime = meeting.StartDatetime.ToShortTimeString();
                        meeting.EndTime = meeting.EndDatetime.ToShortTimeString();
                        meeting.Date = meeting.StartDatetime.ToShortDateString();

                        //��c�Ǘ��҂��ǂ������ꂼ���meeting���f���ɒʒm
                        if (meeting.Owner == getUserParam.User.Id)
                        {
                            meeting.IsOwner = true;

                        }
                        else
                        {
                            meeting.IsOwner = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return meetingDatas;
        }

        //��c����V�K�o�^����API�̃R�[��
        public async Task<CreateMeetingParam> CreateMeetingDataAsync(string uri, MeetingData meetingData, ObservableCollection<MeetingLabelData> labels)
        {

            var json = JsonConvert.SerializeObject(meetingData);
            var jobj = JObject.Parse(json);
            //MeetingData���f������JSON���ɕs�v�ȑ������폜
            jobj.Remove("StartTime");
            jobj.Remove("EndTime");
            jobj.Remove("Date");
            jobj.Remove("IsOwner");

            json = JsonConvert.SerializeObject(jobj);


            var createMeetingParam = new CreateMeetingParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                    //mid���擾����c���x��DB��Labels����POST���鏈��
                    var responseMeetingData = JsonConvert.DeserializeObject<MeetingData>(responseContent);
                    var mid = responseMeetingData.Id;

                    foreach (var label in labels)
                    {
                        await CreateMeetingLabelDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, label.LabelName, mid);
                    }

                    createMeetingParam.IsSuccessed = response.IsSuccessStatusCode;
                    return createMeetingParam;
                }
                else
                {
                    createMeetingParam.HasError = true;
                    createMeetingParam.ApiCallError = true;
                    return createMeetingParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                createMeetingParam.HasError = true;
            }
            return createMeetingParam;


        }

        //��c���x������S���擾����API�R�[��
        public async Task<List<MeetingLabelData>> GetMeetingLabelsDataAsync(string uri, int mid)
        {
            List<MeetingLabelData> meetingLabelDatas = new List<MeetingLabelData>();

            //mid���N�G���X�g�����O�ɉ�����
            uri = uri + "?mid=" + mid;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    meetingLabelDatas = JsonConvert.DeserializeObject<List<MeetingLabelData>>(content);
                    Console.WriteLine(meetingLabelDatas);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return meetingLabelDatas;
        }

        //��c���x������V�K�o�^����API�̃R�[��
        public async Task<CreateMeetingLabelParam> CreateMeetingLabelDataAsync(string uri, string labelName, int mid)
        {
            var meetingLabel = new MeetingLabelData(mid, labelName);
            var json = JsonConvert.SerializeObject(meetingLabel);

            var createMeetingLabelParam = new CreateMeetingLabelParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //���͂��ꂽ�J�[�h���x�������󂩂ǂ����`�F�b�N
                if (string.IsNullOrEmpty(labelName))
                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
                    createMeetingLabelParam.HasError = true;
                    createMeetingLabelParam.BlankLabelName = true;
                    return createMeetingLabelParam;
                }


                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    createMeetingLabelParam.IsSuccessed = response.IsSuccessStatusCode;
                    return createMeetingLabelParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return createMeetingLabelParam;


        }

        //��c����1���폜����API�R�[��
        public async void DeleteMeetingDataAsync(string uri, int mid)
        {
            uri = uri + mid;
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("DELETE SUCCESSED");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
        }

        //userId���烆�[�U�[�����擾����API�R�[��
        public async Task<GetUserParam> GetUserDataAsync(string uri, string userId)
        {
            var getUserParam = new GetUserParam();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + "?userId=" + userId);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    content = content.TrimStart('[');
                    content = content.TrimEnd(']');
                    Console.WriteLine(content);

                    getUserParam.User = JsonConvert.DeserializeObject<UserData>(content);

                    getUserParam.IsSuccessed = true;
                    return getUserParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                getUserParam.HasError = true;
            }

            return getUserParam;
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

                //���͂��ꂽUser��������Ȃ��ꍇ
                if (!response.IsSuccessStatusCode)
                {
                    LoginParam.HasError = true;
                    LoginParam.NotFoundUser = true;
                    return LoginParam;
                }


                //���O�C�����������������ꍇ
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //token�����擾
                    var tokenData = JsonConvert.DeserializeObject<TokenData>(responseContent);
                    LoginParam.TokenData = tokenData;
                    //LoginParam�𐬌���
                    LoginParam.IsSuccessed = response.IsSuccessStatusCode;
                    return LoginParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                LoginParam.HasError = true;
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

            return true;
        }

        //Local�ɕێ�����token���DB���ɑ��݂��邩�`�F�b�N����API�R�[��
        public async Task<TokenCheckParam> CheckTokenDataAsync(string uri, TokenData token)
        {
            var TokenCheckParam = new TokenCheckParam();
            var tokenCheckUrl = uri + "?tokenText=" + token.TokenText;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(tokenCheckUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var receivedTokenData = JsonConvert.DeserializeObject<TokenData>(content);

                    //�L�����ԓ����ǂ�������
                    DateTime dt = DateTime.Now;
                    if (receivedTokenData.StartTime < dt && receivedTokenData.EndTime > dt)
                    {
                        TokenCheckParam.IsSuccessed = true;
                        return TokenCheckParam;
                    }
                    else
                    {
                        //�L�����ԊO��Token�������ꍇ
                        TokenCheckParam.HasError = true;
                        TokenCheckParam.IsOverTimeToken = true;
                    }

                }
                else
                {
                    //�Y������tokenText��DB���Ɍ�����Ȃ��Ƃ�
                    TokenCheckParam.HasError = true;
                    TokenCheckParam.NotFoundTokenText = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return TokenCheckParam;
        }

    }
}
