import * as vscode from 'vscode';
import { AzureCostEstimatorDataProvider } from './AzureCostEstimatorDataProvider';
import { CodelensProvider } from './CodeLensProvider';

export function activate(context: vscode.ExtensionContext) {

	console.log('Azure Cost Estimator VS Code extension is active.');

	const codelensProvider = new CodelensProvider();
	const treeViewProvider = new AzureCostEstimatorDataProvider();
	vscode.commands.registerCommand('azure-cost-estimator.refreshEntry', () =>
		treeViewProvider.refresh()
	);
	vscode.languages.registerCodeLensProvider({ pattern: '**/*.bicep' }, codelensProvider);
	vscode.window.registerTreeDataProvider(
		'azure-cost-estimator:estimations',
		treeViewProvider
	);
}

export function deactivate() { }
