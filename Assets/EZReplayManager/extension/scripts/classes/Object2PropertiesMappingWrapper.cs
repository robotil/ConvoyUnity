using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Text;
using System.IO.Compression;

#if !IS_UNLICENSED
[Serializable()]
[JsonObject(IsReference = true)]
public class Object2PropertiesMappingWrapper /*: ISerializable*/ {
#else
[Serializable()]
public sealed class Object2PropertiesMappingListWrapper : ISerializable {	
#endif
    [JsonProperty("object2PropertiesMappings")]
    public List<Object2PropertiesMapping> object2PropertiesMappings = new List<Object2PropertiesMapping>();
    [JsonProperty("EZR_VERSION")]
    public string EZR_VERSION = EZReplayManager.EZR_VERSION;
    [JsonProperty("recordingInterval")]
    public float recordingInterval;

	//serialization constructor
	/*protected Object2PropertiesMappingWrapper(SerializationInfo info,StreamingContext context) {
	    object2PropertiesMappings = (List<Object2PropertiesMapping>)info.GetValue("object2PropertiesMappings",typeof(List<Object2PropertiesMapping>));
		EZR_VERSION = info.GetString("EZR_VERSION");
		recordingInterval = (float)info.GetValue("recordingInterval",typeof(float));
	}*/
	
	public Object2PropertiesMappingWrapper(List<Object2PropertiesMapping> mappings) {
		object2PropertiesMappings = mappings;
	}
	
	public Object2PropertiesMappingWrapper() {

	}
	
	public void addMapping(Object2PropertiesMapping mapping) {
		object2PropertiesMappings.Add(mapping);
	}

#if !IS_UNLICENSED

    //serialize and save (do not call directly, call saveToFile() instead)
    public string SerializeObject() {

        string serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

        if (EZReplayManager.get.compressSaves) {
            return CompressString.StringCompressor.CompressString(serialized);
        } else {
            return serialized;
        }
    }

    public static Object2PropertiesMappingWrapper DeSerializeObject(string data) {

        if (EZReplayManager.get.compressSaves) {
            try {
                data = CompressString.StringCompressor.DecompressString(data);
            } catch (FormatException) {
                if (EZReplayManager.showWarnings)
                    Debug.LogWarning("EZReplayManager WARNING: Decompressing was unsuccessful. Trying without decompression.");
            } catch (OutOfMemoryException) {
                if (EZReplayManager.showWarnings)
                    Debug.LogWarning("EZReplayManager WARNING: Decompressing was unsuccessful. Trying without decompression.");
            } catch (OverflowException) {
                if (EZReplayManager.showWarnings)
                    Debug.LogWarning("EZReplayManager WARNING: Decompressing was unsuccessful. Trying without decompression.");
            }
        }

        return JsonConvert.DeserializeObject<Object2PropertiesMappingWrapper>(data);
    }

    /*public byte[] SerializeObjectToBinary(BinarySerializationMethod serializationMethod) {

        byte[] serialized = new byte[0];
        if (serializationMethod == BinarySerializationMethod.BINARY_JSON) {
            serialized = this.ToBson();
        } else if (serializationMethod == BinarySerializationMethod.LEGACY_CSHARP_SERIALIZATION) {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(memStream, this);
            serialized = memStream.ToArray();
        }

        if (EZReplayManager.get.compressSaves) {
            return CLZF2.Compress(serialized);
        } else {
            return serialized;
        }
    }*/

    //deserialize and return for loading (do not call directly, call load() instead)
    /*public static Object2PropertiesMappingWrapper DeSerializeObjectFromBinary(byte[] data, BinarySerializationMethod serializationMethod) {
        Object2PropertiesMappingWrapper objectToDeserialize = null;

        try {
            data = CLZF2.Decompress(data);
        } catch (OutOfMemoryException) {
            if (EZReplayManager.showWarnings)
                Debug.LogWarning("EZReplayManager WARNING: Decompressing was unsuccessful. Trying without decompression.");
        } catch (OverflowException) {
            if (EZReplayManager.showWarnings)
                Debug.LogWarning("EZReplayManager WARNING: Decompressing was unsuccessful. Trying without decompression.");
        }

        if (serializationMethod == BinarySerializationMethod.BINARY_JSON) {
            objectToDeserialize = BsonSerializer.Deserialize<Object2PropertiesMappingWrapper>(data);
        } else if (serializationMethod == BinarySerializationMethod.LEGACY_CSHARP_SERIALIZATION) {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToDeserialize = (Object2PropertiesMappingWrapper)bFormatter.Deserialize(stream);
        }

        //print("System.GC.GetTotalMemory(): "+System.GC.GetTotalMemory(false));        

        return objectToDeserialize;
    }*/
#endif

    /*public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        info.AddValue("object2PropertiesMappings", this.object2PropertiesMappings);
        info.AddValue("EZR_VERSION", EZR_VERSION);
        info.AddValue("recordingInterval", this.recordingInterval);
        //base.GetObjectData(info, context);
    }*/
}
