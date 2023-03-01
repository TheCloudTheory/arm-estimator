import * as vscode from 'vscode';
import { AzureCostEstimatorRunner } from './AzureCostEstimatorRunner';

export class AzureCostEstimatorDataProvider implements vscode.TreeDataProvider<vscode.TreeItem> {
    onDidChangeTreeData?: vscode.Event<any> | undefined;
    private readonly runner: AzureCostEstimatorRunner;

    constructor() {       
        this.runner = new AzureCostEstimatorRunner();
    }

    getTreeItem(element: vscode.TreeItem): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }

    getChildren(element?: any): vscode.ProviderResult<vscode.TreeItem[]> {
        if(!vscode.window.activeTextEditor)
        {
            return Promise.resolve([]);
        }

        let currentlyOpenedFile = vscode.window.activeTextEditor.document.uri.fsPath;
        return this.runner.run(currentlyOpenedFile);
    }

    resolveTreeItem?(item: vscode.TreeItem, element: vscode.TreeItem, token: vscode.CancellationToken): vscode.ProviderResult<vscode.TreeItem> {
        throw new Error('Method not implemented.');
    }
    
}