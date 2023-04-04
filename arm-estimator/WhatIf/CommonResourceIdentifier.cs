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
        var tfType = this.otherResourceIdentifier!.Split('.')[1];

        return tfType switch
        {
            "azurerm_resource_group" => "Microsoft.Resources/resourceGroups",
            "azurerm_virtual_network" => "Microsoft.Network/virtualNetworks",
            "azurerm_container_registry" => "Microsoft.ContainerRegistry/registries",
            "azurerm_kubernetes_cluster" => "Microsoft.ContainerService/managedClusters",
            "azurerm_analysis_services_server" => "Microsoft.AnalysisServices/servers",
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

        return new CommonResourceIdentifier("terraform.azurerm_resource_group.ace_reserved.parent");
    }

    public string? GetLocation()
    {
        if (this.type == CommonResourceIdentifierType.AzureResourceManager)
        {
            return this.azureResourceIdentifier!.Location;
        }

        return this.otherResourceIdentifier!.Split('.')[2];
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
