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
        /// ��c���P��擾��API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="mid">��cID</param>
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

                    //[]���������鏈��
                    content = _jsonProcessing.RemoveOuterBrackets(content);

                    //json����I�u�W�F�N�g��
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
        /// ��c���S���擾��API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="userId">���[�U�[ID</param>
        /// <returns>��c���̃��X�g</returns>
        public async Task<GetMeetingsParam> GetMeetingsDataAsync(string uri, string userId)
        {
            var meetingsData = new List<MeetingData>();
            var getMeetingsParam = new GetMeetingsParam();

            //userId����id���擾
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

                        //��c�Ǘ��҂��ǂ������ꂼ���meeting���f���ɒʒm
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
        /// ��c����V�K�o�^����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="meetingData">�V�K�o�^�����c���</param>
        /// <param name="labels">��c�ɕR�Â����x�����X�g</param>
        /// <returns>CreateMeetingParam</returns>
        public async Task<CreateMeetingParam> CreateMeetingDataAsync(string uri, MeetingData meetingData, ObservableCollection<MeetingLabelData> labels)
        {

            var json = JsonConvert.SerializeObject(meetingData);
            var jobj = JObject.Parse(json);

            //MeetingData���f������JSON���ɕs�v�ȑ������폜
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

        /// <summary>
        /// ��c����1���폜����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="mid">�폜�����cID</param>
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
        /// ��c�����X�V����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="meeting">�X�V�Ώۂ̉�c�f�[�^</param>
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


                //���������ꍇ
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Param�𐬌���
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
        /// �w���c�ɂ�����w�胆�[�U�[�������x�����Q���擾
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="mid">���x�����擾����Ώۂ̉�cID</param>
        /// <param name="uid">���x���ɑ΂��鍀�ڂ��擾����Ώۂ̃��[�U�[ID</param>
        /// <returns>GetMeetingLabelsParam</returns>
        public async Task<GetMeetingLabelsParam> GetMeetingLabelsDataAsync(string uri, int mid, int uid)
        {
            GetMeetingLabelsParam getMeetingLabelsParam = new GetMeetingLabelsParam();

            //mid���N�G���X�g�����O�ɉ�����
            uri = uri + "?mid=" + mid;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    getMeetingLabelsParam.MeetingLabelDatas = JsonConvert.DeserializeObject<List<MeetingLabelData>>(content);

                    //�擾�������x���Q���ꂼ�ꎩ���̍��ڂ��擾����
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
        /// ��c�ɑ΂��郉�x����V�K�ǉ�����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="labelName">�ǉ����郉�x���̖��O</param>
        /// <param name="mid">���x����ǉ�����Ώۂ̉�cID</param>
        /// <returns>CreateMeetingLabelParam</returns>
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

        #endregion


        #region MeetingLabelItem


        /// <summary>
        /// ��c���x���ɑ΂��鍀�ڂ�ǉ�����API�̃R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="meetingLabelItem">�ǉ����鍀�ڂ̏��</param>
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
        /// �w�肵����c�̃��x���ɑ΂��郆�[�U�[�������ڌQ�̎擾API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="lid">���ڌQ���擾���郉�x��ID</param>
        /// <param name="uid">���ڌQ���擾����Ώۂ̕ێ��҂̃��[�U�[ID</param>
        /// <returns>GetMeetingLabelItemsParam</returns>
        public async Task<GetMeetingLabelItemsParam> GetMeetingLabelItemsDataAsync(string uri, int lid, int uid)
        {
            GetMeetingLabelItemsParam getMeetingLabelItemsParam = new GetMeetingLabelItemsParam();

            //mid���N�G���X�g�����O�ɉ�����
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
        /// ��c���x���ɑ΂��鍀�ڏ���1���폜����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="iid">�폜�Ώۂ̍���ID</param>
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
        /// ��c�����̍ۂ̎Q���ҏ���o�^����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="uid">�Q���҂̃��[�U�[ID</param>
        /// <param name="mid">�����Ώۂ̉�cID</param>
        /// <returns>CreateParticipateParam</returns>
        public async Task<CreateParticipateParam> CreateParticipateDataAsync(string uri, int uid, int mid)
        {
            var participantData = new ParticipantData(uid, mid);
            var json = JsonConvert.SerializeObject(participantData);

            var jobj = JObject.Parse(json);
            //MeetingData���f������JSON���ɕs�v�ȑ������폜
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
        /// �w�胆�[�U�[����c�ɎQ���ς݂��ǂ����m�F����API�R�[��(���[�U�[ID�̂�)
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="uid">�m�F����Ώۂ̃��[�U�[ID</param>
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
                    //���[�U�[�����������ꍇ
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
        /// �w�胆�[�U�[����c�ɎQ���ς݂��ǂ����m�F����API�R�[��(���[�U�[ID�A��c���ID)
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="uid">�T���Ώۂ̃��[�U�[ID</param>
        /// <param name="mid">�T���Ώۂ̉�cID</param>
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
                    //���[�U�[�����������ꍇ
                    //list -> �v�f�̏���
                    //[]���������鏈��
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
        /// �w���c�ɎQ�����̃��[�U�[��S���擾����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="mid">�T���Ώۂ̉�cID</param>
        /// <returns>GetParitipantsParam</returns>
        public async Task<GetParticipantsParam> GetParticipantsDataAsync(string uri, int mid)
        {
            var getParticipantsParam = new GetParticipantsParam();

            //mid���N�G���X�g�����O�ɉ�����
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

                    //View�p�Ɏ��g��userId�A�e���x�����ڂ��擾
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
        /// ��c�Q���ҏ���1���폜����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="uid">�폜�ΏێQ���҂̃��[�U�[ID</param>
        /// <param name="mid">�Q���҂�����������cID</param>
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
        /// ��c�̎Q���ҏ����X�V����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="participant">�Q���҂̍X�V���e���</param>
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


                //���������ꍇ
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Param�𐬌���
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
        /// ���[�U�[�����擾����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="userId">�Ώۂ̃��[�U�[ID</param>
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
                    //[]���������鏈��
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
        /// ��ӂ̎��R��uid���烆�[�U�[�����擾����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="uid">���[�U�[��uid</param>
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
        /// /���[�U�[����V�K�o�^����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="userId">�o�^���郆�[�U�[ID</param>
        /// <param name="password">�o�^����p�X���[�h</param>
        /// <returns>SignUpParam</returns>
        public async Task<SignUpParam> SignUpUserDataAsync(string uri, string userId, string password)
        {
            var user = new UserData(userId, password);
            var json = JsonConvert.SerializeObject(user);

            var signUpParam = new SignUpParam();

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //�e�[�u������userId�����݂��邩�ǂ����`�F�b�N
                if (await CheckUserDataAsync(uri, userId))

                {
                    //���݂��Ă����ꍇPOST�����s�ŏI��
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
        /// ���O�C��API�̃R�[��
        /// </summary>
        /// <param name="uri">�R�[������API</param>
        /// <param name="userId">���O�C�����̃��[�U�[ID</param>
        /// <param name="password">���O�C�����̃p�X���[�h</param>
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


        /// <summary>
        /// ���[�U�[��񂪑��݂��邩�ǂ����`�F�b�N����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������API</param>
        /// <param name="userId">�m�F�Ώۂ̃��[�U�[ID</param>
        /// <returns>���݂����True�A���Ȃ����False</returns>
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
        /// �g�[�N����񂪐��������̂��ǂ����ƍ�����API�R�[��
        /// </summary>
        /// <param name="uri">�R�[������URL</param>
        /// <param name="token">�g�[�N�����</param>
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

                    //[]���������鏈��
                    content = _jsonProcessing.RemoveOuterBrackets(content);

                    var receivedTokenData = JsonConvert.DeserializeObject<TokenData>(content);


                    //�L�����ԓ����ǂ�������
                    DateTime dt = DateTime.Now;
                    if (receivedTokenData.StartTime < dt && receivedTokenData.EndTime > dt)
                    {
                        tokenCheckParam.IsSuccessed = true;
                        return tokenCheckParam;
                    }
                    else
                    {
                        //�L�����ԊO��Token�������ꍇ
                        tokenCheckParam.HasError = true;
                        tokenCheckParam.IsOverTimeToken = true;
                    }

                }
                else
                {
                    //�Y������tokenText��DB���Ɍ�����Ȃ��Ƃ�
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
