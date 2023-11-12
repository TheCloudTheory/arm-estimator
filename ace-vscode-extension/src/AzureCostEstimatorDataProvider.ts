import * as vscode from 'vscode';
import { AzureCostEstimatorRunner } from './AzureCostEstimatorRunner';

export class AzureCostEstimatorDataProvider implements vscode.TreeDataProvider<vscode.TreeItem> {
    private _onDidChangeTreeData: vscode.EventEmitter<vscode.TreeItem | undefined | null | void> = new vscode.EventEmitter<vscode.TreeItem | undefined | null | void>();
    private readonly runner: AzureCostEstimatorRunner;
    private data: any;

    onDidChangeTreeData: vscode.Event<any> | undefined = this._onDidChangeTreeData.event;

    constructor() {
        this.runner = new AzureCostEstimatorRunner();
    }

    refresh(): void {
        this._onDidChangeTreeData.fire();
    }

    getTreeItem(element: vscode.TreeItem): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }

    getChildren(element?: any): vscode.ProviderResult<vscode.TreeItem[]> {
        if (element) {
            return Promise.resolve(this.buildResourcesTree());
        }

        return Promise.resolve(this.buildTree());
    }

    resolveTreeItem?(item: vscode.TreeItem, element: vscode.TreeItem, token: vscode.CancellationToken): vscode.ProviderResult<vscode.TreeItem> {
        throw new Error('Method not implemented.');
    }

    private async buildTree() {
        let items: vscode.TreeItem[] = [];

        if (!vscode.window.activeTextEditor) {
            return Promise.resolve([]);
        }

        let currentlyOpenedFile = vscode.window.activeTextEditor.document.uri.fsPath;
        const result = await this.runner.run(currentlyOpenedFile);

        if (!result) {
            console.error("There was a problem running ACE.");
            return items;
        }

        this.data = JSON.parse(result);

        items.push(new vscode.TreeItem(`Total cost: ${this.data.TotalCost.Value} ${this.data.Currency}`, vscode.TreeItemCollapsibleState.None));
        items.push(new vscode.TreeItem(`Delta: ${this.data.Delta.Value} ${this.data.Currency}`, vscode.TreeItemCollapsibleState.None));
        items.push(new vscode.TreeItem(`Total resources: ${this.data.TotalResourceCount}`, vscode.TreeItemCollapsibleState.None));
        items.push(new vscode.TreeItem(`Estimated resources: ${this.data.EstimatedResourceCount}`, vscode.TreeItemCollapsibleState.None));
        items.push(new vscode.TreeItem(`Skipped resources: ${this.data.SkippedResourceCount}`, vscode.TreeItemCollapsibleState.None));
        items.push(new vscode.TreeItem(`Resources`, vscode.TreeItemCollapsibleState.Collapsed));

        return items;
    }

    private async buildResourcesTree() {
        let items: vscode.TreeItem[] = [];

        this.data.Resources.forEach((element: any) => {
            items.push(new vscode.TreeItem(element.Id, vscode.TreeItemCollapsibleState.None));
        });

        return items;
    }
}