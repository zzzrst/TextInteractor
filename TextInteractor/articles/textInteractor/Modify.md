# Modify Examples
All string args are written in the format TOREPLACE];[REPLACEWITH
## bool Modify(int replaceType, string args)
### ReplaceOnce
replaceType = 0

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
        file.Replace(0,"Word];[Nothing");
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
replaceType = 1

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
        file.Replace(1,"Word];[Nothing");
        file.Close();
    }
}
```
Text.txt after running
```
Words Nothing word
Nothing words WORD!
```
### ReplaceLine
replaceType = 2

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
        file.Replace(2,"1,3];[Wow");
        file.Replace(2,"4-2];[Cool");
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
### ReplaceRegex
replaceType = 3  
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
        file.Replace(3,"[0-9]];[");
        file.Close();
    }
}
```
Text.txt after running
```
RMv A Th Numbr!
```