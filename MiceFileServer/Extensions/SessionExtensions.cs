using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace MiceFileClient.Extensions
{
	public static class SessionExtensions
	{
        /// <summary>
        /// Sets the given key and value of type <T> to the session storage.
        /// </summary>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }
        /// <summary>
        /// Get object of type <T> with the given key from the session storage.
        /// </summary>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
