using NUnit.Framework;

namespace NUnitTestTextInteractor
{
    using System.IO;
    using System.Reflection;
    using TextInteractor;

    /// <summary>
    /// Unit test for the TextFile Basic functionalities.
    /// </summary>
    public class UnitTest
    {
        TextFile fileEmpty;
        TextFile fileOneLine;
        TextFile fileMultipleLines;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmpty.txt"))
            {
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmptySame.txt"))
            {
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextOneLine.txt"))
            {
                file.WriteLine("There is only one line here.");
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextMultiLine.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }

            //same text as multi line

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSame.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSameWhiteSpaceCaseSensitive.txt"))
            {
                file.WriteLine("     There is a few LI  NES here.      ");
                file.WriteLine("  one here.   ");
                file.WriteLine("    AND on    e Here.   ");
                file.WriteLine("      MA   NY Lines.....     ");
                file.WriteLine(" Much WOW....");
            }

            // simmilar text as multi line
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilar.txt"))
            {
                file.WriteLine("This line should be ignored");
                file.WriteLine("Two Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines..bye");
                file.WriteLine("there is nothing here to see.");
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilarWhiteSpaceCaseSensitive.txt"))
            {
                file.WriteLine("This line should be ignored");
                file.WriteLine("two h  ere.   ");
                file.WriteLine("and o  ne here.    ");
                file.WriteLine("    MA  NY Lines . . bye");
                file.WriteLine("there is nothing here to see.");
            }
        }

        [SetUp]
        public void SetUp()
        {
            fileEmpty = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmpty.txt");
            fileOneLine = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextOneLine.txt");
            fileMultipleLines = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextMultiLine.txt");
        }

        [TearDown]
        public void TearDown()
        {
            fileEmpty.Close();
            fileOneLine.Close();
            fileMultipleLines.Close();
        }

        [Test]
        public void TestOpenClose()
        {
            Assert.IsTrue(fileEmpty.Open(), "File should open.");
            Assert.IsTrue(fileEmpty.Opened, "File should be opened.");

            Assert.AreEqual(fileEmpty.FilePath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmpty.txt", "This is the wrong file to open.");

            Assert.IsTrue(fileEmpty.Close(), "File should close");
            Assert.IsFalse(fileEmpty.Opened, "File should be closed after closing it.");
        }

        [Test]
        public void TestReadFiles()
        {
            // Read empty file
            Assert.IsTrue(fileEmpty.Open(), "File should open.");

            Assert.IsTrue(fileEmpty.FinishedReading(), "There should be nothing to read.");
            Assert.AreEqual(fileEmpty.ReadLine(), null, "There should be no line");
            Assert.IsTrue(fileEmpty.FinishedReading(), "It should be 'done' reading.");

            Assert.IsTrue(fileEmpty.Close(), "File should close");

            //Read a file with one line.
            Assert.IsTrue(fileOneLine.Open(), "File should open.");

            Assert.IsFalse(fileOneLine.FinishedReading(), "There should be more to read.");
            Assert.AreEqual(fileOneLine.ReadLine(), "There is only one line here.", "line 1 was not read");
            Assert.IsTrue(fileOneLine.FinishedReading(), "There should be nothing left to read.");
            Assert.AreEqual(fileEmpty.ReadLine(), null, "There should be no more line");

            fileOneLine.RestartReading();

            Assert.IsFalse(fileOneLine.FinishedReading(), "There should be more to read.");
            Assert.AreEqual(fileOneLine.ReadLine(), "There is only one line here.", "line 1 was not read");
            Assert.IsTrue(fileOneLine.FinishedReading(), "There should be nothing left to read.");

            Assert.IsTrue(fileOneLine.Close(), "File should be closed");

            //Read a file with multiple Lines
            Assert.IsTrue(fileMultipleLines.Open(), "File should open.");

            Assert.IsFalse(fileMultipleLines.FinishedReading(), "There should be more to read.");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "There is a few lines here.", "line 1 was not read");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "One Here.", "line 2 was not read");

            Assert.IsFalse(fileMultipleLines.FinishedReading(), "There should be more to read.");

            Assert.AreEqual(fileMultipleLines.ReadLine(), "And one Here.", "line 3 was not read");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "Many Lines.....", "line 4 was not read");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "    Much Wow....", "line 5 was not read");

            Assert.AreEqual(fileMultipleLines.ReadLine(), null, "There should be no more line");
            Assert.IsTrue(fileMultipleLines.FinishedReading(), "There should be nothing left to read.");

            fileMultipleLines.RestartReading();

            Assert.IsFalse(fileMultipleLines.FinishedReading(), "There should be more to read.");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "There is a few lines here.", "line 1 was not read");
            Assert.AreEqual(fileMultipleLines.ReadLine(), "One Here.", "line 2 was not read");

