using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Type of mobipocket document.
    /// </summary>
    public enum MobipocketType
    {    
        //2 Mobipocket Book
        MobipocketBook = 2,

        //3 PalmDoc Book
        PalmDocBook = 3,

        //4 Audio
        Audio = 4,

        //257 News
        News = 257,

        //258 News_Feed
        NewsFeed = 258,

        //259 News_Magazine
        NewsMagazine = 259,

        //513 PICS
        Pics = 513,

        //514 WORD
        Word = 514,

        //515 XLS
        Xls = 515,

        //516 PPT
        Ppt = 516,

        //517 TEXT
        Text = 517,

        //518 HTML
        Html = 518
    }
}
