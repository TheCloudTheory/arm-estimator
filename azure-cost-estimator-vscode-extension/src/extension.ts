import * as vscode from 'vscode';
import { CodelensProvider } from './CodeLensProvider';

export function activate(context: vscode.ExtensionContext) {

	console.log('Congratulations, your extension "azure-cost-estimator" is now active!');

	let disposable = vscode.commands.registerCommand('azure-cost-estimator.helloWorld', () => {
		vscode.window.showInformationMessage('Hello World from Azure Cost Estimator !');
	});

	context.subscriptions.push(disposable);

	const codelensProvider = new CodelensProvider();
	vscode.languages.registerCodeLensProvider({ pattern: '**/*.bicep' }, codelensProvider);
}

export function deactivate() { }
