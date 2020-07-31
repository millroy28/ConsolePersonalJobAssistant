
using System.Collections.Generic;


namespace ConsoleCodeLibrary
{
    class NoteObject
    {
        public string Title { get; set; }
        public string[] Language { get; set; }
        public string[] Keywords { get; set; }
        public List<ContentCopyPair> Contents { get; set; }



        public NoteObject()
        {
        }

        public NoteObject (string _title, string[] _language, string[] _keywords, List<ContentCopyPair> _contents)
        {
            Title = _title;
            Language = _language;
            Keywords = _keywords;
            Contents = _contents;
        }


    }
}
