import { CodeLens, Range } from "vscode";

export class AzureCostEstimatorCodeLens extends CodeLens {
    constructor(public estimation: number, range: Range) {
        super(range);
    }
}