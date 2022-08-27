import * as vscode from 'vscode';
import { CodelensProvider } from './CodeLensProvider';

export function activate(context: vscode.ExtensionContext) {

	console.log('Azure Cost Estimator VS Code extension is active.');

	const codelensProvider = new CodelensProvider();
	vscode.languages.registerCodeLensProvider({ pattern: '**/*.bicep' }, codelensProvider);

	let statusBar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Right, 100);
	statusBar.text = 'ACE: Idle';
	statusBar.show();
	
	context.subscriptions.push(statusBar);
}

export function deactivate() { }
