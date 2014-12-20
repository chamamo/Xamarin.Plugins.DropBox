using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox.Messages
{
    public class DrbxReceivedMessage<TEntityType> : MvxMessage
    {
        public readonly Dictionary<string, object>  Changes;
        public readonly bool IsDeleted;
        public readonly string Id;

        public DrbxReceivedMessage(object sender, string id, bool isDeleted)
            : base(sender)
        {
            Id = id;
            IsDeleted = isDeleted;
        }

        public DrbxReceivedMessage(object sender, string id, Dictionary<string, object> changes)
            : base(id)
        {
            Id = id;
            Changes = changes;
        }
    }
}
