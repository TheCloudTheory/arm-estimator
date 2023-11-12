import * as vscode from 'vscode';
import { ChildProcess, exec } from 'child_process';
import { AzureCostEstimatorCodeLens } from './AzureCostEstimatorCodeLens';
import { AzureCostEstimatorRunner } from './AzureCostEstimatorRunner';

export class CodelensProvider implements vscode.CodeLensProvider {

    private codeLenses: vscode.CodeLens[] = [];
    private _onDidChangeCodeLenses: vscode.EventEmitter<void> = new vscode.EventEmitter<void>();
    private regex: RegExp;

    public readonly onDidChangeCodeLenses: vscode.Event<void> = this._onDidChangeCodeLenses.event;
    private readonly runner: AzureCostEstimatorRunner;

    constructor() {
        this.regex = /resource .{1,} '.{1,}'/gm;
        this.runner = new AzureCostEstimatorRunner();
    }

    async provideCodeLenses(document: vscode.TextDocument, token: vscode.CancellationToken): Promise<vscode.CodeLens[]> {
        this.codeLenses = [];

        const result = await this.runner.run(document.fileName);
        console.log(result);
        this.generateCodeLens(document);
        return this.codeLenses;
    }

    private generateCodeLens(document: vscode.TextDocument)
    {
        const regex = new RegExp(this.regex);
        const text = document.getText();

        let matches;
        while ((matches = regex.exec(text)) !== null) {
            const line = document.lineAt(document.positionAt(matches.index).line);
            const indexOf = line.text.indexOf(matches[0]);
            const position = new vscode.Position(line.lineNumber, indexOf);
            const range = document.getWordRangeAtPosition(position, new RegExp(this.regex));

            if (range) {
                const codeLens = new AzureCostEstimatorCodeLens(30.56, range);
                this.codeLenses.push(codeLens);
            }
        }
    }

    resolveCodeLens(codeLens: vscode.CodeLens, token: vscode.CancellationToken) {
        if (codeLens instanceof AzureCostEstimatorCodeLens) {
            const lens: AzureCostEstimatorCodeLens = codeLens as AzureCostEstimatorCodeLens;
            codeLens.command = {
                title: `Estimated cost: ${lens.estimation} USD`,
                command: "codelens-sample.codelensAction",
                arguments: [codeLens]
            };

            return codeLens;
        } else {
            codeLens.command = {
                title: `Estimating cost...`,
                command: "codelens-sample.codelensAction",
                arguments: [codeLens]
            };

            return codeLens;
        }
    }
}