using System.Collections.Generic;

namespace Shared.Models.Dto
{
    public class Error
    {
        public Error(string error)
        {
            Errors.Add(error);
        }

        public Error(List<string> errors)
        {
            Errors = errors;
        }

        public List<string> Errors { get; private set; } = new();
    }
}