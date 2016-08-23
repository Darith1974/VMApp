Add-AzureAccount
[string]$b = "hello1" + (Get-Date).Day + (Get-Date).Minute + (Get-Date).Second
$vm1 = New-AzureVMConfig -Name $b -InstanceSize "Small" -Image "a699494373c04fc0bc8f2bb1389d6106__Windows-Server-2012-R2-201409.01-en.us-127GB.vhd"
$pwd = "some@pass1"
$un = "mwasham"
$vm1 | Add-AzureProvisioningConfig -Windows -AdminUserName $un -Password $pwd
$service = $b
$location = "West Europe"
 
## Cloud Service must already exist 
##New-AzureService -ServiceName "help" -Location "West Europe"
$vm1 | New-AzureVM -ServiceName "team101" -Location "West Europe"