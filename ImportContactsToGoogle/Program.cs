using System.Text;

var inputFile = @"D:\google\input.txt";
var outputFile = @"D:\google\output.csv";

if (File.Exists(outputFile))
{
    File.Delete(outputFile);
}

using var readBuffer = File.OpenRead(inputFile);
using var reader = new StreamReader(readBuffer);
using var writeBuffer = new FileStream(outputFile, FileMode.OpenOrCreate);
using var writer = new StreamWriter(writeBuffer, Encoding.UTF8);
string? line;

//writer.WriteLine(string.Join(',', "Name", "Phone 1 - Value", "Phone 2 - Type"));
writer.WriteLine("Name,Given Name,Additional Name,Family Name,Yomi Name,Given Name Yomi,Additional Name Yomi,Family Name Yomi,Name Prefix,Name Suffix,Initials,Nickname,Short Name,Maiden Name,Birthday,Gender,Location,Billing Information,Directory Server,Mileage,Occupation,Hobby,Sensitivity,Priority,Subject,Notes,Language,Photo,Group Membership,Phone 1 - Type,Phone 1 - Value");
while ((line = reader.ReadLine()) != null)
{
    var parts = line.Split(' ');
    string? nameBuffer = null;
    string? number = null;
    foreach (var part in parts.Skip(1))
    {
        if (number == null)
        {
            (var isNumber, number) = tryParseMobileNumber(part);
            if (!isNumber)
            {
                nameBuffer = string.Join(' ', nameBuffer, part.Replace(",", "")).Trim();
                //nameBuffer = string.Join(' ', nameBuffer, "test").Trim();
            }
        }
        else if (number == null)
        {
            nameBuffer = string.Concat(nameBuffer, part);
        }
    }
    if (number != null)
    {
        var result = $"{nameBuffer},{nameBuffer},,,,,,,,,,,,,,,,,,,,,,,,,,,* myContacts,Mobile,{number}";
        writer.WriteLine(result);
    }
    number = null;
    nameBuffer = null;
}
writer.Flush();

static (bool IsNumber, string? Number) tryParseMobileNumber(string? text)
{
    return text is null
        ? default
        : (text.Length == 11 && text.StartsWith("09")
            ? tryParse()
            : text.Length == 10 && text.StartsWith("9")
                ? tryParse()
                : default);

    (bool, string?) tryParse()
        => (long.TryParse(text, out var number), $"0{number:##########}");
}