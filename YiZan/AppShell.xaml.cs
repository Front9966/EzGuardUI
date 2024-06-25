using System.Text.Json;

namespace YiZan
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            var dataPath = FileSystem.Current.AppDataDirectory + "/data.ini";
            string user = "",password = "";
            if (File.Exists(dataPath))
            {
                All.LoginCode = 1;
                StreamReader streamReader = new StreamReader(dataPath);
                string text;
                for (int i = 1; i < 3; i++)
                {
                    if (i == 1)
                    {
                        user = streamReader.ReadLine();
                    }
                    else if (i == 2)
                    {
                        password = streamReader.ReadLine();
                    }
                }
            }
            
            //启动判断
            if (All.LoginCode == 0)//未登录
            {
                ContentPage obj = new HomePage();
                //进入登录主页
                Navigation.PushAsync(obj);
            }
            else if (All.LoginCode == 1)//已登录
            {
                //不操作
                Task.Run(async () =>
                {
                    HttpClient client = new HttpClient();
                    var postInfo = new Dictionary<string, string>();
                    postInfo.Add("account", user);
                    postInfo.Add("password", password);
                    All.qqNumber = user;
                    HttpResponseMessage res = await client.PostAsync(All.hostname + "/api/auth/login", new FormUrlEncodedContent(postInfo));
                    string res_string = await res.Content.ReadAsStringAsync();
                    var resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_Login>>(res_string);
                    if (resJsonData.status == 200)
                    {
                        All.Token = resJsonData.data.token;
                        res = await client.GetAsync("https://api.usuuu.com/qq/" + user);
                        res_string = await res.Content.ReadAsStringAsync();
                        var resQQInfoJson = JsonSerializer.Deserialize<Json_resJson>(res_string);
                        if (resQQInfoJson.code == 200)
                        {
                            All.nicname = resQQInfoJson.data["name"];
                            File.WriteAllText("LoginInfo.ini", res_string);
                        }
                        else 
                        {
                            res_string = File.ReadAllText("LoginInfo.ini");
                            resQQInfoJson = JsonSerializer.Deserialize<Json_resJson>(res_string);
                            All.nicname = resQQInfoJson.data["name"];
                        }
                    }
                    else
                    {
                        //进入登录主页
                        Navigation.PushAsync(new HomePage());
                    }

                });
            }
        }

        //Json数据类型
        public class Json_Login
        {
            public Json_LoginInfo user { get; set; }
            public string? token { get; set; }
            public int exp { get; set; }
            public long expires_time { get; set; }
        }
        public class Json_LoginInfo
        {
            public int uid { get; set; }
            public int wechat_user_id { get; set; }
            public string? account { get; set; }
            public int sex { get; set; }
            public string? nickname { get; set; }
            public string? avatar { get; set; }
            public string? phone { get; set; }
            public string? cancel_time { get; set; }
            public string? now_money { get; set; }
            public string? spread_limit { get; set; }
            public int brokerage_level { get; set; }
            public string? user_type { get; set; }
            public string? promoter_time { get; set; }
            public int is_promoter { get; set; }
            public int pay_count { get; set; }
            public int spread_pay_count { get; set; }
            public string? spread_pay_price { get; set; }
            public int integral { get; set; }
            public int member_level { get; set; }
            public int member_value { get; set; }
            public int count_start { get; set; }
            public int count_fans { get; set; }
        }
        public class Json_qqInfo
        {
            public string? qq { get; set; }
            public string? name { get; set; }
            public string? email { get; set; }
            public string? avatar { get; set; }
        }
        public class Json_resJson
        {
            public int code { get; set; }
            public string? msg { get; set; }
            public Dictionary<string,string> data { get; set; }
        }
    }
}
