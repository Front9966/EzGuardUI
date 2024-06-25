global using YiZan.Model;
global using YiZan.ViewModel;
global using YiZan.View;
global using System;

using System.Text.Json;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Alerts;

public class All 
{
    public static int LoginCode = 0;            //登录状态 0:未登录 1:已登录 2:退出登录
    public static string Token { get; set; }    //用户Token
    public static string nicname { get; set; }  //用户昵称
    public static string qqNumber { get; set; } //用户QQ
    public static string KuaishouAccount_Get_Like { get; set; }    //快手获赞账号Userid
    public static string KuaishouAccount_Like { get; set; }    //快手点赞账号Userid

    public static string hostname = "https://yizan66.asia";         //域名

    public static bool IsAndroid() =>
    DeviceInfo.Current.Platform == DevicePlatform.Android;

    public static bool IsIOS() =>
        DeviceInfo.Current.Platform == DevicePlatform.iOS;

    public static int isV = 1;

    //检测更新
    public static async Task GetUpData(ContentPage obj)
    {
        HttpClient httpClient = new HttpClient();
        try
        {
            HttpResponseMessage res = await httpClient.GetAsync(All.hostname + "/api/appVersion");
            if (res.IsSuccessStatusCode)
            {
                string res_string = await res.Content.ReadAsStringAsync();
                var resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_UpData>>(res_string);
                if (resJsonData.data.appVersion != AppInfo.Current.VersionString)
                {
                    //检测到版本更新
                    //var a = new UpDataPopup(resJsonData.data.appVersion, resJsonData.data.androidAddress, resJsonData.data.iosAddress, resJsonData.data.openUpgrade);
                    var a = new UpDataPopup(resJsonData.data.appVersion, resJsonData.data.androidAddress, resJsonData.data.iosAddress, "0");
                    obj.ShowPopup(a);
                }
                else
                {
                    //await MainThread.InvokeOnMainThreadAsync(() =>
                    //{
                        //Toast.Make("已是最新版本").Show();
                    //});

                }
            }
        }
        catch (HttpRequestException e)
        {
            await MainThread.InvokeOnMainThreadAsync(() => {
                Toast.Make("请求数据失败，网络异常！！！").Show();
            });
        }

    }

    //------Json数据类型----------
    private class Json_UpData
    {
        public string? openUpgrade { get; set; }
        public string? androidAddress { get; set; }
        public string? appVersion { get; set; }
        public string? iosAddress { get; set; }
    }

}
