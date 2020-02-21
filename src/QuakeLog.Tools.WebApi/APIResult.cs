using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuakeLog.Tools.WebApi
{
    public class APIResult
    {
        /// <summary>
        /// Content to response in json
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Status result OK or ERROR
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// List of messages
        /// </summary>
        public List<object> Messages { get; set; }

        /// <summary>
        /// Constructor APIResult
        /// </summary>
        public APIResult()
        {
            Status = "OK";

            Messages = new List<object>
            {
                new
                {
                    type = "SUCCESS",
                    text = "Operação realizada com sucesso."
                }
            };
        }

        /// <summary>
        /// Constructor APIResult using IActionResult
        /// </summary>
        public APIResult(IActionResult result) : this((result as ObjectResult)?.Value) { }

        /// <summary>
        /// Constructor APIResult with values to result
        /// </summary>
        /// <param name="content">Content to create the API result</param>
        public APIResult(object content) : this()
        {
            if (content == null) return;

            if (content is Exception ex) //Creates an error result
            {
                Status = "ERROR";

                var message = string.Empty;

                if (ex.Data != null && ex.Data.Contains("CorrelationId"))
                    message = $"Falha ao processar a requisição. Código de erro: {ex.Data["CorrelationId"].ToString()}.";
                else
                    message = ex.GetBaseException().Message;

                Messages = new List<object>
                {
                    new
                    {
                        type = "ERROR",
                        text = message,
                        trace = ex.GetType() != typeof(ArgumentException) && ex.InnerException != null ? ex.GetBaseException().StackTrace : null
                    }
                };
            }
            else if (content is ActionContext context) //creates a FluentValidator error response
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                Status = "ERROR";

                Messages = new List<object>();

                errors.ForEach(e =>
                {
                    Messages.Add(new { type = "ERROR", text = e });
                });
            }
            else //Creates an Ok response
            {
                Messages = new List<object>(0);
                this.Content = content;
            }
        }
    }
}