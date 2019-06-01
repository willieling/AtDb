using System.Collections.Generic;

namespace AtDb.ErrorLogger
{
    public interface IErrorReporter
    {
        bool HasWarning { get; }
        bool HasError { get; }

        List<string> Warnings { get; }
        List<string> Errors { get; }
    }
}