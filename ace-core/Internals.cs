using System.Runtime.CompilerServices;

// Allows the CLI project to access internal types (WhatIfProcessor, formatters, etc.)
// so it can pass a real IOutputFormatter directly to WhatIfProcessor.
[assembly: InternalsVisibleTo("azure-cost-estimator")]

// Allows the test project to access internal types for unit testing.
[assembly: InternalsVisibleTo("azure-cost-estimator-tests")]
