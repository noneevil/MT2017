namespace WebSite.Interface.Sina
{
    public interface IHttpRequestMethod
    {
        string Request(string uri, string postData);        
    }
}