const core = require('@actions/core');
const github = require('@actions/github');
const execSync = require('child_process').execSync;

try {
    console.log('Downloading Azure Cost Estimator.')
    execSync('wget https://github.com/TheCloudTheory/arm-estimator/releases/download/1.3/linux-x64.zip');
    execSync('unzip linux-x64.zip');
    execSync('chmod +x ./azure-cost-estimator');

    console.log('Running Azure Cost Estimator.')
    let command = './azure-cost-estimator';

    if(core.getInput('resource-group-name')) {
        command += ' ' + core.getInput('template-file') + ' ' + core.getInput('subscription-id') + ' ' + core.getInput('resource-group-name');
    }

    if(core.getInput('subscription-id')) {
        command += ' sub ' + core.getInput('template-file') + ' ' + core.getInput('subscription-id') + ' ' + core.getInput('location');
    }

    if(core.getInput('management-group-id')) {
        command += ' mg ' + core.getInput('template-file') + ' ' + core.getInput('management-group-id');
    }

    if(core.getInput('tenant-id')) {
        command += ' tenant ' + core.getInput('tenant-id') + ' ' + core.getInput('template-file');
    }

    execSync(command);
} catch (error) {
    core.setFailed(error.message);
}
