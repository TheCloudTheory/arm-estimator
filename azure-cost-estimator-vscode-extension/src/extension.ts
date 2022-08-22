import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {
	
	console.log('Congratulations, your extension "azure-cost-estimator" is now active!');

	let disposable = vscode.commands.registerCommand('azure-cost-estimator.helloWorld', () => {
		vscode.window.showInformationMessage('Hello World from Azure Cost Estimator !');
	});

	context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export function deactivate() {}
