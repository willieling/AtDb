using NPOI.SS.UserModel;

namespace AtDb.ErrorSystem
{
    public class NoticeContainer
    {
        public readonly string baseMessage;
        public readonly object[] args;
        public readonly ICell cell;
        public readonly IRow row;

        public NoticeContainer(ICell cell, string baseMessage, params object[] args)
        {
            this.cell = cell;
            this.baseMessage = baseMessage;
            this.args = args;
        }

        public NoticeContainer(IRow row, string baseMessage, params object[] args)
        {
            this.row = row;
            this.baseMessage = baseMessage;
            this.args = args;
        }

        public NoticeContainer(string baseMessage, params object[] args)
        {
            this.baseMessage = baseMessage;
            this.args = args;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}