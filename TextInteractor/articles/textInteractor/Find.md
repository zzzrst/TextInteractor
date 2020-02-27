# Find Examples
## bool Find(string expectedString, int line)
Text.txt
```
There is a few lines here.
One Here.
And one Here.
Many Lines.....
    Much Wow....
```
Program.cs
TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")

        file.Open();

        Console.Writeline(file.Find("There is a few lines here.", 1));
        //True

                Console.Writeline(file.Find("Nonexisting", 4));
        //False

                Console.Writeline(file.Find("Infinite Power!!!!", 999));
        //False

        file.Close();
    }
}
```
## bool Find(string expectedString)
Text.txt
```
There is a few lines here.
One Here.
And one Here.
Many Lines.....
    Much Wow....
```
Program.cs
TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")

        file.Open();

        Console.Writeline(file.Find("There is a few lines here."));
        //True

                Console.Writeline(file.Find("Nonexisting"));
        //False

        file.Close();
    }
}
```
## int FindAndCount(string ExpectedString)
Text.txt
```
The World goes round
and round and
round all over
the place.
```
Program.cs
TextComparer.cs
```c#
public class TextComparer
{
    using TextInteractor;

    public static Main(string[] args)
    {
        TextFile file = new TextInteractor("Text.txt")

        file.Open();

        Console.Writeline(file.FindAndCount("round"));
        //3

        file.Close();
    }
}
```