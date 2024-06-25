namespace YiZan.Model;
//反序列化json数据类型
public class Json_ResJsonClass<T>
{
    public int status { get; set; }
    public string? message { get; set; }
    public T data { get; set; }
}