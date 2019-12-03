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
    public class RestService
    {
        HttpClient _client;
        CheckString _checkString;
        JsonProcessing _jsonProcessing;

        public RestService()
        {
            _client = new HttpClient();
            _checkString = new CheckString();
            _jsonProcessing = new JsonProcessing();
        }

        #region Meeting

        /// <summary>
        /// 会議情報単一取得のAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="mid">会議ID</param>
        /// <returns>GetMeetingParam</returns>
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

                    //[]を除去する処理
                    content = _jsonProcessing.RemoveOuterBrackets(content);

                    //jsonからオブジェクト化
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

        /// <summary>
        /// 会議情報全件取得のAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>会議情報のリスト</returns>
        public async Task<GetMeetingsParam> GetMeetingsDataAsync(string uri, string userId)
        {
            var meetingsData = new List<MeetingData>();
            var getMeetingsParam = new GetMeetingsParam();

            //userIdからidを取得
            GetUserParam getUserParam = await GetUserDataAsync(UserConstants.OpenUserEndPoint, userId);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    meetingsData = JsonConvert.DeserializeObject<List<MeetingData>>(content);
                    Console.WriteLine(meetingsData);

                    getMeetingsParam.Meetings = meetingsData;

                    foreach (MeetingData meeting in meetingsData)
                    {
                        meeting.StartTime = meeting.StartDatetime.ToShortTimeString();
                        meeting.EndTime = meeting.EndDatetime.ToShortTimeString();
                        meeting.Date = meeting.StartDatetime.ToShortDateString();

                        //会議管理者かどうかそれぞれのmeetingモデルに通知
                        if (meeting.Owner == getUserParam.User.Id)
                        {
                            meeting.IsOwner = true;
                            meeting.IsGeneral = false;

                        }
                        else
                        {
                            meeting.IsOwner = false;
                            meeting.IsGeneral = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return getMeetingsParam;
        }

        /// <summary>
        /// 会議情報を新規登録するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="meetingData">新規登録する会議情報</param>
        /// <param name="labels">会議に紐づくラベルリスト</param>
        /// <returns>CreateMeetingParam</returns>
        public async Task<CreateMeetingParam> CreateMeetingDataAsync(string uri, MeetingData meetingData, ObservableCollection<MeetingLabelData> labels)
        {

            var json = JsonConvert.SerializeObject(meetingData);
            var jobj = JObject.Parse(json);

            //MeetingDataモデルからJSON化に不要な属性を削除
            jobj.Remove("StartTime");
            jobj.Remove("EndTime");
            jobj.Remove("Date");
            jobj.Remove("IsOwner");
            jobj.Remove("IsGeneral");

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

        /// <summary>
        /// 会議情報を1件削除するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="mid">削除する会議ID</param>
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

        /// <summary>
        /// 会議情報を更新するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="meeting">更新対象の会議データ</param>
        /// <returns>UpdateMeetingParam</returns>
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

        #endregion


        #region MeetingLabel

        /// <summary>
        /// 指定会議における指定ユーザーが持つラベル情報群を取得
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="mid">ラベルを取得する対象の会議ID</param>
        /// <param name="uid">ラベルに対する項目を取得する対象のユーザーID</param>
        /// <returns>GetMeetingLabelsParam</returns>
        public async Task<GetMeetingLabelsParam> GetMeetingLabelsDataAsync(string uri, int mid, int uid)
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

                    //取得したラベル群それぞれ自分の項目を取得する
                    foreach (MeetingLabelData l in getMeetingLabelsParam.MeetingLabelDatas)
                    {
                        await l.GetMyItemsAsync(uid);
                    }

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

        /// <summary>
        /// 会議に対するラベルを新規追加するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="labelName">追加するラベルの名前</param>
        /// <param name="mid">ラベルを追加する対象の会議ID</param>
        /// <returns>CreateMeetingLabelParam</returns>
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

        #endregion


        #region MeetingLabelItem


        /// <summary>
        /// 会議ラベルに対する項目を追加するAPIのコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="meetingLabelItem">追加する項目の情報</param>
        /// <returns>CreateMeetingLabelItemParam</returns>
        public async Task<CreateMeetingLabelItemParam> CreateMeetingLabelItemDataAsync(string uri, MeetingLabelItemData meetingLabelItem)
        {
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

        /// <summary>
        /// 指定した会議のラベルに対するユーザーが持つ項目群の取得APIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="lid">項目群を取得するラベルID</param>
        /// <param name="uid">項目群を取得する対象の保持者のユーザーID</param>
        /// <returns>GetMeetingLabelItemsParam</returns>
        public async Task<GetMeetingLabelItemsParam> GetMeetingLabelItemsDataAsync(string uri, int lid, int uid)
        {
            GetMeetingLabelItemsParam getMeetingLabelItemsParam = new GetMeetingLabelItemsParam();

            //midをクエリストリングに加える
            uri = uri + "?lid=" + lid + "&uid=" + uid;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    getMeetingLabelItemsParam.MeetingLabelItemDatas = JsonConvert.DeserializeObject<List<MeetingLabelItemData>>(content);
                    getMeetingLabelItemsParam.IsSuccessed = true;
                }
                else
                {
                    getMeetingLabelItemsParam.HasError = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                getMeetingLabelItemsParam.HasError = true;
            }

            return getMeetingLabelItemsParam;
        }

        /// <summary>
        /// 会議ラベルに対する項目情報を1件削除するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="iid">削除対象の項目ID</param>
        /// <returns>DeleteMeetingLabelItemParam</returns>
        public async Task<DeleteMeetingLabelItemParam> DeleteMeetingLabelItemDataAsync(string uri, int iid)
        {
            uri = uri + iid;
            var deleteMeetingLabelItemParam = new DeleteMeetingLabelItemParam();
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    deleteMeetingLabelItemParam.IsSuccessed = true;
                    return deleteMeetingLabelItemParam;

                }
                else
                {
                    deleteMeetingLabelItemParam.HasError = true;
                    return deleteMeetingLabelItemParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                return deleteMeetingLabelItemParam;
            }
        }
        #endregion


        #region Participant

        /// <summary>
        /// 会議入室の際の参加者情報を登録するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="uid">参加者のユーザーID</param>
        /// <param name="mid">入室対象の会議ID</param>
        /// <returns>CreateParticipateParam</returns>
        public async Task<CreateParticipateParam> CreateParticipateDataAsync(string uri, int uid, int mid)
        {
            var participantData = new ParticipantData(uid, mid);
            var json = JsonConvert.SerializeObject(participantData);

            var jobj = JObject.Parse(json);
            //MeetingDataモデルからJSON化に不要な属性を削除
            jobj.Remove("id");
            jobj.Remove("UserId");
            jobj.Remove("LabelItems");

            json = JsonConvert.SerializeObject(jobj);

            var createParticipateParam = new CreateParticipateParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _client.PostAsync(uri, content);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    createParticipateParam.IsSuccessed = response.IsSuccessStatusCode;
                    return createParticipateParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return createParticipateParam;


        }

        /// <summary>
        /// 指定ユーザーが会議に参加済みかどうか確認するAPIコール(ユーザーIDのみ)
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="uid">確認する対象のユーザーID</param>
        /// <returns>CheckParticipantParam</returns>
        public async Task<CheckParticipantParam> CheckParticipantDataAsync(string uri, int uid)
        {
            var checkParticipantParam = new CheckParticipantParam();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + "?uid=" + uid);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (content == "[null]")
                    {
                        checkParticipantParam.HasError = true;
                        checkParticipantParam.NoExistUser = true;
                        return checkParticipantParam;
                    }
                    //ユーザーが見つかった場合
                    checkParticipantParam.IsSuccessed = true;
                    checkParticipantParam.Participant = JsonConvert.DeserializeObject<ParticipantData>(content);
                    return checkParticipantParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                checkParticipantParam.HasError = true;
            }

            return checkParticipantParam;
        }

        /// <summary>
        /// 指定ユーザーが会議に参加済みかどうか確認するAPIコール(ユーザーID、会議情報ID)
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="uid">探索対象のユーザーID</param>
        /// <param name="mid">探索対象の会議ID</param>
        /// <returns>CheckParticipantParam</returns>
        public async Task<CheckParticipantParam> CheckParticipantDataAsync(string uri, int uid, int mid)
        {
            var checkParticipantParam = new CheckParticipantParam();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + "?uid=" + uid + "&mid=" + mid);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (content == "[]")
                    {
                        checkParticipantParam.HasError = true;
                        checkParticipantParam.NoExistUser = true;
                        return checkParticipantParam;
                    }
                    //ユーザーが見つかった場合
                    //list -> 要素の処理
                    //[]を除去する処理
                    content = _jsonProcessing.RemoveOuterBrackets(content);

                    checkParticipantParam.IsSuccessed = true;
                    checkParticipantParam.Participant = JsonConvert.DeserializeObject<ParticipantData>(content);
                    return checkParticipantParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                checkParticipantParam.HasError = true;
            }

            return checkParticipantParam;
        }

        /// <summary>
        /// 指定会議に参加中のユーザーを全件取得するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="mid">探索対象の会議ID</param>
        /// <returns>GetParitipantsParam</returns>
        public async Task<GetParticipantsParam> GetParticipantsDataAsync(string uri, int mid)
        {
            var getParticipantsParam = new GetParticipantsParam();

            //midをクエリストリングに加える
            uri = uri + "?mid=" + mid;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);

                    var participants = JsonConvert.DeserializeObject<List<ParticipantData>>(content);

                    getParticipantsParam.Participants = participants;

                    //View用に自身のuserId、各ラベル項目を取得
                    foreach (ParticipantData p in getParticipantsParam.Participants)
                    {
                        await p.GetMyUserId();
                        //await p.GetMyLabelItems();
                    }


                    getParticipantsParam.IsSuccessed = true;
                }
                else
                {
                    getParticipantsParam.HasError = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                getParticipantsParam.HasError = true;
            }

            return getParticipantsParam;
        }

        /// <summary>
        /// 会議参加者情報を1件削除するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="uid">削除対象参加者のユーザーID</param>
        /// <param name="mid">参加者が入室した会議ID</param>
        /// <returns>DeleteParticipantParam</returns>
        public async Task<DeleteParticipantParam> DeleteParticipantDataAsync(string uri, int uid, int mid)
        {
            uri = uri + "?uid=" + uid + "&mid=" + mid;
            var deleteParticipantParam = new DeleteParticipantParam();

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    deleteParticipantParam.IsSuccessed = true;
                    string content = await response.Content.ReadAsStringAsync();
                    return deleteParticipantParam;
                }

                deleteParticipantParam.HasError = true;
                return deleteParticipantParam;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                deleteParticipantParam.HasError = true;
                return deleteParticipantParam;

            }
        }

        /// <summary>
        /// 会議の参加者情報を更新するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="participant">参加者の更新内容情報</param>
        /// <returns>UpdateParticipantParam</returns>
        public async Task<UpdateParticipantParam> UpdateParticipantDataAsync(string uri, ParticipantData participant)
        {

            var json = JsonConvert.SerializeObject(participant);
            var updateParticipantParam = new UpdateParticipantParam();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.PutAsync(uri, content);


                if (!response.IsSuccessStatusCode)
                {
                    updateParticipantParam.HasError = true;
                    return updateParticipantParam;
                }


                //成功した場合
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Paramを成功に
                    updateParticipantParam.IsSuccessed = response.IsSuccessStatusCode;
                    return updateParticipantParam;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
                updateParticipantParam.HasError = true;
            }
            return updateParticipantParam;
        }
        #endregion


        #region User

        /// <summary>
        /// ユーザー情報を取得するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="userId">対象のユーザーID</param>
        /// <returns>GetUserParam</returns>
        public async Task<GetUserParam> GetUserDataAsync(string uri, string userId)
        {

            var getUserParam = new GetUserParam();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + "?userId=" + userId);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    //[]を除去する処理
                    content = _jsonProcessing.RemoveOuterBrackets(content);

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

        /// <summary>
        /// 一意の自然数uidからユーザー情報を取得するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="uid">ユーザーのuid</param>
        /// <returns>GetUserParam</returns>
        public async Task<GetUserParam> GetUserDataAsync(string uri, int uid)
        {
            var getUserParam = new GetUserParam();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri + uid);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
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

        /// <summary>
        /// /ユーザー情報を新規登録するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="userId">登録するユーザーID</param>
        /// <param name="password">登録するパスワード</param>
        /// <returns>SignUpParam</returns>
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

        /// <summary>
        /// ログインAPIのコール
        /// </summary>
        /// <param name="uri">コールするAPI</param>
        /// <param name="userId">ログイン情報のユーザーID</param>
        /// <param name="password">ログイン情報のパスワード</param>
        /// <returns>LoginParam</returns>
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


        /// <summary>
        /// ユーザー情報が存在するかどうかチェックするAPIコール
        /// </summary>
        /// <param name="uri">コールするAPI</param>
        /// <param name="userId">確認対象のユーザーID</param>
        /// <returns>存在すればTrue、いなければFalse</returns>
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

        #endregion


        #region Token

        /// <summary>
        /// トークン情報が正しいものかどうか照合するAPIコール
        /// </summary>
        /// <param name="uri">コールするURL</param>
        /// <param name="token">トークン情報</param>
        /// <returns>TokenCheckParam</returns>
        public virtual async Task<TokenCheckParam> CheckTokenDataAsync(string uri, TokenData token)
        {
            var tokenCheckParam = new TokenCheckParam();
            var tokenCheckUrl = uri + "?tokenText=" + token.TokenText;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(tokenCheckUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (content == "[null]")
                    {
                        tokenCheckParam.HasError = true;
                        tokenCheckParam.NotFoundTokenText = true;
                        return tokenCheckParam;
                    }

                    //[]を除去する処理
                    content = _jsonProcessing.RemoveOuterBrackets(content);

                    var receivedTokenData = JsonConvert.DeserializeObject<TokenData>(content);


                    //有効時間内かどうか検証
                    DateTime dt = DateTime.Now;
                    if (receivedTokenData.StartTime < dt && receivedTokenData.EndTime > dt)
                    {
                        tokenCheckParam.IsSuccessed = true;
                        return tokenCheckParam;
                    }
                    else
                    {
                        //有効時間外のTokenだった場合
                        tokenCheckParam.HasError = true;
                        tokenCheckParam.IsOverTimeToken = true;
                    }

                }
                else
                {
                    //該当するtokenTextがDB内に見つからないとき
                    tokenCheckParam.HasError = true;
                    tokenCheckParam.NotFoundTokenText = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return tokenCheckParam;
        }
        #endregion

    }
}
