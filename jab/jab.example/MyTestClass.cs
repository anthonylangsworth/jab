﻿using jab.Interfaces;
using Xunit;
using System.Linq;

namespace jab.example
{
    public class MyTestClass
        : ApiBestPracticeTestBase
    {
        /// <summary>
        /// DELETE operations should always contain a ID parameter.
        /// </summary>
        /// <param name="apiOperation"></param>
        [Theory, ApiOperationsData("path/to/swagger.json")]
        public void DeleteMethodsMustContainIdAsKeyParameter(IJabApiOperation apiOperation)
        {
            if (apiOperation.Method == NSwag.SwaggerOperationMethod.Delete)
            {
                Assert.True(apiOperation.Operation.Parameters.Count(p => p.Name == "id") > 0);
            }
        }
    }
}