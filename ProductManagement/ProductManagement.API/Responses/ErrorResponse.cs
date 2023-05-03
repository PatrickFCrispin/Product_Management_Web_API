﻿using FluentValidation.Results;

namespace ProductManagement.API.Responses
{
    internal class ErrorResponse
    {
        // this method will only be called if DTO has one or more errors
        internal static Dictionary<string, string> ToErrorResult(List<ValidationFailure> input)
        {
            var index = 0;
            var errors = new Dictionary<string, string>();
            foreach (var error in input)
            {
                index++;
                errors.Add($"{index}.", error.ErrorMessage);
            };

            return errors;
        }
    }
}