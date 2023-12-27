//using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Helper
{
  public static class SessionExtension
  {
 /*    public static void Set<T>(this ISession session, string key, T value)
    {
      session.SetString(key, JsonSerializer.Serialize(value));
    }
    public static T Get<T>(this ISession session, string key)
    {
      var value = session.GetString(key);
      return value == null ? default : JsonSerializer.Deserialize<T>(value);
    } */
    public static void SetJson<T>(this ISession session, string key, T value)
    {
      session.SetString(key, JsonConvert.SerializeObject(value));
    }
    public static T GetJson<T>(this ISession session, string key)
    {
      var sessionData = session.GetString(key);
      return sessionData == null
          ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
    }
  }
}