            Assert.IsTrue(fileMultipleLines.Close(), "File should be closed");
        }

        [Test]
        public void TestFind()
        {
            // Happy Path.
            fileMultipleLines.Open();
            //Check for the entire line
            Assert.True(fileMultipleLines.Find("There is a few lines here.", 1), "The line [There is a few lines here.] should be found on line 1.");
            Assert.False(fileMultipleLines.Find("There is a few lines here.", 2), "The line [There is a few lines here.] should not be found on line 2.");

            //Check for a substring within the line
            Assert.True(fileMultipleLines.Find("d one H", 3), "The string [d one H] should be found on line 3.");
            Assert.False(fileMultipleLines.Find("Nonexisting", 4), "The string [Nonexisting] should not be found on line 4.");
            Assert.False(fileMultipleLines.Find("Infinite Power!!!!", 999), "Nothing should be found on line 999");

            fileMultipleLines.Close();

            //Try finding on an empty file
            fileEmpty.Open();
            Assert.False(fileEmpty.Find("Is there something?", 1), "There Should be nothing on line 1");
            Assert.False(fileEmpty.Find("There is nothing", 999), "There should be nothing on line 999");
            fileEmpty.Close();
        }
        
        [Test]
        public void TestCompareSameEmpty()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            TextFile fileToCompareSameEmpty = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmptySame.txt");

            fileEmpty.Open();
            fileToCompareSameEmpty.Open();
            fileEmpty.Compare(fileToCompareSameEmpty, result);
            fileEmpty.Close();
            fileToCompareSameEmpty.Close();
        }

        [Test]
        public void TestCompareSame()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            TextFile fileToCompareExactSame = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSame.txt");
            TextFile fileToCompareWhiteSpaceCaseSensitive = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSameWhiteSpaceCaseSensitive.txt");

            fileMultipleLines.Open();
            fileToCompareWhiteSpaceCaseSensitive.Open();
            Assert.IsTrue(fileMultipleLines.Compare(fileToCompareExactSame, result), "Files are identical.");
            Assert.IsFalse(fileMultipleLines.Compare(fileToCompareWhiteSpaceCaseSensitive, result), "Files are different.");
            Assert.IsTrue(fileMultipleLines.Compare(fileToCompareWhiteSpaceCaseSensitive, result, true, true), "Files are identical ignoring whitespace and cases.");
            fileMultipleLines.Close();
            fileToCompareWhiteSpaceCaseSensitive.Close();
        }

        [Test]
        public void TestCompareSimilar()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            TextFile fileToCompareExactSimilar = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilar.txt");
            TextFile fileToCompareWhiteSpaceCaseSensitive = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilarWhiteSpaceCaseSensitive.txt");

            fileMultipleLines.Open();
            fileToCompareWhiteSpaceCaseSensitive.Open();
            Assert.IsFalse(fileMultipleLines.Compare(fileToCompareExactSimilar, result), "Files are Different.");
            Assert.IsTrue(fileMultipleLines.Compare(fileToCompareExactSimilar, result, 2, 3, 4, 12), "Files are Same at certain lines.");
            Assert.IsFalse(fileMultipleLines.Compare(fileToCompareWhiteSpaceCaseSensitive, result, 2, 3, 4, 11), "Files are different even at the same line.");
            Assert.IsTrue(fileMultipleLines.Compare(fileToCompareWhiteSpaceCaseSensitive, result, 2, 3, 4, 11, true, true), "Files are identical at certain lines ignoring whitespace and cases.");
            fileMultipleLines.Close();
            fileToCompareWhiteSpaceCaseSensitive.Close();
        }

        [Test]
        public void TestReplaceOnce()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify1.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult1.txt"))
            {
                file.WriteLine("There is much lines here.");
                file.WriteLine("Two Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }
            TextFile fileToModify = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify1.txt");
            TextFile fileToCompare = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult1.txt");
            fileToModify.Open();
            fileToCompare.Open();
            fileToModify.Modify(0, "One];[Two");
            fileToModify.Modify(0, "a few];[much");
            fileToModify.Compare(fileToCompare, result);
            fileToModify.Close();
            fileToCompare.Close();
        }

        [Test]
        public void TestReplaceAll()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify2.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult2.txt"))
            {
                file.WriteLine("There is much lines here!");
                file.WriteLine("Two Arf!");
                file.WriteLine("And one Arf.");
                file.WriteLine("Many Lines!!!!!");
                file.WriteLine("    Much Wow!!!!");
            }
            TextFile fileToModify = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify2.txt");
            TextFile fileToCompare = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult2.txt");
            fileToModify.Open();
            fileToCompare.Open();
            fileToModify.Modify(1, "Here];[Arf");
            fileToModify.Modify(1, ".];[!");
            fileToModify.Compare(fileToCompare, result);
            fileToModify.Close();
            fileToCompare.Close();
        }

        [Test]
        public void TestReplaceLine()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify3.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult3.txt"))
            {
                file.WriteLine("Arf Arf");
                file.WriteLine("Two Here.");
                file.WriteLine("Arf Arf");
                file.WriteLine("Wow Wow Wow");
                file.WriteLine("Wow Wow Wow");
            }
            TextFile fileToModify = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify3.txt");
            TextFile fileToCompare = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult3.txt");
            fileToModify.Open();
            fileToCompare.Open();
            fileToModify.Modify(2, "1;3];[Arf Arf");
            fileToModify.Modify(2, "4-2];[Wow Wow Wow");
            fileToModify.Compare(fileToCompare, result);
            fileToModify.Close();
            fileToCompare.Close();
        }

        [Test]
        public void TestReplaceRegex()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result.txt";
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify4.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult4.txt"))
            {
                file.WriteLine("    .");
                file.WriteLine(" .");
                file.WriteLine("  .");
                file.WriteLine(" .....");
                file.WriteLine("     ....");
            }
            TextFile fileToModify = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModify4.txt");
            TextFile fileToCompare = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextModifyResult4.txt");
            fileToModify.Open();
            fileToCompare.Open();
            fileToModify.Modify(3, "//[a-zA-Z0-9]];[");
            fileToModify.Compare(fileToCompare, result);
            fileToModify.Close();
            fileToCompare.Close();
        }
    }
}