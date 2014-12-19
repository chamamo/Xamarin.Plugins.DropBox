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

        public DrbxReceivedMessage(string id, bool isDeleted)
            : base(id)
        {
            IsDeleted = isDeleted;
        }

        public DrbxReceivedMessage(string id, Dictionary<string, object> changes)
            : base(id)
        {
            Changes = changes;
        }
    }
}
