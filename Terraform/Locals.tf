locals {
  environment = "tst"

  # Developer IDs. Need to be hardcoded because Azure DevOps lacks the rights to get them from AD
  dev_id = "ADD YOUR AZURE ADD OBJECT ID HERE""b5487c24-b052-4003-a481-9f783fa9a187" 

  # Tags to be used with all objects that support it.
  tags = {
    project = "gft.tests"
    creator = "Terraform"
    environment = local.environment
  }
}

#Azure Client Information (to be used if needed to get infos like Subscription ID and so on)
data "azurerm_client_config" "current" {
}
