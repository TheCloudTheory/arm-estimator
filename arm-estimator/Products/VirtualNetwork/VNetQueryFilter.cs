using Microsoft.Extensions.Logging;

internal class VNetQueryFilter : IQueryFilter
{
    public VNetQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq 'NOTHING_TO_FETCH'";
    }
}
