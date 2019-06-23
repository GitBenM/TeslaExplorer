using System;
using System.Collections.Generic;
using TeslaExplorer.Api;

namespace TeslaExplorer.Models
{
    /// <summary>
    /// Stores an internal list of TeslaApi instances, one instance per user in a dictionary.
    /// </summary>
    public static class UserApiFactory
    {
        public static TeslaApi GetApi(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            if (UserApis.TryGetValue(username, out var api))
                return api;

            var userApi = new TeslaApi(username);

            UserApis.Add(username, userApi);

            return userApi;
        }

        public static void RemoveStaleUsers(DateTimeOffset now)
        {
            foreach (var username in UserApis.Keys)
            {
                if (UserApis.TryGetValue(username, out var userApi) && userApi.HttpClient.DateLastAccess.Add(TimeSpan.FromMinutes(60)) < now) {
                    UserApis.Remove(username);
                }
            }
        }

        private static Dictionary<string, TeslaApi> UserApis { get; set; } = new Dictionary<string, TeslaApi>();
    }
}