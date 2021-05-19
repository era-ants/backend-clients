using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clients
{
    public interface IEnum<T> where T : IEnum<T>
    {
        public int Id { get; }
    }

    // public sealed class EnumJsonConverter<T> : JsonConverter<T> where T : IEnum<T>
    // {
    //     public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //     {
    //             
    //     }
    //
    //     public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}