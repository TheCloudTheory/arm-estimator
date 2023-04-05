using Azure.Core;

namespace ACE.WhatIf;

/// <summary>
/// This class represents a common ID to be used for estimations and output
/// based on the template type. There're actually two different IDs, which
/// are handled by that class:
/// - common Resource ID used by ARM
/// - artificial ID generated on-the-fly for Terraform.
/// 
/// Reason for this is quite straightforward - Terraform most of the cases
/// cannot provide Resource ID as it's known for it only after apply. Therefore
/// we need to introduce a layer of abstraction over a standard ID and ensure
/// each resource ID (no matter which tool was used under the hood) behaves
/// the same way.
/// </summary>
internal class CommonResourceIdentifier
{
    private readonly CommonResourceIdentifierType type;
    private readonly ResourceIdentifier? azureResourceIdentifier;
    private readonly string? otherResourceIdentifier;

    public CommonResourceIdentifier(string resourceId)
    {
        if(resourceId.StartsWith("terraform"))
        {
            this.type = CommonResourceIdentifierType.Terraform;
            this.otherResourceIdentifier = resourceId;
        }
        else
        {
            this.type = CommonResourceIdentifierType.AzureResourceManager;
            this.azureResourceIdentifier = new ResourceIdentifier(resourceId);
        }
    }

    public string GetName()
    {
        if (this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            return this.azureResourceIdentifier!.Name;
        }

        return this.otherResourceIdentifier!.Split('.')[4];
    }

    public string? GetResourceType()
    {
        if(this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            return this.azureResourceIdentifier!.ResourceType;
        }

        return this.GetResourceTypeBasedOnTerraformIdentifier();
    }

    private string? GetResourceTypeBasedOnTerraformIdentifier()
    {
        var parts = this.otherResourceIdentifier!.Split('.');
        var tfType = parts.Length > 5 ? parts[5] : parts[1];

        return tfType switch
        {
            "azurerm_resource_group" => "Microsoft.Resources/resourceGroups",
            "azurerm_virtual_network" => "Microsoft.Network/virtualNetworks",
            "azurerm_container_registry" => "Microsoft.ContainerRegistry/registries",
            "azurerm_kubernetes_cluster" => "Microsoft.ContainerService/managedClusters",
            "azurerm_analysis_services_server" => "Microsoft.AnalysisServices/servers",
            "azurerm_api_management" => "Microsoft.ApiManagement/service",
            "azurerm_app_configuration" => "Microsoft.AppConfiguration/configurationStores",
            "azurerm_application_gateway" => "Microsoft.Network/applicationGateways",
            "azurerm_public_ip" => "Microsoft.Network/publicIPAddresses",
            "azurerm_subnet" => "Microsoft.Network/virtualNetworks/subnets",
            _ => null,
        };
    }

    public CommonResourceIdentifier? GetParent()
    {
        if (this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            if (this.azureResourceIdentifier!.Parent?.Name == null) return null;

            return new CommonResourceIdentifier(this.azureResourceIdentifier!.Parent!);
        }

        var parts = this.otherResourceIdentifier!.Split('.');
        return new CommonResourceIdentifier($"terraform.{parts[1]}.{parts[2]}.{parts[3]}.{parts[4]}");
    }

    public string? GetLocation()
    {
        if (this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            return this.azureResourceIdentifier!.Location;
        }

        return this.otherResourceIdentifier!.Split('.')[7];
    }

    public override string ToString()
    {
        if (this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            return this.azureResourceIdentifier!;
        }

        return this.otherResourceIdentifier!;
    }
}

internal enum CommonResourceIdentifierType
{
    AzureResourceManager,
    Terraform
}
