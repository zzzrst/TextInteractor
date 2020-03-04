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