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

resource "azurerm_resource_group" "rg" {
  name     = "arm-estimator-tests-rg"
  location = "West Europe"
}

resource "azurerm_automation_account" "automation" {
  name                = "example-automation-account"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku_name            = "Basic"

  identity {
    type = "SystemAssigned"
  }
}