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


        //Meeting系API

        //会議情報を単一で取得するAPIコール
        public async Task<GetMeetingParam> GetMeetingDataAsync(string uri, int mid)
        {
            var getMeetingParam = new GetMeetingParam();
            uri = uri + "?mid=" + mid;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    content = content.TrimStart('[');
                    content = content.TrimEnd(']');
                    getMeetingParam.MeetingData = JsonConvert.DeserializeObject<MeetingData>(content);

                    getMeetingParam.MeetingData.StartTime = getMeetingParam.MeetingData.StartDatetime.ToShortTimeString();
                    getMeetingParam.MeetingData.EndTime = getMeetingParam.MeetingData.EndDatetime.ToShortTimeString();
                    getMeetingParam.MeetingData.Date = getMeetingParam.MeetingData.StartDatetime.ToShortDateString();

                    getMeetingParam.IsSuccessed = true;
                    return getMeetingParam;
                }
                else
                {
                    getMeetingParam.HasError = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                getMeetingParam.HasError = true;
            }

            return getMeetingParam;
        }

        //会議情報を全件取得するAPIコール
        public async Task<List<MeetingData>> GetMeetingsDataAsync(string uri, string userId)
        {
            List<MeetingData> meetingDatas = new List<MeetingData>();

            //userIdからidを取得
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

                        //会議管理者かどうかそれぞれのmeetingモデルに通知
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

        //会議情報を新規登録するAPIのコール
        public async Task<CreateMeetingParam> CreateMeetingDataAsync(string uri, MeetingData meetingData, ObservableCollection<MeetingLabelData> labels)
        {

            var json = JsonConvert.SerializeObject(meetingData);
            var jobj = JObject.Parse(json);
            //MeetingDataモデルからJSON化に不要な属性を削除
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

                    //midを取得し会議ラベルDBにLabels分をPOSTする処理
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

        //会議情報を更新するAPIコール
        public async Task<UpdateMeetingParam> UpdateMeetingDataAsync(string uri, MeetingData meeting)
        {

            var json = JsonConvert.SerializeObject(meeting);
            var updateMeetingParam = new UpdateMeetingParam();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.PutAsync(uri, content);


                if (!response.IsSuccessStatusCode)
                {
                    updateMeetingParam.HasError = true;
                    return updateMeetingParam;
                }


                //成功した場合
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Paramを成功に
                    updateMeetingParam.IsSuccessed = response.IsSuccessStatusCode;
                    return updateMeetingParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                updateMeetingParam.HasError = true;
            }
            return updateMeetingParam;
        }


        //MeetingLabel系API

        //会議ラベル情報を全件取得するAPIコール
        public async Task<GetMeetingLabelsParam> GetMeetingLabelsDataAsync(string uri, int mid)
        {
            GetMeetingLabelsParam getMeetingLabelsParam = new GetMeetingLabelsParam();

            //midをクエリストリングに加える
            uri = uri + "?mid=" + mid;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    getMeetingLabelsParam.MeetingLabelDatas = JsonConvert.DeserializeObject<List<MeetingLabelData>>(content);
                    getMeetingLabelsParam.IsSuccessed = true;
                }
                else
                {
                    getMeetingLabelsParam.HasError = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                getMeetingLabelsParam.HasError = true;
            }

            return getMeetingLabelsParam;
        }

        //会議ラベル情報を新規登録するAPIのコール
        public async Task<CreateMeetingLabelParam> CreateMeetingLabelDataAsync(string uri, string labelName, int mid)
        {
            var meetingLabel = new MeetingLabelData(mid, labelName);
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

        //MeetingLabelItem系API

        //会議ラベル項目情報を新規登録するAPIのコール
        public async Task<CreateMeetingLabelItemParam> CreateMeetingLabelItemDataAsync(string uri, string itemName, int lid, int uid)
        {
            var meetingLabelItem = new MeetingLabelItemData(lid, uid, itemName);
            var json = JsonConvert.SerializeObject(meetingLabelItem);

            var createMeetingLabelItemParam = new CreateMeetingLabelItemParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    createMeetingLabelItemParam.IsSuccessed = response.IsSuccessStatusCode;
                    return createMeetingLabelItemParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return createMeetingLabelItemParam;


        }

        //ユーザー情報系API
        //userIdからユーザー情報を取得するAPIコール
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

        //ユーザー情報を新規登録するAPIのコール
        public async Task<SignUpParam> SignUpUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var signUpParam = new SignUpParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //テーブル内にuserIdが存在するかどうかチェック
                if (await CheckUserDataAsync(uri, userId))

                {
                    //存在していた場合POSTを失敗で終了
                    signUpParam.HasError = true;
                    signUpParam.UserExists = true;
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


        //ユーザーがDBに存在するかどうかチェックするAPIコール
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


        //Token情報系API

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
