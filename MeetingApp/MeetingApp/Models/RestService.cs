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

        //会議情報を単一で取得するAPIコール
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

        //会議情報を全件取得するAPIコール
        public async Task<List<MeetingData>> GetMeetingsDataAsync(string uri, string myUserId)
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

                    foreach (MeetingData meeting in meetingDatas)
                    {
                        meeting.StartTime = meeting.StartDatetime.ToShortTimeString();
                        meeting.EndTime = meeting.EndDatetime.ToShortTimeString();
                        meeting.Date = meeting.StartDatetime.ToShortDateString();

                        //会議管理者かどうかそれぞれのmeetingモデルに通知
                        if (meeting.Owner == myUserId)
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

        //会議ラベル情報を新規登録するAPIのコール
        public async Task<CreateMeetingParam> CreateMeetingDataAsync(string uri, MeetingData meetingData)
        {
            var json = JsonConvert.SerializeObject(meetingData);

            var createMeetingParam = new CreateMeetingParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //入力された会議タイトルが空かどうかチェック
                if (string.IsNullOrEmpty(meetingData.Title))
                {
                    //存在していた場合POSTを失敗で終了
                    createMeetingParam.HasError = true;
                    createMeetingParam.BlankMeetingTitle = true;
                    return createMeetingParam;
                }

                //入力された会議実施日が空かどうかチェック
                if (string.IsNullOrEmpty(meetingData.Date))
                {
                    //存在していた場合POSTを失敗で終了
                    createMeetingParam.HasError = true;
                    createMeetingParam.BlankMeetingDate = true;
                    return createMeetingParam;
                }

                //入力された会議開始時間が空かどうかチェック
                if (string.IsNullOrEmpty(meetingData.StartTime))
                {
                    //存在していた場合POSTを失敗で終了
                    createMeetingParam.HasError = true;
                    createMeetingParam.BlankMeetingStartTime = true;
                    return createMeetingParam;
                }

                //入力された会議終了時間が空かどうかチェック
                if (string.IsNullOrEmpty(meetingData.EndTime))
                {
                    //存在していた場合POSTを失敗で終了
                    createMeetingParam.HasError = true;
                    createMeetingParam.BlankMeetingEndTime = true;
                    return createMeetingParam;
                }


                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    createMeetingParam.IsSuccessed = response.IsSuccessStatusCode;
                    return createMeetingParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return createMeetingParam;


        }

        //会議ラベル情報を全件取得するAPIコール
        public async Task<List<MeetingLabelData>> GetMeetingLabelsDataAsync(string uri, int mid)
        {
            List<MeetingLabelData> meetingLabelDatas = new List<MeetingLabelData>();

            //midをクエリストリングに加える
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

        //会議ラベル情報を新規登録するAPIのコール
        public async Task<CreateMeetingLabelParam> CreateMeetingLabelDataAsync(string uri, string labelName)
        {
            var meetingLabel = new MeetingLabelData(labelName);
            var json = JsonConvert.SerializeObject(meetingLabel);

            var createMeetingLabelParam = new CreateMeetingLabelParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //入力されたカードラベル名が空かどうかチェック
                if (string.IsNullOrEmpty(labelName))
                {
                    //存在していた場合POSTを失敗で終了
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

        //会議情報を1件削除するAPIコール
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

        //ユーザー情報を新規登録するAPIのコール
        public async Task<SignUpParam> SignUpUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var signUpParam = new SignUpParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //入力されたuserIdが空かどうかチェック
                if (string.IsNullOrEmpty(userId))
                {
                    //存在していた場合POSTを失敗で終了
                    signUpParam.HasError = true;
                    signUpParam.BlankUserId = true;
                    return signUpParam;
                }

                //入力されたpasswordが空かどうかチェック
                if (string.IsNullOrEmpty(password))
                {
                    //存在していた場合POSTを失敗で終了
                    signUpParam.HasError = true;
                    signUpParam.BlankPassword = true;
                    return signUpParam;
                }

                //テーブル内にuserIdが存在するかどうかチェック
                if (await CheckUserDataAsync(uri, userId))

                {
                    //存在していた場合POSTを失敗で終了
                    signUpParam.HasError = true;
                    signUpParam.UserExists = true;
                    return signUpParam;
                }

                //入力されたパスワードが指定文字数を満たしているかどうかチェック
                if (password.Length < 6)

                {
                    //存在していた場合POSTを失敗で終了
                    signUpParam.HasError = true;
                    signUpParam.ShortPassword = true;
                    return signUpParam;
                }

                //入力されたuserIdが半角英数字のみで構成されているかチェック
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

        //ログインAPIのコール
        public async Task<LoginParam> LoginUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var LoginParam = new LoginParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                //入力されたuserIdが空かどうかチェック
                if (string.IsNullOrEmpty(userId))
                {
                    //存在していた場合POSTを失敗で終了
                    LoginParam.HasError = true;
                    LoginParam.BlankUserId = true;
                    return LoginParam;
                }

                //入力されたpasswordが空かどうかチェック
                if (string.IsNullOrEmpty(password))
                {
                    //存在していた場合POSTを失敗で終了
                    LoginParam.HasError = true;
                    LoginParam.BlankPassword = true;
                    return LoginParam;
                }

                var response = await _client.PostAsync(uri, content);

                //入力されたUserが見つからない場合
                if (!response.IsSuccessStatusCode)
                {
                    LoginParam.HasError = true;
                    LoginParam.NotFoundUser = true;
                    return LoginParam;
                }


                //ログイン処理が成功した場合
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //token情報を取得
                    var tokenData = JsonConvert.DeserializeObject<TokenData>(responseContent);
                    LoginParam.TokenData = tokenData;
                    //LoginParamを成功に
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


        //会議情報を単一で取得するAPIコール
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

        //Localに保持するtoken情報がDB内に存在するかチェックするAPIコール
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

                    //有効時間内かどうか検証
                    DateTime dt = DateTime.Now;
                    if (receivedTokenData.StartTime < dt && receivedTokenData.EndTime > dt)
                    {
                        TokenCheckParam.IsSuccessed = true;
                        return TokenCheckParam;
                    }
                    else
                    {
                        //有効時間外のTokenだった場合
                        TokenCheckParam.HasError = true;
                        TokenCheckParam.IsOverTimeToken = true;
                    }

                }
                else
                {
                    //該当するtokenTextがDB内に見つからないとき
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
