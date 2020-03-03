# Replace Examples
##  bool ReplaceOccurances(string toReplace, string replaceWith, int numberOfTimes = -1)
### ReplaceOnce
Text.txt
```
Words Word word
Word words WORD!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")
        file.Open();
        file.ReplaceOccurances("Word", "Nothing", 1);
        file.Close();
    }
}
```
Text.txt after running
```
Words Nothing word
Word words WORD!
```
### ReplaceAll

Text.txt
```
Words Word word
Word words WORD!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")
        file.Open();
        file.Replace("Word", "Nothing");
        file.Close();
    }
}
```
Text.txt after running
```
Words Nothing word
Nothing words WORD!
```
### ReplaceRegex
The Regex is based off of Microsoft's definition of Regexs.

Text.txt
```
R3M0v3 A11 Th3 Numb3r5!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")
        file.Open();
        file.Replace("[0-9]", "");
        file.Close();
    }
}
```
Text.txt after running
```
RMv A Th Numbr!
```
##  bool ReplaceLine(int[] lines, string replaceWith)

Text.txt
```
So
Many
Lines
To
Replace
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")
        file.Open();
        file.Replace(new int[]{1,3}, "Wow");
        file.Replace(new int[]{4,5}, "Cool");
        file.Close();
    }
}
```
Text.txt after running
```
Wow
Many
Wow
Cool
Cool
```