using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Net.Http;

namespace CQMacroCreator
{

    class PFStuff
    {

        static public bool logres;
        static public bool DQResult;
        static public string DQlvl;
        static bool _running = true;
        string token;
        string kongID;
        static public string username;
        static public int userID = 0;
        public static int questID;
        public static List<int[]> getResult;
        public static long followers;
        public static int[] lineup;
        public static List<int> questList;
        public static int dungeonLvl;
        public static List<int[]> dungeonLineup;

        public PFStuff(string t, string kid)
        {
            token = t;
            kongID = kid;
            this.getUsername(kongID);
        }

        public void LoginKong()
        {
            PlayFabSettings.TitleId = "E3FA";
            var request = new LoginWithKongregateRequest
            {
                AuthTicket = token,
                CreateAccount = false,
                KongregateId = kongID,
            };
            var loginTask = PlayFabClientAPI.LoginWithKongregateAsync(request);
            while (_running)
            {
                if (loginTask.IsCompleted)
                {
                    var apiError = loginTask.Result.Error;
                    var apiResult = loginTask.Result.Result;

                    if (apiError != null)
                    {
                        logres = false;
                        MessageBox.Show("Failed to log in. Error: " + apiError.ErrorMessage);
                        return;
                    }
                    else if (apiResult != null)
                    {
                        logres = true;
                        return;
                    }
                    _running = true;
                }
                Thread.Sleep(1);
            }
            logres = false;
            return;
        }

        public void GetGameData()
        {
            questList = new List<int>();
            getResult = new List<int[]>();
            var request = new ExecuteCloudScriptRequest()
            {
                RevisionSelection = CloudScriptRevisionOption.Live,
                FunctionName = "status",
                FunctionParameter = new { token = token, kid = kongID }
            };
            var statusTask = PlayFabClientAPI.ExecuteCloudScriptAsync(request);
            bool _running = true;
            while (_running)
            {
                if (statusTask.IsCompleted)
                {
                    var apiError = statusTask.Result.Error;
                    var apiResult = statusTask.Result.Result;

                    if (apiError != null)
                    {
                        return;
                    }
                    else if (apiResult.FunctionResult != null)
                    {
                        JObject json = JObject.Parse(apiResult.FunctionResult.ToString());
                        string el = json["data"]["city"]["daily"]["setup"].ToString();
                        string elvl = json["data"]["city"]["daily"]["hero"].ToString();
                        string levels = json["data"]["city"]["hero"].ToString();
                        string promos = json["data"]["city"]["promo"].ToString();
                        string quests = json["data"]["city"]["quests"].ToString();
                        DQlvl = json["data"]["city"]["daily"]["lvl"].ToString();
                        quests = quests.Substring(1, quests.Length - 2);
                        questList = quests.Split(',').Select(Int32.Parse).ToList();
                        followers = Convert.ToInt64(json["data"]["followers"].ToString());
                        int[] heroLevels = getArray(levels);
                        int[] enemyLineup = getArray(el);
                        int[] enemyLevels = getArray(elvl);
                        int[] heroPromos = getArray(promos);
                        getResult.Add(heroLevels);
                        getResult.Add(enemyLineup);
                        getResult.Add(enemyLevels);
                        getResult.Add(heroPromos);
                        return;
                    }
                    _running = false;
                }
                Thread.Sleep(1);
            }
            return;
        }

        public void sendDQSolution()
        {
            try
            {
                var request = new ExecuteCloudScriptRequest()
                {
                    RevisionSelection = CloudScriptRevisionOption.Live,
                    FunctionName = "pved",
                    FunctionParameter = new { setup = lineup, kid = kongID, max = true }
                };
                var statusTask = PlayFabClientAPI.ExecuteCloudScriptAsync(request);
                bool _running = true;
                while (_running)
                {
                    if (statusTask.IsCompleted)
                    {
                        var apiError = statusTask.Result.Error;
                        var apiResult = statusTask.Result.Result;

                        if (apiError != null)
                        {
                            DQResult = false;
                            return;
                        }
                        else if (apiResult.FunctionResult.ToString().Contains("true"))
                        {
                            getResult = new List<int[]>();
                            JObject json = JObject.Parse(apiResult.FunctionResult.ToString());
                            string el = json["data"]["city"]["daily"]["setup"].ToString();
                            string elvl = json["data"]["city"]["daily"]["hero"].ToString();
                            string levels = json["data"]["city"]["hero"].ToString();
                            string promos = json["data"]["city"]["promo"].ToString();
                            DQlvl = json["data"]["city"]["daily"]["lvl"].ToString();
                            int[] heroLevels = getArray(levels);
                            int[] enemyLineup = getArray(el);
                            int[] enemyLevels = getArray(elvl);
                            int[] heroPromos = getArray(promos);
                            getResult.Add(heroLevels);
                            getResult.Add(enemyLineup);
                            getResult.Add(enemyLevels);
                            getResult.Add(heroPromos);

                            DQResult = true;
                            return;
                        }
                        _running = false;
                    }
                    Thread.Sleep(1);
                }
                DQResult = false;
                return;
            }
            catch (Exception ex)
            {
                Task.Run(() => sendLog("CQMC " + ex.Message));
                DQResult = false;
                return;
            }
        }

