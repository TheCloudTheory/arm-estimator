import * as vscode from 'vscode';
import { ChildProcess, exec } from 'child_process';

export class AzureCostEstimatorDataProvider implements vscode.TreeDataProvider<vscode.TreeItem> {
    onDidChangeTreeData?: vscode.Event<any> | undefined;
    
    private aceLocation: any;
    private subscriptionId: any;
    private resourceGroupName: any;

    constructor() {       
        let aceConfiguration = vscode.workspace.getConfiguration('azure-cost-estimator');
        
        this.aceLocation = aceConfiguration.get('ace.location');
        this.subscriptionId = aceConfiguration.get('ace.subscriptionId');
        this.resourceGroupName = aceConfiguration.get('ace.resourceGroupName');
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

        const command = `${this.aceLocation} ${currentlyOpenedFile} ${this.subscriptionId} ${this.resourceGroupName} --silent --generateJsonOutput --stdout`;
        console.log(command);
        const process = exec(command);

        process.stdout?.on('data', data => {
            console.log(data);
        });

        process.stderr?.on('data', err => {
            console.log(err);
        });

        return Promise.resolve([]);
    }

    resolveTreeItem?(item: vscode.TreeItem, element: vscode.TreeItem, token: vscode.CancellationToken): vscode.ProviderResult<vscode.TreeItem> {
        throw new Error('Method not implemented.');
    }
    
}