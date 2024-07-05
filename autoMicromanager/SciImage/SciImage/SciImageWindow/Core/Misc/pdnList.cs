/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;

namespace SciImage
{
    /// <summary>
    /// A very simple linked-list class, done functional style. Use null for
    /// the tail to indicate the end of a list.
    /// </summary>
    public sealed class pdnList
    {
        private object head;
        public object Head
        {
            get
            {
                return head;
            }
        }

        private pdnList tail;
        public pdnList Tail
        {
            get
            {
                return tail;
            }
        }

        public pdnList(object head, pdnList tail)
        {
            this.head = head;
            this.tail = tail;
        }
    }
}