        public void sendDungSolution()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                RevisionSelection = CloudScriptRevisionOption.Live,
                FunctionName = "dungeon",
                FunctionParameter = new { setup = lineup, max = true }
            };
            var statusTask = PlayFabClientAPI.ExecuteCloudScriptAsync(request);
            bool _running = true;
            while (_running)
            {
                if (statusTask.IsCompleted)
                {
                    var apiError = statusTask.Result.Error;
                    var apiResult = statusTask.Result.Result;

                    if (apiError != null)
                    {
                        DQResult = false;
                        return;
                    }
                    else if (apiResult.FunctionResult != null && apiResult.FunctionResult.ToString().Contains("true"))
                    {
                        DQResult = true;
                        return;
                    }
                    _running = false;
                }
                Thread.Sleep(1);
            }
            DQResult = false;
            return;
        }

        public void sendQuestSolution()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                RevisionSelection = CloudScriptRevisionOption.Live,
                FunctionName = "pve",
                FunctionParameter = new { setup = lineup, id = questID }
            };
            var statusTask = PlayFabClientAPI.ExecuteCloudScriptAsync(request);
            bool _running = true;
            while (_running)
            {
                if (statusTask.IsCompleted)
                {
                    var apiError = statusTask.Result.Error;
                    var apiResult = statusTask.Result.Result;

                    if (apiError != null)
                    {
                        DQResult = false;
                        return;
                    }
                    else if (apiResult != null && apiResult.FunctionResult.ToString().Contains("true"))
                    {
                        DQResult = true;
                        return;
                    }
                    _running = false;
                }
                Thread.Sleep(1);
            }
            DQResult = false;
            return;
        }

        public void sendWB()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                RevisionSelection = CloudScriptRevisionOption.Specific,
                SpecificRevision = 190,
                FunctionName = "fightWB",
                FunctionParameter = new { setup = lineup, kid = kongID }
            };
            var statusTask = PlayFabClientAPI.ExecuteCloudScriptAsync(request);
            bool _running = true;
            while (_running)
            {
                if (statusTask.IsCompleted)
                {
                    var apiError = statusTask.Result.Error;
                    var apiResult = statusTask.Result.Result;

                    if (apiError != null)
                    {
                        DQResult = false;
                        return;
                    }
                    else if (apiResult != null && apiResult.FunctionResult.ToString().Contains("true"))
                    {
                        DQResult = true;
                        return;
                    }
                    _running = false;
                }
                Thread.Sleep(1);
            }
            DQResult = false;
            return;
        }

        private static int[] getArray(string s)
        {
            s = Regex.Replace(s, @"\s+", "");
            s = s.Substring(1, s.Length - 2);
            int[] result = s.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            return result;
        }

        internal static void getDungeonData(string id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://cosmosquest.net/public.php?kid=" + id);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                JObject json = JObject.Parse(content);
                var dungData = json["dungeon"];
                dungeonLineup = new List<int[]>();
                dungeonLvl = int.Parse(dungData["lvl"].ToString());

                string el = dungData["setup"].ToString();
                string elvl = dungData["hero"].ToString();

                int[] enemyLineup = getArray(el);
                int[] enemyLevels = getArray(elvl);
                dungeonLineup.Add(enemyLineup);
                dungeonLineup.Add(enemyLevels);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public async void getUsername(string id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://api.kongregate.com/api/user_info.json?user_id=" + id);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject json = JObject.Parse(content);
                username = json["username"].ToString();

                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string> { { "uget", username } };
                    var cont = new FormUrlEncodedContent(values);
                    var resp = await client.PostAsync("http://dcouv.fr/cq.php", cont);
                    var respString = await resp.Content.ReadAsStringAsync();
                    userID = int.Parse(respString);
                }
            }
            catch (Exception)
            {
                username = null;
            }
        }
        public async Task<bool> sendLog(string e)
        {
            try
            {
                var d = new Dictionary<string, string>();
                d["p"] = userID.ToString();
                d["e"] = e;
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string> { { "ierr", JsonConvert.SerializeObject(d) } };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("http://dcouv.fr/cq.php", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
            }
            return true;
        }
    }
}
