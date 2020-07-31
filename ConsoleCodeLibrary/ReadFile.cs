
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;

namespace ConsoleCodeLibrary
{
    class ReadFile
    {
        public const char TitleBeginAndEnd = '\'';
        public const char CommentBeginAndEnd = '/';
        public const char KeywordsBegin = '<';
        public const char KeywordsEnd = '>';
        public const char LanguageBegin = '[';
        public const char LanguageEnd = ']';
        public const string BeginContentSection = ":CONTENT:";
        public const string BeginCodeSection = ":CODE:";
        public const string EndCodeSection = ":ENDCODE:";
        public const string ContentPageBreak = ":BREAK:";

        
        public static List<string[]> ReadFileContents(string filePath)
        {
            List<string> rawFileContents = FileIO.GetFile(filePath);
            List<string[]> parsedContents = new List<string[]>();
            parsedContents.Add(ParseFileTitle(rawFileContents));
            parsedContents.Add(ParseFileLanguage(rawFileContents));
            parsedContents.Add(ParseFileKeywords(rawFileContents));
            parsedContents.Add(ParseFileContents(rawFileContents));
            return parsedContents;
        }

        public static NoteObject ParseAndReturnSnippet(string filePath)
        {   //List should contain 4 elements: Title, Languages, Keywords, Content
            List<string[]> fileContent = ReadFileContents(filePath);
            string copyableCode = "";
            bool codeTag = false;

            List<ContentCopyPair> contents = new List<ContentCopyPair>();
            List<string> pageOfContent = new List<string>();
            foreach (string s in fileContent[3])
            {
                if(s == BeginCodeSection)
                {
                    codeTag = true;
                    //continue;
                } 
                else if (s == EndCodeSection)
                {
                    codeTag = false;
                    //continue;
                }
                if (codeTag)
                {
                    copyableCode += s + '\n';
                }
                if (s == ContentPageBreak)
                {
                    contents.Add(new ContentCopyPair(pageOfContent.ToArray(), copyableCode));
                    pageOfContent.Clear();
                    copyableCode = "";
                }
                else 
                {
                    pageOfContent.Add(s);
                }
            }
            contents.Add(new ContentCopyPair(pageOfContent.ToArray(), copyableCode));
            
            string title = "";          //converting title from string array (with one element, granted) to string
            foreach(string s in fileContent[0])
            {
                title += s;
            }
            NoteObject snippet = new NoteObject
            {
                Title = title,
                Language = fileContent[1],
                Keywords = fileContent[2],
                Contents = contents
            };
            return snippet;
        }
        public static string[] ParseFileLanguage(List<string> rawFileContents)
        {
            foreach (string s in rawFileContents)
            {
                if (s.StartsWith(LanguageBegin) && s.EndsWith(LanguageEnd))
                {
                    string[] languages = s.Split(',');
                    languages[0] = languages[0].Replace(LanguageBegin.ToString(), "");
                    languages[languages.Length - 1] = languages[languages.Length - 1].Replace(LanguageEnd.ToString(), "");
                    return languages;
                }
            }
            string[] empty = new string[0];
            return empty;
        }
        public static string[] ParseFileKeywords(List<string> rawFileContents)
        {
            foreach (string s in rawFileContents)
            {
                if (s.StartsWith(KeywordsBegin) && s.EndsWith(KeywordsEnd))
                {
                    string[] keywords = s.Split(',');
                    keywords[0] = keywords[0].Replace(KeywordsBegin.ToString(), "");
                    keywords[keywords.Length - 1] = keywords[keywords.Length - 1].Replace(KeywordsEnd.ToString(),"");
                    return keywords;
                }
            }
            string[] empty = new string[0];
            return empty;
        }
        public static string[] ParseFileTitle(List<string> rawFileContents)
        {
            foreach (string s in rawFileContents)
            {
                if (s.StartsWith(TitleBeginAndEnd) && s.EndsWith(TitleBeginAndEnd))
                {
                    string[] title = { s };
                    title[0] = title[0].Replace(TitleBeginAndEnd.ToString(), "");
                    return title;
                }
            }
            string[] empty = new string[0];
            return empty;
        }
        public static string[] ParseFileContents(List<string> rawFileContents)
        {
            bool contentTag = false;
            List<string> contentBlock = new List<string>();

            foreach (string s in rawFileContents)
            {
                if (contentTag)
                {
                    contentBlock.Add(s);
                }
                if(s == BeginContentSection)
                {
                    contentTag = true;
                }
            }
            string[] content = contentBlock.ToArray();
            return content;
        }

