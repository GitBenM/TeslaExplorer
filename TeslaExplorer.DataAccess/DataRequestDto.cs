using Newtonsoft.Json;
using System;

namespace TeslaExplorer.DataAccess
{
    public class DataRequest<T>
    {
        [JsonProperty("response")]
        public T Response { get; set; }
    }
}
