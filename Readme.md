# bleak.AutoConvert

AutoConvert makes it easy, using extersion methods, to AutoConvert data types within C# when Properties and/or Keys match on both object types.

## Getting Started

Install the nuget package.

### Prerequisites

* .NET Standard / Core 2.0 

### Usage

Here is some sample usage.

```
public class Source
{
    public string StrVal { get; set; }
    public string LongVal { get; set; }
    public string NullableLongValNull { get; set; }
    public string NullableLongValSet { get; set; }
    public string IntVal { get; set; }
    public string NullableIntValNull { get; set; }
    public string NullableIntValSet { get; set; }
}
```
```
public class Destination
{
    public string StrVal { get; set; }
    public long LongVal { get; set; }
    public long? NullableLongValNull { get; set; }
    public long? NullableLongValSet { get; set; }
    public int IntVal { get; set; }
    public int? NullableIntValNull { get; set; }
    public int? NullableIntValSet { get; set; }
}
```
Convert from an Object:
```
var source = new Source();
source.StrVal = "Banana";
source.LongVal = "12345";
source.IntVal = "1234";
source.NullableLongValSet = "12345";
source.NullableIntValSet = "1234";

var destination = source.AutoConvert<Destination>();
```

Convert from a Dictionary:
```
var source = new Dictionary<string, object>()
{
    { "StrVal","Banana" },
    { "LongVal",12345 },
    { "IntVal", 1234 },
    { "NullableLongValSet", 12345 },
    { "NullableIntValSet", 1234 },
};

var destination = source.AutoConvert<Destination>();
```

AutoMap a previously instantiated Object to another previously instantiated Object.

```
var id = Guid.NewGuid();
var foreignKey = Guid.NewGuid();
var source = new Object1() { Id = id, Name = "Banana", ForeignKey = foreignKey };
var destination = new Object2() { Id = id };

AutoMap.Update(source, destination);
```


### See Also

Please refer to the github project.

https://github.com/jamalkhan/AutoConvert

### License

This project is licensed with The Unlicense. 