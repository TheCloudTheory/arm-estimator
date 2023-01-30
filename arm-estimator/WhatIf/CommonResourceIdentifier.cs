using Azure.Core;

namespace ACE.WhatIf;

public class CommonResourceIdentifier
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

        return this.otherResourceIdentifier!.Split('.')[3];
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
