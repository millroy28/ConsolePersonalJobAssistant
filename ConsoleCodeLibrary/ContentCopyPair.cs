using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCodeLibrary
{
    class ContentCopyPair
    {
        public string[] ContentBlock { get; set; }
        public string CodeBlock { get; set; }
        public ContentCopyPair()
        {

        }

        public ContentCopyPair(string[] _contentBlock, string _codeBlock)
        {
            ContentBlock = _contentBlock;
            CodeBlock = _codeBlock;
        }
    }



}
