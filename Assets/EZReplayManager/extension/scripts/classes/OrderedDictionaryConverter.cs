using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using MyCollections;

class OrderedDictionaryConverter<T> : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return (objectType == typeof(OrderedDictionary<string, T>));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray array = JArray.Load(reader);
        OrderedDictionary<int, T> dict = new OrderedDictionary<int, T>();
        foreach (JObject obj in array.Children<JObject>()) {
            int key = int.Parse(obj["Key"].ToString());
            T val = obj["Value"].ToObject<T>();
            dict.Add(key, val);
        }
        return dict;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        OrderedDictionary<int, T> dict = (OrderedDictionary<int, T>)value;
        JArray array = new JArray();
        foreach (KeyValuePair<int, T> kvp in dict) {
            JObject obj = new JObject();
            obj.Add("Key", kvp.Key);
            obj.Add("Value", JToken.FromObject(kvp.Value));
            array.Add(obj);
        }
        array.WriteTo(writer);
    }
}