using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace Client.classes
{
    public class Json
    {
        public JsonType Type { get; set; }
        public object obj { get; set; }
        // public string json { get; set; }
        
        public Json(){}
        public Json(JsonType type, object obj)
        {
            Type = type;
            this.obj = obj;
        }
        
        private string Serialize()
        {
            return JsonConvert.SerializeObject(this);
            // if (obj != null)
            // {
            //     json = JsonConvert.SerializeObject(this);
            // }else{
            //     throw new JsonObjectNotFoundException(Type.ToString());
            // }
        }
        private Json Deserialize<T>(byte[] data)
        {
            return JsonConvert.DeserializeObject<Json>(Encoding.ASCII.GetString(data));

            // if (json != null)
            // {
            //     var here = JsonConvert.DeserializeObject<Json>(json);
            //     if (here == null) return;
            //     json = here.json;
            //     Type = here.Type;
            //     obj = JsonConvert.DeserializeObject<T>(json);
            // }else{
            //     throw new JsonStringNotFoundException(Type.ToString());
            // }
        }
        // private byte[] GetBytes()
        // {
        //     return Encoding.ASCII.GetBytes(json);
        // }
        // private void SetBytes(byte[] bytes)
        // {
        //     json = Encoding.ASCII.GetString(bytes);
        // }
        
        public byte[] Send()
        {
            var res = Serialize();
            Debug.WriteLine(res);
            return Encoding.ASCII.GetBytes(res);
            // Serialize();
            // Debug.WriteLine(json);
            // return GetBytes();
        }
        public Json Receive(byte[] bytes)
        {
            return Deserialize<Json>(bytes);
            // SetBytes(bytes);
            // Deserialize<object>();
        }
    }

    public enum JsonType
    {
        Card = 1,
        Player = 2,
        Game = 3,
    }
    
}