        public static ColorProfile ReadColorProfile(string fileName)
        {   //CAUTION: A change to the order of the Color Profile class & color param file will require modifying this method to avoid out of range errors
            ColorProfile colors = new ColorProfile();
            List<string> colorProfileRawContents = FileIO.GetFile("config", fileName);
            
            string[] listText = colorProfileRawContents[0].Split(':');
            colors.ListText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), listText[1]);
            
            string[] highlightedListText = colorProfileRawContents[1].Split(':');
            colors.ListTextHighlight = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), highlightedListText[1]);
           
            string[] highlightedListTextBg = colorProfileRawContents[2].Split(':');
            colors.ListTextHighlightBackground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), highlightedListTextBg[1]);
            
            string[] border = colorProfileRawContents[3].Split(':');
            colors.Border = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), border[1]);
            
            string[] title = colorProfileRawContents[4].Split(':');
            colors.TitleText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), title[1]);
            
            string[] properties = colorProfileRawContents[5].Split(':');
            colors.PropertyText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), properties[1]);
            
            string[] content = colorProfileRawContents[6].Split(':');
            colors.ContentText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), content[1]);

            string[] copyable = colorProfileRawContents[7].Split(':');
            colors.CopyText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), copyable[1]);

            string[] navInfoTxt = colorProfileRawContents[8].Split(':');
            colors.NavText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), navInfoTxt[1]);

            string[] navInfoTxtBg = colorProfileRawContents[9].Split(':');
            colors.NavTextBackground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), navInfoTxtBg[1]);

            string[] menuText = colorProfileRawContents[10].Split(':');
            colors.MenuText = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), menuText[1]);

            string[] menuTextBg = colorProfileRawContents[11].Split(':');
            colors.MenuTextBackground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), menuTextBg[1]);

            return colors;
        }

        public static string[] ReadCategories()
        {
            List<string> rawCategories = FileIO.GetFile("config", "CATEGORIES");
            string[] categories = rawCategories.ToArray();
            foreach (string c in categories)
            {
                FileIO.CategoryDirectoryCheck(c);
            }
            return categories;            
        }

        public static List<KeyValuePair<string, string>> ReadFileTitles(string category)
        {
            //DrawScreen tempDraw = new DrawScreen();
            int maxTitleLength = 30;  // DrawScreen.MainVerticalBorderLocation - 1;
            string[] fileList = FileIO.GetFileList(category);
            string[] titles = new string[fileList.Length];
            List<KeyValuePair<string, string>> fileNamesAndTitles = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < fileList.Length; i++)
            {
                string title = FileIO.GetFirstLine(fileList[i]);
                if(title.StartsWith(TitleBeginAndEnd) && title.EndsWith(TitleBeginAndEnd))
                {
                    char[] titleChars = title.ToCharArray();
                    string cleanTitle = "";
                    for (int j = 0; j < titleChars.Length; j++)
                    {
                        if(titleChars[j] != TitleBeginAndEnd && j < maxTitleLength)
                        {
                            cleanTitle += titleChars[j].ToString();
                        }
                    }
                    fileNamesAndTitles.Add(new KeyValuePair<string,string>(fileList[i], cleanTitle));
                    
                }
            }
            return fileNamesAndTitles;
        }


    }
}
