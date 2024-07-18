# MessagePack

---

MessagePack is an efficient binary serialization format. It lets you exchange data among multiple languages like JSON but it's faster and smaller. For example, small integers (like flags or error code) are encoded into a single byte, and typical short strings only require an extra byte in addition to the strings themselves.

## Installation
You need to install:

- MessagePack.package in [here](https://github.com/MessagePack-CSharp/MessagePack-CSharp/releases)
- Nuget.package in [here](https://github.com/GlitchEnzo/NuGetForUnity/releases)

Import package to Unity :

- MessagePackAnalyzer ở trong Nuget
    
Setup :scroll:  [Here](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#unity-support)

If build <b>II2CPP</b> -->  [Warning](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#aot-code-generation-support-for-unityxamarin)

global
```bash
dotnet tool install --global MessagePack.Generator
```
local
```bash
dotnet new tool-manifest
dotnet tool install MessagePack.Generator
```

```bash
Usage: mpc [options...]

Options:
  -i, -input <String>                                Input path to MSBuild project file or the directory containing Unity source files. (Required)
  -o, -output <String>                               Output file path(.cs) or directory(multiple generate file). (Required)
  -c, -conditionalSymbol <String>                    Conditional compiler symbols, split with ','. (Default: null)
  -r, -resolverName <String>                         Set resolver name. (Default: GeneratedResolver)
  -n, -namespace <String>                            Set namespace root name. (Default: MessagePack)
  -m, -useMapMode <Boolean>                          Force use map mode serialization. (Default: False)
  -ms, -multipleIfDirectiveOutputSymbols <String>    Generate #if-- files by symbols, split with ','. (Default: null)
```
mpc targets C# code with [MessagePackObject] or [Union] annotations.

// Simple Sample:
```bash
dotnet mpc -i "Assets" -o "Assets\Generated\MessagePackGenerated.cs"
```
--- 

## Usage
Here's a simple example of how to use MessagePack in C#:

```csharp
using MessagePack;

[MessagePackObject]
public class MyClass
{
    [Key(0)]
    public int Id { get; set; }

    [Key(1)]
    public string Name { get; set; }
}

var instance = new MyClass
{
    Id = 1,
    Name = "Foo"
};

// Serialize to MessagePack format
byte[] bytes = MessagePackSerializer.Serialize(instance);

// Deserialize from MessagePack format
MyClass deserialized = MessagePackSerializer.Deserialize<MyClass>(bytes);

In this example, MyClass is marked with the MessagePackObject attribute, and its properties are marked with the Key attribute. The MessagePackSerializer.Serialize method is used to serialize the instance into a byte array, and the MessagePackSerializer.Deserialize method is used to deserialize the byte array back into an instance.
```

---
## API
[High-Level API (MessagePackSerializer)](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#high-level-api-messagepackserializer)

[Low-Level API (IMessagePackFormatter<T\>)](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#low-level-api-imessagepackformattert)

[Primitive API (MessagePackWriter, MessagePackReader)](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#primitive-api-messagepackwriter-messagepackreader)

[Main Extension Point (IFormatterResolver)](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#main-extension-point-iformatterresolver)

[Attribute](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#messagepackformatterattribute)

---

## Benefits
MessagePack has several benefits:

- It's faster and smaller than JSON.
- It's language-neutral, making it a good choice for data interchange between different programming languages.
- It supports complex data types, including custom classes and collections.

---
## Optimizing for Mobile
When using MessagePack in mobile environments, consider the following optimizations:

- Use Contractless Resolvers: These allow for quick setup and smaller codebases, which are ideal for the limited resources available on mobile devices.
- Cache Serialized Data: Improve performance by caching frequently accessed data in its serialized form.
- Asynchronous Operations: Perform serialization and deserialization asynchronously to maintain UI responsiveness.

---
## Further Resources
For more information, check out the official MessagePack documentation.
[github]: https://github.com/MessagePack-CSharp/MessagePack-CSharp

---
If you need more detailed explanations or additional sections, just let me know at sonpd2302@gmail.com!
