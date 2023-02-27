import * as vscode from 'vscode';
import { AzureCostEstimatorDataProvider } from './AzureCostEstimatorDataProvider';
import { CodelensProvider } from './CodeLensProvider';

export function activate(context: vscode.ExtensionContext) {

	console.log('Azure Cost Estimator VS Code extension is active.');

	const codelensProvider = new CodelensProvider();
	vscode.languages.registerCodeLensProvider({ pattern: '**/*.bicep' }, codelensProvider);
	vscode.window.registerTreeDataProvider(
		'azure-cost-estimator:estimations',
		new AzureCostEstimatorDataProvider()
	);
}

export function deactivate() { }
