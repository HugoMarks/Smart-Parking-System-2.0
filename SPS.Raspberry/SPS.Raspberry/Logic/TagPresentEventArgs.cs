using SPS.Raspberry.Core.MFRC522;
using System;

namespace SPS.Raspberry.Logic
{
    public class TagPresentEventArgs : EventArgs
    {
        public TagUid TagUid { get; set; }

        public TagPresentEventArgs(TagUid tagUid)
        {
            TagUid = tagUid;
        }
    }
}
