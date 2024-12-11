using NUnit.Framework;
using System.Drawing.Printing;

namespace jtifedit3.Tests
{
    public class Class1
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void PaperSizes()
        {
            foreach (var name in PrinterSettings.InstalledPrinters.Cast<string>())
            {
                Console.WriteLine($"# {name}");
                var printerSettings = new PrinterSettings();
                printerSettings.PrinterName = name;

                foreach (var one in printerSettings.PaperSizes.Cast<PaperSize>())
                {
                    var paperSize = one as PaperSize;
                    Console.WriteLine($"- {paperSize.PaperName} {paperSize.Width}x{paperSize.Height}");
                }
            }
        }
    }
}
