using System.Text.Json;

namespace Clothing_boutique_web.Areas.Admin.Models
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var user = session.Get(key);
            return user == null ? default : JsonSerializer.Deserialize<T>(key);
        }
    }
}
