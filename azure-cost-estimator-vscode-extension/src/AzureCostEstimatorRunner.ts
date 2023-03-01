import * as vscode from 'vscode';
import { ChildProcess, exec } from 'child_process';

export class AzureCostEstimatorRunner {

    private aceLocation: string | undefined;
    private subscriptionId: string| undefined;
    private resourceGroupName: string| undefined;

    constructor() {       
        let configuration = vscode.workspace.getConfiguration();
        
        this.aceLocation = configuration.get<string>('ace.location');
        this.subscriptionId = configuration.get<string>('ace.subscriptionId');
        this.resourceGroupName = configuration.get<string>('ace.resourceGroupName');
        
    }

    run(templatePath: string): Promise<any> {
        if(!this.aceLocation || !this.subscriptionId || !this.resourceGroupName)
        {
            vscode.window.showWarningMessage("Azure Cost Estimator configuration is not complete. Make sure you entered all required information in the extension configuration.");
            return new Promise(() => {});
        }

        const command = `${this.aceLocation} ${templatePath} ${this.subscriptionId} ${this.resourceGroupName} --silent --generateJsonOutput --stdout`;
        const process = exec(command);

        process.stdout?.on('data', data => {
            console.log(data);
        });

        process.stderr?.on('data', err => {
            console.log(err);
        });

        return Promise.resolve([]);
    }
}