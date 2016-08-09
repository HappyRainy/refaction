﻿using System;
using System.Net.Http;
using Xero.RefactoringExercise.Domain.Context;

namespace Xero.RefactoringExercise.WebApi.Infrastructure
{
    public class WebApiAppContext : AppContext
    {
        readonly HttpRequestMessage _request;

        public WebApiAppContext(HttpRequestMessage request)
            : base(Guid.NewGuid().ToString("D"))
        {
            _request = request;
        }

    }
}