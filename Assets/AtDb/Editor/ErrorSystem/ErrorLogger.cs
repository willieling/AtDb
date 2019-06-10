using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace AtDb.ErrorSystem
{
    public class ErrorLogger
    {
        public readonly Queue<NoticeContainer> errors;
        public readonly Queue<NoticeContainer> warnings;

        public ErrorLogger()
        {
            errors = new Queue<NoticeContainer>();
            warnings = new Queue<NoticeContainer>();
        }

        public void AddWarning(ICell cell, string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(cell, baseMessage, args);
            warnings.Enqueue(container);
        }

        public void AddWarning(IRow row, string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(row, baseMessage, args);
            warnings.Enqueue(container);
        }

        public void AddWarning(string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(baseMessage, args);
            warnings.Enqueue(container);
        }

        public void AddError(ICell cell, string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(cell, baseMessage, args);
            errors.Enqueue(container);
        }

        public void AddError(IRow row, string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(row, baseMessage, args);
            errors.Enqueue(container);
        }

        public void AddError(string baseMessage, params object[] args)
        {
            NoticeContainer container = new NoticeContainer(baseMessage, args);
            errors.Enqueue(container);
        }

        public void CopyNotices(IErrorLogger otherLogger)
        {
            CopyNotices(otherLogger.ErrorLogger.errors, errors);
            CopyNotices(otherLogger.ErrorLogger.warnings, warnings);
        }

        private void CopyNotices(Queue<NoticeContainer> source, Queue<NoticeContainer> destination)
        {
            foreach (NoticeContainer item in source)
            {
                destination.Enqueue(item);
            }
        }
    }
}