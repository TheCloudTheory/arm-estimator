import * as vscode from 'vscode';
import { AzureCostEstimatorCodeLens } from './AzureCostEstimatorCodeLens';

export class CodelensProvider implements vscode.CodeLensProvider {

    private codeLenses: vscode.CodeLens[] = [];
    private _onDidChangeCodeLenses: vscode.EventEmitter<void> = new vscode.EventEmitter<void>();
    private regex: RegExp;
    public readonly onDidChangeCodeLenses: vscode.Event<void> = this._onDidChangeCodeLenses.event;

    constructor() {
        this.regex = /resource .{1,} '.{1,}'/gm;
    }

    provideCodeLenses(document: vscode.TextDocument, token: vscode.CancellationToken): vscode.ProviderResult<vscode.CodeLens[]> {
        this.codeLenses = [];
        const regex = new RegExp(this.regex);
        const text = document.getText();
        let matches;
        while ((matches = regex.exec(text)) !== null) {
            const line = document.lineAt(document.positionAt(matches.index).line);
            const indexOf = line.text.indexOf(matches[0]);
            const position = new vscode.Position(line.lineNumber, indexOf);
            const range = document.getWordRangeAtPosition(position, new RegExp(this.regex));

            if (range) {
                this.codeLenses.push(new AzureCostEstimatorCodeLens(30.56, range));
            }
        }
        return this.codeLenses;
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
        }

        return null;
    }
}