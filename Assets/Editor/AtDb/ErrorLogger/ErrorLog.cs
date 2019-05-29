using System;
using System.Collections.Generic;

namespace AtDb.ErrorLogger
{
    public class ErrorLogger
    {
        public bool CheckForErrors(IEnumerable<IErrorReporter> reporters)
        {
            bool hasError = false;
            bool hasWarning = false;

            List<string> errors = new List<string>();
            List<string> warnings = new List<string>();

            foreach (IErrorReporter reporter in reporters)
            {
                if (reporter.HasWarning)
                {
                    hasWarning = true;
                    warnings.AddRange(reporter.Warnings);
                }

                if (reporter.HasError)
                {
                    hasError = true;
                    errors.AddRange(reporter.Errors);
                }
            }

            if(hasError)
            {
                PrintErrors(errors);
            }

            if (hasWarning)
            {
                PrintWarnings(errors);
            }

            return hasError;
        }

        private void PrintErrors(List<string> errors)
        {
            throw new NotImplementedException();
        }

        private void PrintWarnings(List<string> errors)
        {
            throw new NotImplementedException();
        }
    }
}