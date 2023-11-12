/* eslint-disable @typescript-eslint/naming-convention */

export type AzureCostEstimatorResult = {
    TotalCost: HumanReadableCost
};

export type HumanReadableCost = {
    Value: number,
    OriginalValue: number
};