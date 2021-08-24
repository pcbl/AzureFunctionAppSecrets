#The Resource Group
resource "azurerm_resource_group" "global" {
  name     = "gft-tests-secret-${local.environment}"
  location = "westeurope" 
  tags     = local.tags
}

#Vault
resource "azurerm_key_vault" "global" {
  name                            = "gft-secrets-test-${local.environment}"
  location                        = azurerm_resource_group.global.location
  resource_group_name             = azurerm_resource_group.global.name
  enabled_for_deployment          = true
  enabled_for_disk_encryption     = true
  enabled_for_template_deployment = true
  tenant_id                       = data.azurerm_client_config.current.tenant_id
  soft_delete_enabled             = false
  purge_protection_enabled        = false

  sku_name = "standard"

  #Rights to the dev
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = local.dev_id

    secret_permissions = [
      "Backup", "Delete", "Get", "List", "Purge", "Recover", "Restore", "Set"
    ]
  }

  tags = local.tags
}

#a Secret
resource "azurerm_key_vault_secret" "the_secret" {
  name         = "the-secret"
  value        = "oh-man-it-leaked"
  key_vault_id = azurerm_key_vault.global.id

  tags = local.tags
}