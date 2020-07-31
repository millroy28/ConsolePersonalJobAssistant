using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace ConsoleCodeLibrary
{
    class DrawScreen
    {
        public static int XMax { get; set; }
        public static int YMax { get; set; }
        public static int XListStart { get; set; }
        public static int YListStart { get; set; }
        public static int MainHorizontalBorderLocation { get; set; }
        public static int MainVerticalBorderLocation { get; set; }
        public static char MainHorizontalBorderCharacter { get; set; }
        public static char MainVerticalBorderCharacter { get; set; }
        public static char MainVerticalHorizontalIntersectionCharacter { get; set; }
        public static char SecondaryHorizontalBorderCharacter { get; set; }
        public static char SecondaryVerticalHorizontalIntersectionCharacter { get; set; }
        public static int MaxListLength { get; set; }
        public ColorProfile Colors { get; set; }
        public string CategoryName { get; set; }
        public int ListPage { get; set; }
        public int Selection { get; set; }
        public int ContentPage { get; set; }
        public int Focus { get; set; }
        public List<KeyValuePair<string, string>> FilesAndTitles { get; set; }
        public bool ListScrollUpAllow { get; set; }
        public bool ListScrollDownAllow { get; set; }
        public bool ListEndOfList { get; set; }
        public bool ContentScrollUpAllow { get; set; }
        public bool ContentScrollDownAllow { get; set; }
        public bool ContentEndOfContent { get; set; }
        public List<NoteObject> Snippets { get; set; }
        public string ContentForClipboard { get; set; }


        //Default Constructor
        public DrawScreen()
        {
        }
        //Constructor
        public DrawScreen(int[] _screenParams, ColorProfile _colors, string _categoryName, List<KeyValuePair<string, string>> _filesAndTitles, List<NoteObject> _snippets)
        {
            XMax = _screenParams[0];
            YMax = _screenParams[1];
            XListStart = 1;
            YListStart = 2;
            MainHorizontalBorderLocation = 1;
            MainVerticalBorderLocation = 31;
            MainHorizontalBorderCharacter = '═';
            MainVerticalBorderCharacter = '│';
            MainVerticalHorizontalIntersectionCharacter = '╤';
            SecondaryHorizontalBorderCharacter = '─';
            SecondaryVerticalHorizontalIntersectionCharacter = '├';
            MaxListLength = 0;
            Colors = _colors;
            CategoryName = _categoryName;
            ListPage = 0;
            Selection = 0;
            ContentPage = 0;
            Focus = 0;
            FilesAndTitles = _filesAndTitles;
            Snippets = _snippets;
            ContentForClipboard = "";
        }
        public void MoveListSelection(bool up)
        {

            Console.SetCursorPosition(XListStart, YListStart + Selection);
            Console.Write("                              ");
            Console.SetCursorPosition(XListStart, YListStart + Selection);
            Console.ForegroundColor = Colors.ListTextHighlight;
            Console.BackgroundColor = Colors.ListTextHighlightBackground;
            Console.Write(FilesAndTitles[Selection + (MaxListLength * ListPage)].Value);
            Console.ResetColor();

            Console.ForegroundColor = Colors.ListText;
            if (up)
            {
                Console.SetCursorPosition(XListStart, YListStart + Selection + 1);
                Console.Write("                              ");
                Console.SetCursorPosition(XListStart, YListStart + Selection + 1);
                Console.Write(FilesAndTitles[Selection + (MaxListLength * ListPage) + 1].Value);
            }
            else
            {
                Console.SetCursorPosition(XListStart, YListStart + Selection - 1);
                Console.Write("                              ");
                Console.SetCursorPosition(XListStart, YListStart + Selection - 1);
                Console.Write(FilesAndTitles[Selection + (MaxListLength * ListPage) - 1].Value);
            }
            Console.ResetColor();
        }
        public void HighlightCurrentListSelectionAfterTransition()
        {
            Console.SetCursorPosition(XListStart, YListStart + Selection);
            Console.Write("                              ");
            Console.SetCursorPosition(XListStart, YListStart + Selection);
            Console.ForegroundColor = Colors.ListTextHighlight;
            Console.BackgroundColor = Colors.ListTextHighlightBackground;
            Console.Write(FilesAndTitles[Selection + (MaxListLength * ListPage)].Value);
            Console.ResetColor();
        }
        public void PrintList () //+++++++++++++++++Move the menu printing into its own method below and make it print either based on 
        {
            //ListStatus = -1 when on the last page of the list. Will break on a -1 when PgDn is pressed here disallowing further scrolling.
            //ListStatus = 1 when on the first page of the list. Will break on a 1 when PgUp is pressed here disallowing further scrolling.
            //ListStatus = 0 otherwise, allowing scrolling either way.
            //ListStatus = 2 if the list is too small for scrolling

            int listStartIndex = MaxListLength * ListPage;

            //clear list on display
            for (int i = YListStart; i < YMax; i++)
            {
                Console.SetCursorPosition(XListStart, i);
                Console.Write("                              ");
            }
            
            for (int i = YListStart, j = listStartIndex; i < YMax; i++, j++)
            {
                Console.SetCursorPosition(XListStart, i);
                if (j == FilesAndTitles.Count)  //If end of list is reached
                {
                    ListEndOfList = true;
                    DrawNavText();
                    return;
                }
                if(j == listStartIndex + MaxListLength) //If end of display length is reached
                {
                    ListEndOfList = false;
                    DrawNavText();
                    return;
                }
                Console.ForegroundColor = Colors.ListText;
                Console.Write(FilesAndTitles[j].Value);
            }
            //ListStatus = 0;
            return;
        }
        public void DrawNavText()
            {
            DrawNavTextBackground();
            Console.ForegroundColor = Colors.NavText;
            //Console.BackgroundColor = Colors.NavTextBackground; Taken care of by DrawNavTextBackground()
            int page = 0;
            bool upAllow;
            bool downAllow;
            bool end = false;

            if (Focus == 0)
            {
                page = ListPage;
                end = ListEndOfList;
                Console.SetCursorPosition(XListStart, YMax - 1);
            }   
            else if (Focus == 1)
            {
                page = ContentPage;
                end = ContentEndOfContent;
                Console.SetCursorPosition(MainVerticalBorderLocation + 2, YMax - 1);
            }

            if (end)
            {
                if (page == 0)
                {
                    Console.Write($"PAGE {page + 1}");
                    upAllow = false;
                    downAllow = false;
                } 
                else
                {
                    Console.Write($"PAGE {page + 1} PgUp: ▲");
                    upAllow = true;
                    downAllow = false;
                }
            } 
            else
            {
                if (page == 0)
                {
                    Console.Write($"PAGE {page + 1}           PgDn: ▼");
                    upAllow = false;
                    downAllow = true;
                } 
                else
                {
                    Console.Write($"PAGE {page + 1} PgUp: ▲ / PgDn: ▼");
                    upAllow = true;
                    downAllow = true;
                }
            }

            if (Focus == 0)
            {
                ListScrollUpAllow = upAllow;
                ListScrollDownAllow = downAllow;
            }
            else if (Focus == 1)
            {
                ContentScrollUpAllow = upAllow;
                ContentScrollDownAllow = downAllow;
            }

            Console.ResetColor();
        }
        private void DrawNavTextBackground()
        {
            Console.BackgroundColor = Colors.NavTextBackground;
            for (int x = 0; x < XMax; x++)
            {
                if(x != MainVerticalBorderLocation)
                {
                    Console.SetCursorPosition(x, YMax - 1);
                    Console.Write(" ");
                }
            }
        }
        public void DrawBorders ()
        {
            ClearHeaderAndContentWindow();
            Console.ForegroundColor = Colors.Border;
            VerticleBorder();
            HorizontalBorder();
            PrintCategoryTitle(CategoryName);
            Console.ResetColor();

            MaxListLength = YMax - 1 - 1 - MainHorizontalBorderLocation;
        }
        public static void VerticleBorder()
        {
            for (int i = MainHorizontalBorderLocation; i < YMax; i++)
            {
                Console.SetCursorPosition(MainVerticalBorderLocation, i);
                Console.Write(MainVerticalBorderCharacter);
            }
        }
        public static void HorizontalBorder()
        {
            for(int i = 0; i < XMax; i++)
            {
                Console.SetCursorPosition(i, MainHorizontalBorderLocation);
                Console.Write(MainHorizontalBorderCharacter);
            }
            Console.SetCursorPosition(MainVerticalBorderLocation, MainHorizontalBorderLocation);
            Console.Write(MainVerticalHorizontalIntersectionCharacter);
        }
        public void PrintCategoryTitle(string category)
        {
            Console.SetCursorPosition(2, MainHorizontalBorderLocation);
            Console.ForegroundColor = Colors.Border;
            Console.Write('╡');
            Console.ForegroundColor = Colors.TitleText;
            Console.Write(category.ToUpper());
            Console.ForegroundColor = Colors.Border;
            Console.Write('╞');
            Console.ResetColor();
        }
        public void PrintContentsHeader()
        {
            ClearHeaderArea();
            int index = Selection + (MaxListLength * ListPage);
            
            //Print Title
            Console.ForegroundColor = Colors.TitleText;
            string title = Snippets[index].Title;
            int titleStartX = ((XMax - MainVerticalBorderLocation-title.Length) / 2) + MainVerticalBorderLocation;
            int titleStartY = MainHorizontalBorderLocation + 1;
            Console.SetCursorPosition(titleStartX, titleStartY);
            Console.Write(title);

            //Print Languages & Keywords
            Console.ForegroundColor = Colors.PropertyText;
            string languages = "";
            for (int i = 0; i < Snippets[index].Language.Length; i++)
            {
                languages += Snippets[index].Language[i];
                if(i + 1 < Snippets[index].Language.Length)
                {
                    languages += ", ";
                }
            }
            int languageStartX = MainVerticalBorderLocation + 2;
            int languageStartY = MainHorizontalBorderLocation + 2;
            Console.SetCursorPosition(languageStartX, languageStartY);
            Console.Write(languages);

            string keywords = "";
            for (int i = 0; i < Snippets[index].Keywords.Length; i++)
            {
                keywords += Snippets[index].Keywords[i];
                if (i + 1 < Snippets[index].Keywords.Length)
                {
                    keywords += ", ";
                }
            }
            int keywordsStartX = XMax - (keywords.Length + 2);
            int keywordsStartY = MainHorizontalBorderLocation + 2;
            Console.SetCursorPosition(keywordsStartX, keywordsStartY);
            Console.Write(keywords);

            //Draw content bottom border
            Console.ForegroundColor = Colors.Border;
            int secondaryBorderStartX = MainVerticalBorderLocation + 1;
            int secondaryBorderStartY = MainHorizontalBorderLocation + 3;
            for (int i = secondaryBorderStartX; i < XMax; i++)
            {
                Console.SetCursorPosition(i, secondaryBorderStartY);
                Console.Write(SecondaryHorizontalBorderCharacter);
            }
            Console.SetCursorPosition(MainVerticalBorderLocation, secondaryBorderStartY);
            Console.Write(SecondaryVerticalHorizontalIntersectionCharacter);

            PrintContentsBody();
        }
        public void PrintContentsBody()
        {
            ClearContentArea();
            //clears content for clipboard
            ContentForClipboard = "";
            //prints first contents page
            int index = Selection + (MaxListLength * ListPage);
            Console.ForegroundColor = Colors.ContentText;
            int x = MainVerticalBorderLocation + 3;
            int y = MainHorizontalBorderLocation + 5;
            bool addToCopyString = false;

            foreach (string s in Snippets[index].Contents[ContentPage].ContentBlock)
            {
                if (s == ReadFile.BeginCodeSection)
                {
                    Console.ForegroundColor = Colors.CopyText;
                    addToCopyString = true;
                }
                else if (s == ReadFile.EndCodeSection)
                {
                    Console.ForegroundColor = Colors.ContentText;
                    addToCopyString = false;
                } 
                else 
                {
                    if (addToCopyString)
                    {
                        ContentForClipboard += s + '\n';
                    }
                    y += PrintContentLine(s, x, y);
                }
            }
            Console.ResetColor();

            if (Snippets[index].Contents.Count - 1 == ContentPage)
            {
                ContentEndOfContent = true;
            } 
            else
            {
                ContentEndOfContent = false;
            }

        }
        private int PrintContentLine(string line, int x, int y)
        {   //If line is so long it can't be printed in one line, will cut printable portion, write it, and call itself again with remainder
            int linesPrinted = 0;
            int maxLineLength = XMax - x - 2;
            Console.SetCursorPosition(x, y);

            if (line.Length > maxLineLength)
            {
                string remainder = line.Substring(maxLineLength);
                string truncated = line.Substring(0, maxLineLength);
                Console.Write(truncated);
                linesPrinted++;
                linesPrinted += PrintContentLine(remainder, x, y + linesPrinted);
            } else
            {
                Console.Write(line);
                linesPrinted++;
            }
            return linesPrinted;
        }
        public void ClearHeaderAndContentWindow()
        {
            ClearHeaderArea();
            ClearContentArea();
        }
        public void ClearHeaderArea()
        {
            int headerAreaStartY = MainHorizontalBorderLocation + 1;
            int secondaryBorderStartY = MainHorizontalBorderLocation + 4;
            for (int y = headerAreaStartY; y < secondaryBorderStartY; y++)
            {
                for (int x = MainVerticalBorderLocation + 1; x < XMax; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }
        public void ClearContentArea()
        {
            int secondaryBorderStartY = MainHorizontalBorderLocation + 3;
            for (int y = secondaryBorderStartY + 1; y < YMax - 1; y++)
            {
                for (int x = MainVerticalBorderLocation + 1; x < XMax; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }
        public bool PromptYesOrNo(string prompt)
        {
            string promptInstructions = "Press ENTER for yes or ESC for no:";
            int promptBoxWidth = (XMax - MainVerticalBorderLocation) / 2;
            int promptBoxXStart = (promptBoxWidth / 2) + MainVerticalBorderLocation;
            int promptBoxYStart = (YMax - MainHorizontalBorderLocation) / 2;

            Console.ForegroundColor = ConsoleColor.Red;

            //Top Border
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart);
            Console.Write("╔");
            for(int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart);
                Console.Write("═");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth -1, promptBoxYStart);
            Console.Write("╗");
            
            if (prompt.Length >= promptBoxWidth - 2) //trims string in case of message overrun - prompts will be hardcoded so overruns should only happen in odd screen size configs
            {
                prompt = prompt.Substring(0, promptBoxWidth - 2);
            }
            // Inside Box Row 1
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 1);
            Console.Write("║");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 1);
                Console.Write(" ");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 1);
            Console.Write("║");
            // Inside Box Row 2
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 2);
            Console.Write("║");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 2);
                Console.Write(" ");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 2);
            Console.Write("║");
            // Message
            int promptXStart = promptBoxXStart + 1 + (((promptBoxWidth - 2) - prompt.Length) / 2); //centering message inside box
            Console.SetCursorPosition(promptXStart, promptBoxYStart + 1);
            Console.Write(prompt);
            int promptInstructionsXStart = promptBoxXStart + 1 + (((promptBoxWidth - 2) - promptInstructions.Length) / 2); //centering message inside box
            Console.SetCursorPosition(promptInstructionsXStart, promptBoxYStart + 2);
            Console.Write(promptInstructions);
            //Bottom Border
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 3);
            Console.Write("╚");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 3);
                Console.Write("═");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 3);
            Console.Write("╝");

            do
            {

                while (!Console.KeyAvailable)
                {
                    // Wait...
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        return true;
                    case ConsoleKey.Escape:
                        return false;
                    default:
                        break;
                }
            } while (true);
        }
        public string PromptForInput(string prompt)
        {
            int promptBoxWidth = (XMax - MainVerticalBorderLocation) / 2;
            int promptBoxXStart = (promptBoxWidth / 2) + MainVerticalBorderLocation;
            int promptBoxYStart = (YMax - MainHorizontalBorderLocation) / 2;

            Console.ForegroundColor = Colors.MenuText;

            //Top Border
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart);
            Console.Write("╔");
            for(int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart);
                Console.Write("═");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth -1, promptBoxYStart);
            Console.Write("╗");
            
            if (prompt.Length >= promptBoxWidth - 2) //trims string in case of message overrun - prompts will be hardcoded so overruns should only happen in odd screen size configs
            {
                prompt = prompt.Substring(0, promptBoxWidth - 2);
            }
            // Inside Box Row 1
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 1);
            Console.Write("║");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 1);
                Console.Write(" ");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 1);
            Console.Write("║");
            // Inside Box Row 2
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 2);
            Console.Write("║");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 2);
                Console.Write(" ");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 2);
            Console.Write("║");
            // Message
            int promptXStart = promptBoxXStart + 1 + (((promptBoxWidth - 2) - prompt.Length) / 2); //centering message inside box
            Console.SetCursorPosition(promptXStart, promptBoxYStart + 1);
            Console.Write(prompt);
                     
            //Bottom Border
            Console.SetCursorPosition(promptBoxXStart, promptBoxYStart + 3);
            Console.Write("╚");
            for (int x = promptBoxXStart + 1; x < promptBoxWidth + promptBoxXStart - 1; x++)
            {
                Console.SetCursorPosition(x, promptBoxYStart + 3);
                Console.Write("═");
            }
            Console.SetCursorPosition(promptBoxXStart + promptBoxWidth - 1, promptBoxYStart + 3);
            Console.Write("╝");

            //Get Input:
            Console.SetCursorPosition(promptBoxXStart + 3, promptBoxYStart + 2);
            Console.Write(">");
            return Console.ReadLine();
        }

    }

}
