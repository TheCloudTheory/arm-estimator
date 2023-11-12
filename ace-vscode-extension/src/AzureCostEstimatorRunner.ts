import * as vscode from 'vscode';
import { ChildProcess, exec, ExecException } from 'child_process';

export class AzureCostEstimatorRunner {

    private aceLocation: string | undefined;
    private subscriptionId: string | undefined;
    private resourceGroupName: string | undefined;

    constructor() {
        let configuration = vscode.workspace.getConfiguration();

        this.aceLocation = configuration.get<string>('ace.location');
        this.subscriptionId = configuration.get<string>('ace.subscriptionId');
        this.resourceGroupName = configuration.get<string>('ace.resourceGroupName');

    }

    async run(templatePath: string): Promise<any> {
        if (!this.aceLocation || !this.subscriptionId || !this.resourceGroupName) {
            vscode.window.showWarningMessage("Azure Cost Estimator configuration is not complete. Make sure you entered all required information in the extension configuration.");
            return new Promise(() => { });
        }

        const command = `${this.aceLocation} ${templatePath} ${this.subscriptionId} ${this.resourceGroupName} --silent --generateJsonOutput --stdout`;
        return await this.execShellCommand(command);
    }

    execShellCommand(command: string) {
        const exec = require('child_process').exec;
        return new Promise((resolve, reject) => {
            exec(command, (error: ExecException, stdout: string, stderr: string) => {
                if (error) {
                    console.warn(error);
                }

                resolve(stdout ? stdout : stderr);
            });
        });
    }
}