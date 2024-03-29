﻿using FluentValidation.Results;

namespace ProductManagement.Application.Responses
{
    public class ServiceResponse<T> : GenericResponse
    {
        public T? Data { get; set; }
    }

    public class ServiceCollectionResponse<T> : GenericResponse
    {
        public IEnumerable<T>? Data { get; set; }
    }

    public class GenericResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public static class ErrorResponse
    {
        public static Dictionary<string, string> ToErrorResponse(this List<ValidationFailure> input)
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
