using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace PriceTagMaker
{
    public static class Printer
    {
        private static Dictionary<string, int> _receiptsOnPage = new Dictionary<string, int>
        {
            { "Стандартный", 8 },
            { "Телефонный", 15 },
            { "Большой", 3 },
            { "Огромный", 2 },
            { "Акционный", 2 },
            { "A4", 1 }
        };

        public static void Print(List<Receipt> receipts, string savepath)
        {
            if(receipts.Count > 0)
            {
                string receiptType = receipts[0].ReceiptType;
                if(receiptType == "")
                {
                    return;
                }
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                Random rndNum = new Random();
                savepath += "\\" + receiptType + "_" + DateTime.Now.ToString("ddMMyyyy_ss") + "_" + rndNum.Next(0, 100000).ToString() + ".docx";
                File.Copy(path + @"\templates\" + receiptType + ".docx", savepath);

                Application word = new Application();
                word.Visible = true;
                Document d2 = word.Documents.Open(savepath, Visible: true);

                Range oRange = d2.Content;
                oRange.Copy();

                int receiptsCount = _receiptsOnPage[receiptType];
                int receiptsOverall = receipts.Count;
                foreach (Receipt receipt in receipts)
                {

                    FindAndReplace(word, $"{{code{receiptsCount}}}", receipt.Code);
                    FindAndReplace(word, $"{{name{receiptsCount}}}", receipt.Name);
                    if(receiptType == "A4")
                    {
                        FindAndReplace(word, $"{{price{receiptsCount}}}", receipt.Price);
                        FindAndReplace(word, $"{{oldPrice{receiptsCount}}}", receipt.OldPrice);
                    }
                    else
                    {
                        FindAndReplace(word, $"{{price{receiptsCount}}}", receipt.Price + " p.");
                        FindAndReplace(word, $"{{oldPrice{receiptsCount}}}", receipt.OldPrice + " p.");
                    }
                    FindAndReplace(word, $"{{date{receiptsCount}}}", "");
                    FindAndReplace(word, $"Старая цена{receiptsCount}", "Старая цена");
                    receiptsCount--;
                    receiptsOverall--;

                    if (receiptsCount == 0 && receiptsOverall > 0)
                    {
                        oRange.Collapse(WdCollapseDirection.wdCollapseEnd);
                        oRange.InsertBreak();
                        oRange.Paste();
                        receiptsCount = _receiptsOnPage[receiptType];
                    }
                }
                while(receiptsCount > 0)
                {
                    FindAndReplace(word, $"{{code{receiptsCount}}}", "");
                    FindAndReplace(word, $"{{name{receiptsCount}}}", "");
                    FindAndReplace(word, $"{{price{receiptsCount}}}", "");
                    FindAndReplace(word, $"{{oldPrice{receiptsCount}}}", "");
                    FindAndReplace(word, $"{{date{receiptsCount}}}", "");
                    FindAndReplace(word, $"Старая цена{receiptsCount}", "");
                    receiptsCount--;
                }

                d2.Save();
            }
                       
        }

        private static void FindAndReplace(Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 1;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }
    }
}
