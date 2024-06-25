using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;
using System.Text.Json;
namespace YiZan.ViewModel;
public class FirstPageModel : INotifyPropertyChanged
{
    //轮播图绑定参数
    private List<TpClass> _tpClasses;
    public List<TpClass> tpClasses
    {
        get => _tpClasses;
        set
        {
            _tpClasses = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tpClasses"));
        }
    }
    //公告绑定参数
    private string _notice;
    public string notice
    {
        get => _notice;
        set 
        {
            _notice = value;
            PropertyChanged.Invoke(this,new PropertyChangedEventArgs("notice"));
        }
    }
    //活动内容绑定
    private List<ActivityContent> _activityContents { get; set; }
    public List<ActivityContent> activityContents
    {
        get => _activityContents;
        set
        {
            _activityContents = value;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("activityContents"));
        }
    }

    //初始化
    public FirstPageModel()
    {
        //初始化广告位
        GetTpData();
        //获取公告
        GetNoticeData();
        //获取活动内容
        GetActivityContentData();
    }

    //-----------网络获取数据------------
    //获取广告图
    public async void GetTpData()
    {
        //创建HTTP客户端
        HttpClient httpClient = new HttpClient();
        try
        {
            //获取广告轮播图的Json数据
            HttpResponseMessage res = await httpClient.GetAsync(All.hostname + "/api/index/banner");
            if (res.IsSuccessStatusCode)
            {
                //反序列化JSON数据到c#数据类型
                string content = await res.Content.ReadAsStringAsync();
                var resDataType = JsonSerializer.Deserialize<Json_ResJsonClass<IList<Json_typeClass>>>(content);
                //开始赋值内容
                List<TpClass> _temp = new List<TpClass>();
                for (int i = 0; i < resDataType.data.Count; i++)
                {
                    _temp.Add(new TpClass() { imagePath = resDataType.data[i].pic, Title = resDataType.data[i].name });
                }
                tpClasses = _temp;
            }
        }
        catch (HttpRequestException e)
        {
            Toast.Make("请求数据失败，网络可能存在异常哦").Show();
        }
    }
    //获取公告
    public async void GetNoticeData() 
    {
        string url_Path = All.hostname + "/api/notice/index";
        HttpClient httpClient = new HttpClient();
        try
        {
            HttpResponseMessage res = await httpClient.GetAsync(url_Path);
            if (res.IsSuccessStatusCode)
            {
                string res_string = await res.Content.ReadAsStringAsync();
                var resJsonClass = JsonSerializer.Deserialize<Json_ResJsonClass<Json_NoticeClass>>(res_string);
                notice = resJsonClass.data.content;
            }

        }
        catch (HttpRequestException e)
        {
            Toast.Make("请求数据失败，网络可能存在异常哦").Show();
        }
    }
    //获取活动内容
    public async void GetActivityContentData() 
    {
        var url_Path = All.hostname + "/api/active/lst?page=1&limit=10";
        HttpClient httpClient = new HttpClient();
        try 
        {
            HttpResponseMessage res = await httpClient.GetAsync(url_Path);
            if (res.IsSuccessStatusCode) 
            {
                string content = await res.Content.ReadAsStringAsync();
                var resJsonClass = JsonSerializer.Deserialize<Json_ResJsonClass<Json_ActivityDataClass>>(content);
                List<ActivityContent> _temp = new List<ActivityContent>();
                for (int i = 0; i < resJsonClass.data.count; i++) 
                {
                    _temp.Add(new ActivityContent() { Content = resJsonClass.data.list[i].content, ImageUrl = resJsonClass.data.list[i].img, Title = resJsonClass.data.list[i].title });
                }
                activityContents = _temp;
            }
        }
        catch (HttpRequestException e)
        {
            Toast.Make("请求数据失败，网络可能存在异常哦").Show();
        }
    }

    //-----------JSON数据类型-----------
    //响应轮播广告图的JSON类型数据
    public class Json_typeClass
    {
        public string? name { get; set; }
        public string? pic { get; set; }
        public string? url { get; set; }
        public int group_data_id { get; set; }
        public int group_mer_id { get; set; }
    }
    //响应公告的JSON类型数据
    public class Json_NoticeClass
    {
        public int id { get; set; }
        public string? title { get; set; }
        public long add_time { get; set; }
        public int is_del { get; set; }
        public string? content { get; set; }
    }
    //响应活动内容的JSON DATA数据类
    public class Json_ActivityDataClass
    {
        public int count { get; set; }
        public IList<Json_ActivityContentClass> list { get; set; }
    }
    //响应活动内容的JSON数据类
    public class Json_ActivityContentClass
    {
        public int id { get; set; }
        public string? title { get; set; }
        public long add_time { get; set; }
        public int is_del { get; set; }
        public string? content { get; set; }
        public string? img { get; set; }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}