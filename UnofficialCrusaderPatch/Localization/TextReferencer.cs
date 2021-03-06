﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;

namespace UCP
{
    static class TextReferencer
    {
        struct TextRef
        {
            string keyword;
            public string Keyword { get { return this.keyword; } }

            string reference;
            public string Reference { get { return this.reference; } }

            public TextRef(string keyword, string reference)
            {
                this.keyword = keyword;
                this.reference = reference;
            }
        }

        public static void SetText(TextBlock tb, string text)
        {
            int last = 0, index = 0;
            while (index < text.Length)
            {
                if (text[index] == '[')
                {
                    int start1 = index + 1;
                    int end1 = text.IndexOf(']', start1);
                    if (end1 >= 0 && end1 < text.Length - 2 && text[end1 + 1] == '(')
                    {
                        int start2 = end1 + 2;
                        int end2 = text.IndexOf(')', start2);
                        if (end2 >= 0)
                        {
                            string url = text.Substring(start2, end2 - start2);
                            string urltext = text.Substring(start1, end1 - start1);

                            if (index > last)
                            {
                                tb.Inlines.Add(text.Substring(last, index - last));
                            }

                            Hyperlink hyperlink = new Hyperlink(new Run(urltext));
                            hyperlink.RequestNavigate += (s, e) => Process.Start(url);
                            hyperlink.NavigateUri = new Uri(url);
                            hyperlink.ToolTip = url;
                            tb.Inlines.Add(hyperlink);

                            last = end2 + 1;
                            index = end2;
                        }
                    }
                }

                index++;
            }

            if (index > last)
            {
                tb.Inlines.Add(text.Substring(last, index - last));
            }
        }
    }
}
