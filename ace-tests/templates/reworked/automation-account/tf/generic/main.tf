terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  features {}
  skip_provider_registration = true
}

locals {
  name = "automation${formatdate("yyyyMMddhhmmss", timestamp())}"
}

data "azurerm_resource_group" "rg" {
  name = "arm-estimator-tests-rg"
}

resource "azurerm_automation_account" "automation" {
  name                = local.name
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  sku_name            = "Basic"

  identity {
    type = "SystemAssigned"
  }
}