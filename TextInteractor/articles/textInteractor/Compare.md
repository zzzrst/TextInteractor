# Compare Examples
By default, white spaces and cases are NOT ignored.
## bool Compare(TextFile txtFile, string resultFilePath)
Text1.txt
```
Here is a document.
Filled with text.
Have fun!
```
Text2.txt
```
Here is a document.
Filled with text.
Have fun!
```
Text3.txt
```
There is one Document,
That is not like the rest.
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file1 = new TextInteractor("Text1.txt")
        TextFile file2 = new TextInteractor("Text2.txt")
        TextFile file3 = new TextInteractor("Text3.txt")

        Console.Writeline(file1.Compare(file2,"Temp/Result.txt"));
        //true
        Console.Writeline(file1.Compare(file3,"Temp/Result.txt"));
        //false
    }
}
```
## bool Compare(TextFile txtFile, string resultFilePath, bool ignoreWhitespace, bool caseInsensitive)
Text1.txt
```
Here is a document.
Filled with text.
Have fun!
```
Text2.txt
```
HERE i s a doc Ument.
Fill ed with TEXT.
Ha ve fu n!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file1 = new TextInteractor("Text1.txt")
        TextFile file2 = new TextInteractor("Text2.txt")

        Console.Writeline(file1.Compare(file2,"Temp/Result.txt",true,true));
        //true
    }
}
```
## bool Compare(TextFile txtFile, string resultFilePath, int startingLine, int startingIndex, int endingLine, int endingIndex)
The startingLine is exclusive. The endingLine is inclusive. The startingIndex and endingIndex are based off the substring syntax in c#.  

Text1.txt
```
Hello There
This document says hi.
Now its time
to say goodbye
```
Text2.txt
```
Heya!
Some document says hi.
Now its here
to write more code!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file1 = new TextInteractor("Text1.txt")
        TextFile file2 = new TextInteractor("Text2.txt")

        Console.Writeline(file1.Compare(file2,"Temp/Result.txt",1,4,3,8));
        //true
    }
}
```
## bool Compare(TextFile txtFile, string resultFilePath, int startingLine, int startingIndex, int endingLine, int endingIndex, bool ignoreWhitespace, bool caseInsensitive)
Text1.txt
```
Hello There
This document says hi.
Now its time
to say goodbye
```
Text2.txt
```
HE YA!
SO ME DOC ument says hi.
Now it s HE re
to write more code!
```

TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file1 = new TextInteractor("Text1.txt")
        TextFile file2 = new TextInteractor("Text2.txt")

        Console.Writeline(file1.Compare(file2,"Temp/Result.txt",1,4,3,6,true,true));
        //true
    }
}
```