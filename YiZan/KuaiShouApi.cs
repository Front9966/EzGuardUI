
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace YiZan;
public class KuaiShouApi
{
    public string loginTimeString;
    public QrInfo Qr;
    public LoginInfo loginInfo;
    public string? eid;
    public string did;
    //获取二维码
    public void GetQr()
    {
        did = GetDid();
        //did = "web_f01b438b0da431377ca2fcbe22047ada";

        loginTimeString = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var url = "https://id.kuaishou.com/rest/c/infra/ks/qr/start";
        var postContent = new Dictionary<string, string>();
        postContent.Add("sid", "kuaishou.server.web");
        postContent.Add("channelType", "UNKNOWN");
        HttpClient httpClient = new HttpClient();

        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Headers.Add("Cookie", "did=" + did + ";didv=" + loginTimeString);
        httpRequestMessage.Content = new FormUrlEncodedContent(postContent);

        var res = httpClient.SendAsync(httpRequestMessage).Result;
        var res_string = res.Content.ReadAsStringAsync().Result;
        JObject a = JObject.Parse(res_string);
        Qr = new QrInfo() { sid = (string)a["sid"], imageData = (string)a["imageData"], qrLoginSignature = (string)a["qrLoginSignature"], qrLoginToken = (string)a["qrLoginToken"] };
    }
    //返回1：扫描成功  707：已过期
    public int GetLoginCode(string qrLoginToken, string qrLoginSignature)
    {
        var url = "https://id.kuaishou.com/rest/c/infra/ks/qr/scanResult";
        var postContent = new Dictionary<string, string>() {
            { "qrLoginToken",qrLoginToken },
            { "qrLoginSignature",qrLoginSignature},
            { "channelType","UNKNOWN"},
            { "encryptHeaders",""}
        };
        HttpClient httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(61000);
        try
        {
            var res = httpClient.PostAsync(url, new FormUrlEncodedContent(postContent)).Result.Content.ReadAsStringAsync().Result;
            JObject resJsonData = JObject.Parse(res);
            if (((int)resJsonData["result"]) != 707)
            {
                JObject aaa = (JObject)resJsonData["user"];
                eid = (string)aaa["eid"];
                return (int)resJsonData["result"];
            }
            else
            {
                return 707;
            }
        }
        catch (Exception e) 
        {
            
        }
        return 707;
    }
    //获取Cookies
    public async Task<string> GetCookie(string qrLoginToken, string qrLoginSignature)
    {
        string url, res_string, cookie, QrToken, timeString;
        HttpResponseMessage res;
        Dictionary<string, string> postContent;
        JObject resJsonData;
        //获取QRtoken
        url = "https://id.kuaishou.com/rest/c/infra/ks/qr/acceptResult";
        postContent = new Dictionary<string, string>()
        {
            { "qrLoginToken", qrLoginToken },
            { "qrLoginSignature", qrLoginSignature },
            { "sid","kuaishou.server.web" },
            { "channelType", "UNKNOWN" },
            { "encryptHeaders", "" }
        };
        HttpClient httpClient = new HttpClient();
        res = await httpClient.PostAsync(url, new FormUrlEncodedContent(postContent));
        res_string = res.Content.ReadAsStringAsync().Result;
        resJsonData = JObject.Parse(res_string);
        QrToken = (string)resJsonData["qrToken"];

        //获取callback
        url = "https://id.kuaishou.com/pass/kuaishou/login/qr/callback";
        postContent = new Dictionary<string, string>()
        {
            { "qrToken",QrToken },
            { "sid","kuaishou.server.web" },
            { "channelType","UNKNOWN" },
            { "encryptHeaders","" }
        };
        res = httpClient.PostAsync(url, new FormUrlEncodedContent(postContent)).Result;
        res_string = res.Content.ReadAsStringAsync().Result;
        resJsonData = JObject.Parse(res_string);
        loginInfo = new LoginInfo()
        {
            Web_at = (string)resJsonData["kuaishou.server.web.at"],
            Web_st = (string)resJsonData["kuaishou.server.web_st"],
            passToken = (string)resJsonData["passToken"],
            ssecurity = (string)resJsonData["ssecurity"],
            userId = (long)resJsonData["userId"]
        };

        //获取ph参数
        timeString = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        url = "https://www.kuaishou.com/rest/infra/sts?followUrl=https://passport.kuaishou.com/pc/account/passToken/result?successful=true&id=SSO_" + timeString + "&for=passTokenSuccess&failUrl=https://passport.kuaishou.com/pc/account/passToken/result?successful=false&id=SSO_" + timeString + "&for=passTokenSuccess&setRootDomain=false";
        url = System.Web.HttpUtility.UrlEncode(url);
        url += "&__loginPage=" + System.Web.HttpUtility.UrlEncode("https://passport.kuaishou.com/pc/account/passToken/result?successful=false&id=SSO_" + timeString + "&for=pullTokenFail");
        url += "&sid=" + System.Web.HttpUtility.UrlEncode("kuaishou.server.web");
        url = "https://id.kuaishou.com/pass/kuaishou/login/passToken?callback=" + url;
        var Heads = new HttpClientHandler()
        {
            AllowAutoRedirect = false
        };
        var client = new HttpClient(Heads);
        cookie = "userId=" + loginInfo.userId + ";";
        cookie += "passToken=" + loginInfo.passToken;
        client.DefaultRequestHeaders.Add("Cookie", cookie);
        var a = client.GetAsync(url).Result;

        //跳转获取ph
        client = new HttpClient(Heads);
        cookie = "did=" + did + ";";
        cookie += "didv=" + loginTimeString + ";kpf=PC_WEB;clientid=3;kpn=KUAISHOU_VISION;userId=" + loginInfo.userId.ToString();
        url = a.Headers.Location.ToString();
        client.DefaultRequestHeaders.Add("Cookie", cookie);
        client.DefaultRequestHeaders.Add("Host", "www.kuaishou.com");
        client.DefaultRequestHeaders.Add("Referer", "https://www.kuaishou.com/new-reco");
        res = client.GetAsync(url).Result;
        HttpResponseHeaders b = res.Headers;
        string ph = b.GetValues("Set-Cookie").Last();
        string reg = "ph=(.+);";
        Match math = Regex.Match(ph, reg);
        ph = math.Groups[1].Value;

        //合成Cookies
        cookie = "did=" + did + ";";
        cookie += "didv=" + loginTimeString + ";kpf=PC_WEB;clientid=3;kpn=KUAISHOU_VISION;";
        cookie += "userId=" + loginInfo.userId.ToString() + ";";
        cookie += "kuaishou.server.web_st=" + loginInfo.Web_st + ";";
        cookie += "kuaishou.server.web_ph=" + ph;
        return cookie;
    }
    //获取DID
    public string GetDid() 
    {
        var url = "https://www.kuaishou.com/?isHome=1";
        var client = new HttpClient();
        var res = client.GetAsync(url).Result;
        HttpResponseHeaders b = res.Headers;
        var did_string = b.GetValues("Set-Cookie").Last();
        string reg = "did=([^;]+);";
        Match math = Regex.Match(did_string, reg);
        did_string = math.Groups[1].Value;
        return did_string;
    }
    //二维码信息
    public class QrInfo
    {
        public string? imageData { get; set; }
        public string? qrLoginSignature { get; set; }
        public string? qrLoginToken { get; set; }
        public string? sid { get; set; }
    }
    //登录信息
    public class LoginInfo
    {
        public string? Web_at { get; set; }
        public string? Web_st { get; set; }
        public string? passToken { get; set; }
        public string? ssecurity { get; set; }
        public long? userId { get; set; }
    }
}

