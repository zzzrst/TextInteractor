using NUnit.Framework;

namespace NUnitTestTextInteractor
{
    using System.IO;
    using System.Reflection;
    using TextInteractor;
    public class UnitTest
    {
        TextFile fileEmpty;
        TextFile fileOneLine;
        TextFile fileMultipleLines;
        TextFile fileToCompareExactSame;
        TextFile fileToCompareSimilar;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmpty.txt"))
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

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSame.txt"))
            {
                file.WriteLine("There is a few lines here.");
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
                file.WriteLine("    Much Wow....");
            }

            using (var file = File.CreateText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilar.txt"))
            {
                file.WriteLine("One Here.");
                file.WriteLine("And one Here.");
                file.WriteLine("Many Lines.....");
            }
        }

        [SetUp]
        public void SetUp()
        {
            fileEmpty = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextEmpty.txt");
            fileOneLine = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextOneLine.txt");
            fileMultipleLines = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextMultiLine.txt");
            fileToCompareExactSame = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSame.txt");
            fileToCompareSimilar = new TextInteractor(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextSimilar.txt");
        }

        [TearDown]
        public void TearDown()
        {
            fileEmpty.Close();
            fileOneLine.Close();
            fileMultipleLines.Close();
            fileToCompareExactSame.Close();
            fileToCompareSimilar.Close();
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
            fileMultipleLines.Open();
            //Check for the entire line
            Assert.True(fileMultipleLines.Find("There is a few lines here.", 1), "The line [There is a few lines here.] should be found on line 1.");
            Assert.False(fileMultipleLines.Find("There is a few lines here.", 2), "The line [There is a few lines here.] should not be found on line 2.");

            //Check for a substring within the line
            Assert.True(fileMultipleLines.Find("d one H", 3), "The string [d one H] should be found on line 3.");
            Assert.False(fileMultipleLines.Find("Nonexisting", 4), "The string [Nonexisting] should not be found on line 4.");

            fileMultipleLines.Close();
        }
    }
}