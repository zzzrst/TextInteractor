# TextInteractor
Below are links to examples of how you would use the TextInteractor class and it's methods
- [ReadLine](https://zzzrst.github.io/TextInteractor/articles/textInteractor/ReadLine.html)
- [Compare](https://zzzrst.github.io/TextInteractor/articles/textInteractor/Compare.html)
- [Find](https://zzzrst.github.io/TextInteractor/articles/textInteractor/Find.html)
- [Replace](https://zzzrst.github.io/TextInteractor/articles/textInteractor/Modify.html)

### Sample code
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
## Notes
- The when calling a method from the textInteractor object, the method will automaticaly open the file if it is not opened yet.