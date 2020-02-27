# ReadLine Examples
## ReadLine()
Text.txt
```
Hello world
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

        Console.Writeline(file.ReadLine());
        //Hello World

        file.Close();
    }
}
```
## RestartReading()
Text.txt
```
Hello world
There is one more line.
Goodbye.
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

        Console.Writeline(file.ReadLine());
        //Hello World

        Console.Writeline(file.ReadLine());
        //There is one more Line.

        file.RestartReading();
        Console.Writeline(file.ReadLine());
        //Hello World

        file.Close();
    }
}
```
## FinishedReading()
Text.txt
```
Hello world.
Good Bye.
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

        Console.Writeline(file.ReadLine());
        //Hello World

        Console.Writeline(file.FinishedReading());
        //False

        Console.Writeline(file.ReadLine());
        //Good Bye.
        
        Console.Writeline(file.FinishedReading());
        //True

        file.Close();
    }
}
```