﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace jab.Extensions
{
    public class ActiveTheoryTestCase : XunitTheoryTestCase
    {
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public ActiveTheoryTestCase() { }

        public ActiveTheoryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, ITestMethod testMethod)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod) { }

        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                        IMessageBus messageBus,
                                                        object[] constructorArguments,
                                                        ExceptionAggregator aggregator,
                                                        CancellationTokenSource cancellationTokenSource)
        {
            // Duplicated code from SkippableFactTestCase. I'm sure we could find a way to de-dup with some thought.
            var skipMessageBus = new ActiveFactMessageBus(messageBus);

            var envVar = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);

            if (envVar.Contains("ACTIVE_TESTS"))
            {
                var result = await base.RunAsync(diagnosticMessageSink, skipMessageBus, constructorArguments, aggregator, cancellationTokenSource);
                if (skipMessageBus.DynamicallySkippedTestCount > 0)
                {
                    result.Failed -= skipMessageBus.DynamicallySkippedTestCount;
                    result.Skipped += skipMessageBus.DynamicallySkippedTestCount;
                }

                return result;
            } else
            {
                RunSummary skipResult = new RunSummary();
                skipResult.Skipped += 1;
                return skipResult;
            }

            
        }
    }

}
