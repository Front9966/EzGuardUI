using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;
using System.Text.Json;
namespace YiZan.ViewModel;
public class FirstPageModel : INotifyPropertyChanged
{
    //�ֲ�ͼ�󶨲���
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
    //����󶨲���
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
    //����ݰ�
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

    //��ʼ��
    public FirstPageModel()
    {
        //��ʼ�����λ
        GetTpData();
        //��ȡ����
        GetNoticeData();
        //��ȡ�����
        GetActivityContentData();
    }

    //-----------�����ȡ����------------
    //��ȡ���ͼ
    public async void GetTpData()
    {
        //����HTTP�ͻ���
        HttpClient httpClient = new HttpClient();
        try
        {
            //��ȡ����ֲ�ͼ��Json����
            HttpResponseMessage res = await httpClient.GetAsync(All.hostname + "/api/index/banner");
            if (res.IsSuccessStatusCode)
            {
                //�����л�JSON���ݵ�c#��������
                string content = await res.Content.ReadAsStringAsync();
                var resDataType = JsonSerializer.Deserialize<Json_ResJsonClass<IList<Json_typeClass>>>(content);
                //��ʼ��ֵ����
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
            Toast.Make("��������ʧ�ܣ�������ܴ����쳣Ŷ").Show();
        }
    }
    //��ȡ����
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
            Toast.Make("��������ʧ�ܣ�������ܴ����쳣Ŷ").Show();
        }
    }
    //��ȡ�����
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
            Toast.Make("��������ʧ�ܣ�������ܴ����쳣Ŷ").Show();
        }
    }

    //-----------JSON��������-----------
    //��Ӧ�ֲ����ͼ��JSON��������
    public class Json_typeClass
    {
        public string? name { get; set; }
        public string? pic { get; set; }
        public string? url { get; set; }
        public int group_data_id { get; set; }
        public int group_mer_id { get; set; }
    }
    //��Ӧ�����JSON��������
    public class Json_NoticeClass
    {
        public int id { get; set; }
        public string? title { get; set; }
        public long add_time { get; set; }
        public int is_del { get; set; }
        public string? content { get; set; }
    }
    //��Ӧ����ݵ�JSON DATA������
    public class Json_ActivityDataClass
    {
        public int count { get; set; }
        public IList<Json_ActivityContentClass> list { get; set; }
    }
    //��Ӧ����ݵ�JSON������
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